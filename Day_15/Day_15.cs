using System.Linq.Expressions;
using System.Text.RegularExpressions;
namespace AoC2022
{
    
    public class cDay_15 : WeekN
    {
        readonly string[] inputLines;
        readonly int x = 15; // x = [1..25]
        readonly Map TheMap;
        
        public cDay_15()
        {
            Title = $"--- Day {x}: Beacon Exclusion Zone ---";
            inputLines = ReadInput($"input_Day{x}.txt");
            //inputLines = ReadInput("");
            TheMap = new(inputLines);

        }
        public string[] ReadInput(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "input_sample.txt";
            }
            return File.ReadAllLines(fileName);
        }
        public override string Part1()
        {
            //TheMap.ProcessLineN(2000000);
            TheMap.ProcessLine(2000000);
            bool AtRow2000000(Beacon B) => B.y == 2000000;
            long count = TheMap.Rows[2000000].Length() - TheMap.Beacons.Values.Where(AtRow2000000).Count() ;
            return $"{x}.1 - {count}";
        }
        public override string Part2()
        {
            string debugPrint="";
            long part2 = 0;
            for (int i = 0; i< 4000000; i++)
            {
                TheMap.ProcessLine(i);
                Row R = TheMap.Rows[i].InvertRow(0, 4000000);
                if (R.Length() ==1)
                {
                    debugPrint += $"{R.ToString()},{i}//";
                    part2 = (long) R[0].Center() * 4000000 + i;
                }
            }
            return $"{x}.2 - {part2}";
        }
        class Map
        {
            public Dictionary<string, Sensor> Sensors;
            public Dictionary<string, Beacon> Beacons;
            public Dictionary<string, Coordinate> CoveredLocations;
            public Dictionary<int, Row> Rows;
            public Map(string[] inputLines)
            {
                Sensors = new Dictionary<string, Sensor>();
                Beacons = new Dictionary<string, Beacon>();
                CoveredLocations = new Dictionary<string, Coordinate>();
                foreach (string line in inputLines)
                {
                    //Sensor at x=2, y=18: closest beacon is at x=-2, y=15
                    Match m = Regex.Match(line, @"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=+(-?\d+)");
                    string CoordinateS = $"{m.Groups[1].Value},{m.Groups[2].Value}";
                    Sensors.Add(CoordinateS, new Sensor(CoordinateS));
                    string CoordinateB = $"{m.Groups[3].Value},{m.Groups[4].Value}";
                    if (!Beacons.ContainsKey(CoordinateB))
                        Beacons.Add(CoordinateB, new Beacon(CoordinateB));
                    Sensors[CoordinateS].ClosestBeacon = Beacons[CoordinateB];

                }
                Rows = new();
            }
            /// <summary>
            /// slower approach
            /// </summary>
            /// <param name="N"></param>
            public void ProcessLineN(int N)
            {
                foreach(Sensor S in Sensors.Values)
                {
                    int _X_start = S.x - S.Clearance() + Math.Abs(S.y - N); 
                    int _X_end = S.x + S.Clearance() - Math.Abs(S.y - N);
                    for (int i = _X_start; i <= _X_end; i++)
                    {
                        string Coord = $"{i},{N}";
                        if(!CoveredLocations.ContainsKey(Coord) && !Beacons.ContainsKey(Coord))
                            CoveredLocations.Add(Coord, new Coordinate(Coord));
                    }                    
                }
            }
            /// <summary>
            /// faster approach
            /// </summary>
            /// <param name="N"></param>
            public void ProcessLine(int N)
            {
                if (!Rows.ContainsKey (N))
                    Rows.Add(N, new Row() { row = N });
                foreach (Sensor S in Sensors.Values)
                {
                    int _X_start = S.x - S.Clearance() + Math.Abs(S.y - N);
                    int _X_end = S.x + S.Clearance() - Math.Abs(S.y - N);
                    if (_X_start<=_X_end)
                    Rows[N].AddSegment(new Segment(_X_start, _X_end));
                }
            }
        }
        class Beacon : Coordinate
        {
            public Beacon(string XY) : base(XY)
            {
            }
        }
        class Sensor : Coordinate
        {
            public Beacon? ClosestBeacon;
            public Sensor(string XY) : base(XY)
            {
            }
            public int Clearance()
            {
                if (ClosestBeacon == null)
                    return -1;
                else
                    return DistanceTo(ClosestBeacon);
            }
        }
        class Coordinate
        {
            public int x;
            public int y;
            public Coordinate(string XY)
            {
                string[] _X_Y = XY.Split(',');
                x=int.Parse(_X_Y[0]);
                y=int.Parse(_X_Y[1]);
            }
            public override string ToString()
            {
                return $"{x},{y}";
            }
            public int DistanceTo(Coordinate another)
            {
                return Math.Abs(x - another.x) + Math.Abs(y - another.y);
            }
        }
        class Row : List<Segment>
        {
            public int row;
            public override string ToString()
            {
                string output="";
                foreach(Segment segment in this)
                    output += segment.ToString();
                return output;
            }
            public void SubtractSegment(Segment another)
            {
                List<Segment> PendingRemove = new List<Segment>();
                List<Segment> PendingAppend = new List<Segment>();
                foreach (Segment segment in this)
                { 
                    if (segment.Intersect(another))
                    {
                        PendingRemove.Add(segment);
                        PendingAppend.AddRange(segment.Except(another));
                    }
                }
                bool PendingRemoval(Segment m) => PendingRemove.Contains(m);
                this.RemoveAll(PendingRemoval);
                this.AddRange(PendingAppend);
            }
            public void AddSegment(Segment another)
            {
                this.SubtractSegment(another);
                this.Add(another);
            }
            public Row InvertRow(int lower, int upper)
            {
                Row InvertList = new Row();

                InvertList.Add( new Segment(lower, upper));
                foreach(Segment segment in this)
                    InvertList.SubtractSegment(segment);
                return InvertList;
            }
            public int Length()
            {
                int length = 0;
                foreach (Segment S in this)
                    length += S.Length();
                return length;
            }
        }
        class Segment
        {
            int min;
            int max;
            public Segment(int min, int max)
            {
                this.min = min;
                this.max = max;
            }
            public override string ToString()
            {
                return $"[{min},{max}]";
            }
            public bool Intersect(Segment another)
            {
                return !((this.max < another.min) || (this.min > another.max));
            }
            public List<Segment> Except(Segment another)
            {
                List<Segment> segments = new List<Segment>();
                if(this.min < another.min && another.min <= this.max)
                    segments.Add(new Segment(min,another.min-1));
                if (this.min <= another.max && another.max < this.max)
                    segments.Add(new Segment(another.max+1, max));
                return segments;
            }
            public int Length()
                { return this.max - this.min + 1;}
            public int Center()
                { return (this.min + this.max)/2; }
        }
    }
}