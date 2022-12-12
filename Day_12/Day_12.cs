namespace AoC2022
{
    public class cDay_12 : WeekN
    {
        readonly string[] inputLines;
        readonly int x = 12; // x = [1..25]
        HillMap TheHill;
        public cDay_12()
        {
            Title = $"--- Day {x}: Hill Climbing Algorithm ---";
            inputLines = ReadInput($"input_Day{x}.txt");
            //inputLines = ReadInput("");
            TheHill = new(inputLines);
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
            TheHill.Flow();
            return $"{x}.1 - {TheHill.E.Steps}";
        }
        public override string Part2()
        {
            int bestScore = TheHill.E.Steps;
            foreach (Position s in TheHill.a)
            {
                TheHill.Flow(s);
                if (bestScore > TheHill.E.Steps && TheHill.E.Steps >0)
                    bestScore = TheHill.E.Steps;
            }
            return $"{x}.2 - {bestScore}";
        }

        class Position
        {
            public int Height;
            public List<Position> NextMoves;
            public int Steps=0;
            //public Position? BestMove;
            public Position(char input)
            {
                Height = Convert.ToInt32(input);
                NextMoves = new List<Position>();
            }
            public override string ToString()
            {
                //return Height.ToString();
                //return Convert.ToChar(Height).ToString();
                return Steps.ToString(" 00");
            }
            public void Flow()
            {
                foreach (Position move in NextMoves)
                {
                    if (move.Steps == 0 || move.Steps > this.Steps + 1)
                    {
                        move.Steps = this.Steps + 1;
                        //move.BestMove = this;
                        move.Flow();
                    }
                }
            }
        }

        class HillMap : Dictionary<string, Position>
        {
            readonly int iMax;
            readonly int jMax;
            readonly Position S;
            public readonly Position E;
            public readonly List<Position> a;
            public HillMap(string[] inputLines)
            {
                iMax = inputLines.Length;
                jMax = inputLines[0].Length;
                a = new List<Position>();
                for (int i = 0; i < iMax; i++)
                {
                    for (int j = 0; j < jMax; j++)
                    {
                        this.Add($"{i},{j}", new Position(inputLines[i][j]));
                        if (inputLines[i][j] == 'S')
                            S = this[$"{i},{j}"];
                        else if (inputLines[i][j] == 'E')
                            E = this[$"{i},{j}"];
                        else if (inputLines[i][j] == 'a')
                            a.Add(this[$"{i},{j}"]);
                    }
                }
                S.Height = Convert.ToInt32('a');
                E.Height = Convert.ToInt32('z');
                SCAN();
            }            
            public override string ToString()
            {
                string debugOutput = Environment.NewLine;
                for (int i = 0; i < iMax; i++)
                {
                    for (int j = 0; j < jMax; j++)
                    {
                        debugOutput += this[$"{i},{j}"].ToString();
                    }
                    debugOutput += Environment.NewLine;
                }
                return debugOutput;
            }
            private void SCAN()
            {
                for (int i = 0; i < iMax; i++)
                {
                    for (int j = 0; j < jMax; j++)
                    {
                        string me = $"{i},{j}";
                        string neighbor = $"{i},{j + 1}";
                        if (this.ContainsKey(neighbor))
                            if (this[neighbor].Height - this[me].Height <2)
                                this[me].NextMoves.Add(this[neighbor]);
                        neighbor = $"{i},{j - 1}";
                        if (this.ContainsKey(neighbor))
                            if (this[neighbor].Height - this[me].Height < 2)
                                this[me].NextMoves.Add(this[neighbor]);
                        neighbor = $"{i + 1},{j}";
                        if (this.ContainsKey(neighbor))
                            if (this[neighbor].Height - this[me].Height < 2)
                                this[me].NextMoves.Add(this[neighbor]);
                        neighbor = $"{i - 1},{j}";
                        if (this.ContainsKey(neighbor))
                            if (this[neighbor].Height - this[me].Height < 2)
                                this[me].NextMoves.Add(this[neighbor]);
                    }
                }
            }
            public void Flow()
            {
                S.Flow();
            }
            public void Flow(Position s)
            {
                foreach(Position p in this.Values)
                {
                    p.Steps = 0;
                }
                s.Flow();
            }
        }
    }
}