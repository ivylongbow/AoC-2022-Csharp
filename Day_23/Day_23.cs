namespace AoC2022
{    
    public class cDay_23 : WeekN
    {
        readonly string[] inputLines;
        readonly int x = 23; // x = [1..25]
        readonly Map TheMap;
        public cDay_23()
        {
            Title = $"--- Day {x}: Unstable Diffusion ---";
            inputLines = ReadInput($"input_Day{x}.txt");
            //inputLines = ReadInput("");
            TheMap= new(inputLines);
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
            //TheMap.DebugPrint((-3, 10), (-3, 10));
            TheMap.Play(10);
            //TheMap.DebugPrint((-3, 10), (-3, 10));

            return $"{x}.1 - {TheMap.part1result()}";
        }
        public override string Part2()
        {
            int part2result = TheMap.Play(10000);
            return $"{x}.2 - {part2result+10}";
        }

        class Map: Dictionary<string, Monkey>
        {
            //public Dictionary<string, Monkey> ProposedMap =new();
            string Priority = "NSWE";
            public List<string> ProposedList = new();
            List<Monkey> monkeys = new();
            int x_min=0;
            int x_max=0;
            int y_min=0;
            int y_max=0;
            public Map(string[] inputLines)
            {
                for (int y = 0; y < inputLines.Length; y++)
                    for(int x = 0; x < inputLines[y].Length; x++)
                        if (inputLines[y][x] == '#')    
                        {
                            string Location = $"{x},{y}";
                            this.Add(Location, new Monkey(this, x, y));
                            monkeys.Add(this[Location]);
                        }
            }
             
            public int Play(int rounds)
            {               

                for (int y = 0; y < rounds; y++)
                {
                    // propose
                    ProposedList.Clear();
                    int settledCount = 0;
                    foreach (Monkey M in monkeys)
                    {
                        string _p = M.Propose(Priority);
                        if (M.settled)
                            settledCount++;
                        else
                            ProposedList.Add(_p);
                    }
                    // check
                    if (settledCount == monkeys.Count)
                    {
                        Console.WriteLine(y+1);
                        return (y + 1);
                    }
                    // Move
                    foreach (Monkey M in monkeys.Where((Monkey m) => !m.settled))
                    {
                        if (ProposedList.Where((string x) => x == M.Proposed()).Count() == 1)
                            M.Move();
                    }
                    // change order
                    Priority += Priority[0];
                    Priority = Priority[1..];
                }
                return rounds; 
            }
            public void DebugPrint((int min,int max) X, (int min,int max) Y)
            {
                string outputBufer = "";
                for (int y = Y.min; y < Y.max; y++) 
                {
                    outputBufer += Environment.NewLine;
                    for (int x = X.min; x < X.max; x++)
                    {
                        if (this.ContainsKey($"{x},{y}"))
                            outputBufer += "#"; 
                        else
                            outputBufer += ".";
                    }                   
                }
                Console.WriteLine(outputBufer);
            }
            public int part1result()
            {
                // reset
                x_min = monkeys[0].x;
                x_max = monkeys[0].x;
                y_min = monkeys[0].y;
                y_max = monkeys[0].y;

                foreach (Monkey M in monkeys)
                {
                    if (M.x < x_min)
                        x_min = M.x;
                    if (M.y < y_min)
                        y_min = M.y;
                    if (M.x > x_max)
                        x_max = M.x;
                    if (M.y > y_max)
                        y_max = M.y;
                }
                return (x_max-x_min +1)*(y_max-y_min +1) - monkeys.Count();
            }
        }
        class Monkey
        {
            public Map Map;
            public int x;
            public int y;
            public (int x, int y) Proposal;
            public bool settled=false;
            public Monkey(Map map, int x, int y)
            {
                Map = map;
                this.x = x;
                this.y = y;
                Proposal = (x, y);
            }
            public (bool,int,int) CheckSurrounding(char Direction)
            {
                switch (Direction)
                {
                    case 'N':
                        return (!(Map.ContainsKey($"{x - 1},{y - 1}") || Map.ContainsKey($"{x},{y - 1}") || Map.ContainsKey($"{x + 1},{y - 1}")), 0, -1);
                    case 'S':
                        return (!(Map.ContainsKey($"{x - 1},{y + 1}") || Map.ContainsKey($"{x},{y + 1}") || Map.ContainsKey($"{x + 1},{y + 1}")), 0, 1);
                    case 'W':
                        return (!(Map.ContainsKey($"{x - 1},{y - 1}") || Map.ContainsKey($"{x - 1},{y}") || Map.ContainsKey($"{x - 1},{y + 1}")), -1, 0);
                    case 'E':
                        return (!(Map.ContainsKey($"{x + 1},{y - 1}") || Map.ContainsKey($"{x + 1},{y}") || Map.ContainsKey($"{x + 1},{y + 1}")), 1, 0);
                }
                return (false,0,0);
            }
            public string Propose(string Priority)
            {
                (bool r, int x, int y) _1stPri = CheckSurrounding(Priority[0]);
                (bool r, int x, int y) _2ndPri = CheckSurrounding(Priority[1]);
                (bool r, int x, int y) _3rdPri = CheckSurrounding(Priority[2]);
                (bool r, int x, int y) _4thPri = CheckSurrounding(Priority[3]);

                settled = _1stPri.r && _2ndPri.r && _3rdPri.r && _4thPri.r;
                if (settled)
                    Proposal = (x, y);
                else if (_1stPri.r)
                    Proposal = (x + _1stPri.x, y + _1stPri.y);
                else if (_2ndPri.r)
                    Proposal = (x + _2ndPri.x, y + _2ndPri.y);
                else if (_3rdPri.r)
                    Proposal = (x + _3rdPri.x, y + _3rdPri.y);
                else if (_4thPri.r)
                    Proposal = (x + _4thPri.x, y + _4thPri.y);
                else
                    Proposal = (x, y);

                //Proposed = ;
                //i && _2ndPri && _3rdPri && _4thPri)

                return Proposed();
            }
            public string Proposed()
            { return $"{Proposal.x},{Proposal.y}"; }
            public void Move()
            {
                Map.Remove($"{x},{y}");
                x = Proposal.x;
                y = Proposal.y;
                Map.Add($"{x},{y}", this);
            }
        }
    }
}