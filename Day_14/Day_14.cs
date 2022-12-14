namespace AoC2022
{    
    public class cDay_14 : WeekN
    {
        readonly string[] inputLines;
        readonly int x = 14; // x = [1..25]
        Map map;
        public cDay_14()
        {
            Title = $"--- Day {x}: xxxxxxxx xxxxxxxx ---";
            inputLines = ReadInput($"input_Day{x}.txt");
            //inputLines = ReadInput("");
            map = new(inputLines);
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
            while (map.DropSand());
            return $"{x}.1 - {map.Sands.Count-map.InitialCount}";
        }
        public override string Part2()
        {
            while (map.DropSand2()) ;
            return $"{x}.2 - {map.Sands.Count - map.InitialCount}";
        }
        class Map
        {
            public Wall Walls = new();
            public Dictionary<string,Dot> Sands = new();
            public int MaxY;
            public int InitialCount;
            public Map(string[] inputLines)
            {
                MaxY = 0;
                foreach(string line in inputLines)
                {
                    Wall wall = new();
                    string[] parts = line.Split("->",StringSplitOptions.TrimEntries);
                    Dot? LastEnd = null;
                    foreach (string part in parts)
                    {
                        if (wall.Count== 0)
                        {
                            LastEnd = new Dot(part);
                            wall.Add(LastEnd);
                        }                            
                        else
                        {
                            Dot NextEnd = new Dot(part);
                            wall.AddBetween(LastEnd, NextEnd);
                            LastEnd = NextEnd;
                        }
                        if (MaxY<LastEnd.Y)
                            MaxY=LastEnd.Y;
                    }
                    Walls.AddRange(wall);
                }                
                foreach(Dot dot in Walls)
                {
                    if(!Sands.ContainsKey(dot.ToString()))
                        Sands.Add(dot.ToString(), dot);
                }
                InitialCount=Sands.Count;
            }
            public bool DropSand()
            {
                Dot NewSand = new Dot("500,0");
                while (NewSand.InRange(this))
                    if (NewSand.Move(this) == false)
                    {
                        this.Sands.Add(NewSand.ToString(), NewSand);
                        return true;
                    }
                return false;
            }
            public bool DropSand2()
            {
                if (this.Sands.ContainsKey("500,0"))
                    return false;

                Dot NewSand = new Dot("500,0");
                while (NewSand.InRange(this))
                    if (NewSand.Move(this) == false)
                    {
                        this.Sands.Add(NewSand.ToString(), NewSand);
                        return true;
                    }
                this.Sands.Add(NewSand.ToString(), NewSand);
                return true;
            }
        }
        class Wall:List<Dot>
        {
            public void AddBetween(Dot? A, Dot B)
            {
                if (A.X > B.X)
                    for (int x = B.X; x < A.X; x++)
                        this.Add(new Dot(x, A.Y));
                else if (A.X < B.X)
                    for (int x = B.X; x > A.X; x--)
                        this.Add(new Dot(x, A.Y));
                else if (A.Y > B.Y)
                    for (int y = B.Y; y < A.Y; y++)
                        this.Add(new Dot(A.X, y));
                else if (A.Y < B.Y)
                    for (int y = B.Y; y > A.Y; y--)
                        this.Add(new Dot(A.X, y));
            }

        }
        class Dot:IEquatable<Dot>
        {
            public int X;
            public int Y;
            public Dot(int x, int y)
            {
                X = x;
                Y = y;
            }
            public Dot(string Coordinate)
            {
                string[] _X_Y = Coordinate.Split(',');
                X = int.Parse(_X_Y[0]);
                Y = int.Parse(_X_Y[1]);
            }
            public override string ToString()
            {
                return $"{X},{Y}";
            }
            public bool InRange(Map map)
            {
                return (this.Y < map.MaxY + 1);
            }
            public bool Move(Map map)
            { 
                if (!map.Sands.ContainsKey($"{this.X},{this.Y+1}"))
                {
                    this.Y++;
                    return true;
                }
                else if (!map.Sands.ContainsKey($"{this.X - 1},{this.Y + 1}"))
                {
                    this.Y++;
                    this.X--;
                    return true;
                }
                else if (!map.Sands.ContainsKey($"{this.X + 1},{this.Y + 1}"))
                {
                    this.Y++;
                    this.X++;
                    return true;
                }
                else
                    return false;
            }
            public bool Equals(Dot? other)
            {
                if (null == other) throw new NotImplementedException();
                return X == other.X && Y == other.Y;
            }
        }
    }
}