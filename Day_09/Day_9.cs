
namespace AoC2022
{
    
    public class cDay_9 : WeekN
    {
        readonly string[] inputLines;
        readonly int x = 9; // x = [1..25]
        readonly Rope rope;
        public cDay_9()
        {
            Title = $"--- Day {x}: Rope Bridge ---";
            inputLines = ReadInput($"input_Day{x}.txt");
            //inputLines = ReadInput("");
            rope = new();

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
            foreach(string line in inputLines)
            {
                //string[] instruction = line.Split(line, ' ');
                for (int i = 0; i < Convert.ToInt32(line[2..]); i++)
                {
                    rope.Move(line[0]);
                }
            }
            return $"{x}.1 - {rope.HistoryT.Count}";
        }
        public override string Part2()
        {
            Rope[] ropes = new Rope[10];
            for (int j = 0; j < 10; j++)
            {
                ropes[j] = new();
            }
            foreach (string line in inputLines)
            {
                //string[] instruction = line.Split(line, ' ');
                for (int i = 0; i < Convert.ToInt32(line[2..]); i++)
                {
                    ropes[0].Move(line[0]);
                    for (int j = 1; j < 10; j++)
                    {
                        ropes[j].Follow(ropes[j - 1]);
                    }
                }
            }
            return $"{x}.2 - {ropes[9].HistoryH.Count}";
        }
    }
    public class Rope
    {
        public class Knot
        {
            public int x;
            public int y;
            public int LengthTo(Knot anotherCoord)
            {
                return Math.Max(Math.Abs(anotherCoord.x - x), Math.Abs(anotherCoord.y - y));
            }
            public override string ToString()
            {
                return $"{x},{y}";
            }
            public Knot(int x, int y)
            {
                this.x = x;
                this.y = y;
            }  
        }
        public Knot H;
        public Knot T;
        public Dictionary<string,Knot> HistoryT, HistoryH;
        public Rope()
        {
            H = new(0, 0);
            T = new(0, 0);
            HistoryT = new Dictionary<string, Knot>();
            HistoryT.Add(T.ToString(), new Knot(0, 0)); 
            HistoryH = new Dictionary<string, Knot>();
            HistoryH.Add(T.ToString(), new Knot(0, 0));
        }
        public void Follow(Rope prevRope)
        {
            if (prevRope.H.LengthTo(H) > 1)
            {
                if (prevRope.H.x - H.x > 0)
                    Move('R');
                if (prevRope.H.y - H.y > 0)
                    Move('U');
                if (prevRope.H.x - H.x <0)
                    Move('L');
                if (prevRope.H.y - H.y <0)
                    Move('D');

                if (!HistoryH.ContainsKey(H.ToString()))
                {
                    HistoryH.Add(H.ToString(), new Knot(H.x, H.y));
                }
            }
        }
        public void Move(char Direction)
        {
            switch (Direction)
            {
                case 'U':
                    H.y += 1;
                    if (H.LengthTo(T) > 1)
                    {
                        T.y = H.y - 1;
                        T.x = H.x;
                        if (!HistoryT.ContainsKey(T.ToString()))
                        {
                            HistoryT.Add(T.ToString(), new Knot(T.x,T.y));
                        }
                    }
                    break;
                case 'D':
                    H.y -= 1;
                    if (H.LengthTo(T) > 1)
                    {
                        T.y = H.y + 1;
                        T.x = H.x;
                        if (!HistoryT.ContainsKey(T.ToString()))
                        {
                            HistoryT.Add(T.ToString(), new Knot(T.x, T.y));
                        }
                    }
                    break;
                case 'L':
                    H.x -= 1;
                    if (H.LengthTo(T) > 1)
                    {
                        T.x = H.x + 1;
                        T.y = H.y;
                        if (!HistoryT.ContainsKey(T.ToString()))
                        {
                            HistoryT.Add(T.ToString(), new Knot(T.x, T.y));
                        }
                    }
                    break;
                case 'R':
                    H.x += 1;
                    if (H.LengthTo(T) > 1)
                    {
                        T.x = H.x - 1;
                        T.y = H.y;
                        if (!HistoryT.ContainsKey(T.ToString()))
                        {
                            HistoryT.Add(T.ToString(), new Knot(T.x, T.y));
                        }
                    }
                    break;
            }
        }
    }

}