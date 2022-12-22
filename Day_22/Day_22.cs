namespace AoC2022
{    
    public class cDay_22 : WeekN
    {
        readonly string[] inputLines;
        readonly int x = 22; // x = [1..25]
        Map TheMap;
        public cDay_22()
        {
            Title = $"--- Day {x}: Monkey Map ---";
            inputLines = ReadInput($"input_Day{x}.txt");
            //inputLines = ReadInput("");
            TheMap = new Map(inputLines);
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
            TheMap.GenerateMap1();
            TheMap.Journey(false);
            int result = TheMap.Curr.y * 1000 + TheMap.Curr.x * 4 + TheMap.DIR;
            return $"{x}.1 - {result}";
        }
        public override string Part2()
        {
            TheMap.GenerateMap2();
            TheMap.Journey(false);
            int result = TheMap.Curr.y * 1000 + TheMap.Curr.x * 4 + TheMap.DIR;
            return $"{x}.2 - {result}";
        }
    }
    class Map
    {
        Dictionary<string, Cell> rawmap;
        Dictionary<string,(string,int)> cheatingmap;
        List<string> instructions;
        Cell? Home;
        public Cell? Curr;
        public int DIR = 0; 
        public class Cell
        {
            public int x;
            public int y;
            public char c;
            public Dictionary<string,int> dir_change = new();
            public Dictionary<string,Cell> next;
            public Cell(int x, int y, char c)
            {
                this.x = x;
                this.y = y;
                this.c = c;
                next = new();
                next.Add("L", this);
                next.Add("R", this);
                next.Add("U", this);
                next.Add("D", this);
            }
        }
        public Map(string[] inputLines)
        {
            rawmap = new Dictionary<string, Cell>();
            for(int y = 0; y < inputLines.Length -1; y++)
                for(int x = 0; x < inputLines[y].Length; x++)
                {
                    char c = inputLines[y][x];
                    if (c != ' ')
                    {
                        Cell C = new Cell(x+1, y+1, c);
                        rawmap.Add($"{x+1},{y+1}", C);
                    }                        
                }

            foreach(Cell C in rawmap.Values)
            {
                Home = C;
                Curr = C;
                break;
            }           

            string lastLine = inputLines.Last();
            lastLine = lastLine.Replace("R", " R ").Replace("L", " L ");
            instructions= lastLine.Split(' ').ToList();
        }
        public void GenerateMap1()
        {
            foreach (Cell C in rawmap.Values)
            {
                int x = C.x;
                int y = C.y;

                string L = $"{x - 1},{y}";
                if (!rawmap.ContainsKey(L))
                {
                    L = $"{x},{y}"; int i = 0;
                    while (rawmap.ContainsKey(L))
                        L = $"{x + ++i},{y}";
                    L = $"{x + --i},{y}";
                }
                if (rawmap[L].c == '.')
                    C.next["L"] = rawmap[L];

                string R = $"{x + 1},{y}";
                if (!rawmap.ContainsKey(R))
                {
                    R = $"{x},{y}"; int i = 0;
                    while (rawmap.ContainsKey(R))
                        R = $"{x + --i},{y}";
                    R = $"{x + ++i},{y}";
                }
                if (rawmap[R].c == '.')
                    C.next["R"] = rawmap[R];

                string U = $"{x},{y - 1}";
                if (!rawmap.ContainsKey(U))
                {
                    U = $"{x},{y}"; int i = 0;
                    while (rawmap.ContainsKey(U))
                        U = $"{x},{y + ++i}";
                    U = $"{x},{y + --i}";
                }
                if (rawmap[U].c == '.')
                    C.next["U"] = rawmap[U];

                string D = $"{x},{y + 1}";
                if (!rawmap.ContainsKey(D))
                {
                    D = $"{x},{y}"; int i = 0;
                    while (rawmap.ContainsKey(D))
                        D = $"{x},{y + --i}";
                    D = $"{x},{y + ++i}";
                }
                if (rawmap[D].c == '.')
                    C.next["D"] = rawmap[D];
            }
        }
        public void GenerateMap2()
        {
            GenerateCheatMap();
            foreach (Cell C in rawmap.Values)
            {
                int x = C.x;
                int y = C.y;
                int dir_change = 0;

                string L = $"{x - 1},{y}";
                if (!rawmap.ContainsKey(L))
                {
                    (L,dir_change) = Find("L",C);
                    C.next["L"] = C;
                }
                if (rawmap[L].c == '.')
                {
                    C.next["L"] = rawmap[L];
                    C.dir_change.Add("L", dir_change);
                }
                    

                string R = $"{x + 1},{y}";
                dir_change = 0;
                if (!rawmap.ContainsKey(R))
                {
                    (R, dir_change) = Find("R", C);
                    C.next["R"] = C;
                }
                if (rawmap[R].c == '.')
                {
                    C.next["R"] = rawmap[R];
                    C.dir_change.Add("R", dir_change);
                }

                string U = $"{x},{y - 1}";
                dir_change = 0;
                if (!rawmap.ContainsKey(U))
                {
                    (U, dir_change) = Find("U", C);
                    C.next["U"] = C;
                }
                if (rawmap[U].c == '.')
                {
                    C.next["U"] = rawmap[U];
                    C.dir_change.Add("U", dir_change);
                }

                string D = $"{x},{y + 1}";
                dir_change = 0;
                if (!rawmap.ContainsKey(D))
                {
                    (D, dir_change) = Find("D", C);
                    C.next["D"] = C;
                }
                if (rawmap[D].c == '.')
                {
                    C.next["D"] = rawmap[D];
                    C.dir_change.Add("D", dir_change);
                }
            }

            (string,int) Find(string dir, Cell C)
            {
                return cheatingmap[$"{C.x},{C.y},{dir}"];
            }
            void GenerateCheatMap()
            {
                cheatingmap = new();
                int x1,y1,x2,y2;
                for (int i = 0; i < 4; i++) 
                {   // Fr <=> T
                    x1 = 8 + i; y1 = 0; x2 = 3 - i; y2 = 4;
                    cheatingmap.Add($"{x1 + 1},{y1 + 1},U", ($"{x2 + 1},{y2 + 1}", 2));
                    cheatingmap.Add($"{x2 + 1},{y2 + 1},U", ($"{x1 + 1},{y1 + 1}", 2));
                    // Fr <=> L
                    x1 = 8; y1 = i; x2 = 4 + i; y2 = 4;
                    cheatingmap.Add($"{x1 + 1},{y1 + 1},L", ($"{x2 + 1},{y2 + 1}", 3));
                    cheatingmap.Add($"{x2 + 1},{y2 + 1},U", ($"{x1 + 1},{y1 + 1}", 1));
                    // Fr <=> R
                    x1 = 11; y1 = i; x2 = 15; y2 = 11 - i;
                    cheatingmap.Add($"{x1 + 1},{y1 + 1},R", ($"{x2 + 1},{y2 + 1}", 2));
                    cheatingmap.Add($"{x2 + 1},{y2 + 1},R", ($"{x1 + 1},{y1 + 1}", 2));
                    // Bk <=> T
                    x1 = 8 + i; y1 = 11; x2 = 3 - i; y2 = 7;
                    cheatingmap.Add($"{x1 + 1},{y1 + 1},D", ($"{x2 + 1},{y2 + 1}", 2));
                    cheatingmap.Add($"{x2 + 1},{y2 + 1},D", ($"{x1 + 1},{y1 + 1}", 2));
                    // Bk <=> L
                    x1 = 8 ; y1 = 8 + i; x2 = 7 - i; y2 = 7;
                    cheatingmap.Add($"{x1 + 1},{y1 + 1},L", ($"{x2 + 1},{y2 + 1}", 1));
                    cheatingmap.Add($"{x2 + 1},{y2 + 1},D", ($"{x1 + 1},{y1 + 1}", 3));
                    // T <=> R
                    x1 = 0; y1 = 4 + i; x2 = 15 - i; y2 = 11;
                    cheatingmap.Add($"{x1 + 1},{y1 + 1},L", ($"{x2 + 1},{y2 + 1}", 1));
                    cheatingmap.Add($"{x2 + 1},{y2 + 1},D", ($"{x1 + 1},{y1 + 1}", 3));
                    // B <=> R
                    x1 = 11; y1 = 4 + i; x2 = 15 - i; y2 = 8;
                    cheatingmap.Add($"{x1 + 1},{y1 + 1},R", ($"{x2 + 1},{y2 + 1}", 1));
                    cheatingmap.Add($"{x2 + 1},{y2 + 1},U", ($"{x1 + 1},{y1 + 1}", 3));
                }

                int w = 50;
                (int x, int y) Fr = (w, 0);
                (int x, int y) Bk = (w, w * 2);
                (int x, int y) L = (0, w * 2);
                (int x, int y) R = (w * 2, 0);
                (int x, int y) T = (0, w * 3);
                (int x, int y) B = (w, w);

                for (int i = 0; i < 50; i++)
                {
                    // Fr <=> T, checked
                    x1 = Fr.x + i; y1 = Fr.y; x2 = T.x; y2 = T.y+i;
                    cheatingmap.Add($"{x1 + 1},{y1 + 1},U", ($"{x2 + 1},{y2 + 1}", 1));
                    cheatingmap.Add($"{x2 + 1},{y2 + 1},L", ($"{x1 + 1},{y1 + 1}", 3));

                    // Fr <=> L, checked
                    x1 = Fr.x; y1 = Fr.y + i; x2 = L.x; y2 = L.y + w - 1 - i;
                    cheatingmap.Add($"{x1 + 1},{y1 + 1},L", ($"{x2 + 1},{y2 + 1}", 2));
                    cheatingmap.Add($"{x2 + 1},{y2 + 1},L", ($"{x1 + 1},{y1 + 1}", 2));

                    // Bk <=> R, checked
                    x1 = Bk.x + w - 1; y1 = Bk.y + i; x2 = R.x + w -1; y2 = R.y + w - 1 - i;
                    cheatingmap.Add($"{x1 + 1},{y1 + 1},R", ($"{x2 + 1},{y2 + 1}", 2));
                    cheatingmap.Add($"{x2 + 1},{y2 + 1},R", ($"{x1 + 1},{y1 + 1}", 2));

                    // Bk <=> T, checked
                    x1 = Bk.x + i; y1 = Bk.y + w - 1; x2 = T.x + w - 1; y2 = T.y + i;
                    cheatingmap.Add($"{x1 + 1},{y1 + 1},D", ($"{x2 + 1},{y2 + 1}", 1));
                    cheatingmap.Add($"{x2 + 1},{y2 + 1},R", ($"{x1 + 1},{y1 + 1}", 3));

                    // B <=> L, checked
                    x1 = B.x; y1 = B.y + i; x2 = L.x + i; y2 = L.y;
                    cheatingmap.Add($"{x1 + 1},{y1 + 1},L", ($"{x2 + 1},{y2 + 1}", 3));
                    cheatingmap.Add($"{x2 + 1},{y2 + 1},U", ($"{x1 + 1},{y1 + 1}", 1));

                    // B <=> R, checked
                    x1 = B.x + w - 1; y1 = B.y + i; x2 = R.x + i; y2 = R.y + w - 1;
                    cheatingmap.Add($"{x1 + 1},{y1 + 1},R", ($"{x2 + 1},{y2 + 1}", 3));
                    cheatingmap.Add($"{x2 + 1},{y2 + 1},D", ($"{x1 + 1},{y1 + 1}", 1));

                    // T <=> R, checked
                    x1 = T.x + i; y1 = T.y + w - 1; x2 = R.x + i; y2 = R.y;
                    cheatingmap.Add($"{x1 + 1},{y1 + 1},D", ($"{x2 + 1},{y2 + 1}", 4));
                    cheatingmap.Add($"{x2 + 1},{y2 + 1},U", ($"{x1 + 1},{y1 + 1}", 4));


                }
            }
        }
        public void Journey(bool debug)
        {
            Dictionary<int,string> Neighbor=new Dictionary<int,string>();
            Neighbor.Add(0, "R");
            Neighbor.Add(1, "D");
            Neighbor.Add(2, "L");
            Neighbor.Add(3, "U");
            Curr = Home; DIR = 0;
            string debugPrint = "";
            foreach (string command in instructions)
            {
                //for(int x = 0; x < debug; x++)
                //{ 
                //    string command = instructions[x];
                
                if (command == "R")
                {
                    DIR = ++DIR % 4;
                    debugPrint = command;
                    //if (x >debug-5)
                    //    Console.Write(command);
                }
                else if (command == "L")
                {
                    DIR = (DIR + 3) % 4;
                    debugPrint = command;
                    //if (x > debug - 5) 
                    //    Console.Write(command);
                }
                else
                {
                    //if (x > debug - 5)
                    //    Console.WriteLine(command);
                    debugPrint += command;
                    int steps = int.Parse(command);
                    for (int i = 0; i < steps; i++)
                    {
                        int dir_change = 0;
                        if (Curr.dir_change.ContainsKey(Neighbor[DIR]))
                            dir_change = Curr.dir_change[Neighbor[DIR]];
                        //if (dir_change != 0)
                        //    Console.WriteLine($"{debugPrint}{Environment.NewLine}{Curr.x},{Curr.y},{DIR}");
                        Curr = Curr.next[Neighbor[DIR]];
                        DIR = (DIR + dir_change) % 4;
                        //if (dir_change != 0)
                        //    Console.WriteLine($"{Curr.x},{Curr.y},{DIR}");
                    }
                    if (debug)
                        Console.WriteLine($"{debugPrint}:{Curr.x},{Curr.y},{DIR}");
                }
            }
            //Console.WriteLine($"{Curr.x},{Curr.y},{DIR}");
        }
    }
}