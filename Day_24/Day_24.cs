namespace AoC2022
{    
    public class cDay_24 : WeekN
    {
        readonly string[] inputLines;
        readonly int x = 24; // x = [1..25]
        Map TheMap;
        int bestMove;
        public cDay_24()
        {
            Title = $"--- Day {x}: Blizzard Basin ---";
            inputLines = ReadInput($"input_Day{x}.txt");
           // inputLines = ReadInput("");
            TheMap=new(inputLines);
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
            
            //Console.WriteLine(TheMap.WeatherReport(0));
            bestMove = TheMap.BeginJourney ( Map.Entrance, Map.Exit, 0);

            return $"{x}.1 - {bestMove}";
        }
        public override string Part2()
        {       
            int bestMove2 = TheMap.BeginJourney(Map.Exit, Map.Entrance, bestMove + 1);
            int bestMove3 = TheMap.BeginJourney(Map.Entrance, Map.Exit, bestMove2 + 1);

            return $"{x}.2 - {bestMove3}";
        }
    }
    class Map
    {
        public static int X_map;
        public static int Y_map;
        public static Coord Entrance;
        public static Coord Exit;
        public int t = 0;
        int t_max;
        Dictionary<int,List<Coord>> PossibleLocationsAt= new();
        List<Blizzard> blizzards = new List<Blizzard>();
        Dictionary<int,List<string>> AtMinute = new Dictionary<int,List<string>>();
        public Map(string[] inputLines)
        {
            Y_map= inputLines.Length-2;
            X_map= inputLines[0].Length-2;
            Entrance = new Coord() with { x = 0, y = 0 };
            Exit = new Coord() with { x = X_map - 1, y = Y_map - 1 };
            //BestMove = new int[X_map,Y_map];
            for (int y = 0; y < Y_map; y++)
                for (int x = 0; x < X_map; x++)
                {
                    char c = inputLines[y + 1][x + 1];
                    if (c != '.')
                        blizzards.Add(new Blizzard(x, y, c));
                }
            //get GCD:
            int GCD = X_map > Y_map ? X_map : Y_map;
            int smaller = X_map + Y_map - GCD;
            while (smaller != 0)
            {
                int tmp = GCD % smaller;
                GCD = smaller;
                smaller = tmp;
            }
            //get LCM:
            t_max = X_map * Y_map / GCD;
            WeatherForecast();
        }
        public string WeatherReport(int time_in_minute)
        {
            string outputBuffer = "";

            for (int y = 0; y < Y_map; y++)
            {
                outputBuffer += "#";
                for (int x = 0; x < X_map; x++)
                {
                    int cnt = AtMinute[time_in_minute % t_max].Where((string loc) => loc == $"{x},{y}").Count();
                    if (cnt > 1)
                        outputBuffer += $"{cnt}";
                    else if (cnt == 1)
                    {
                        outputBuffer += "@";
                    }
                    else
                        outputBuffer += ".";
                }
                outputBuffer += "#" + Environment.NewLine;
            }

            return outputBuffer;
        }
        class Blizzard
        {
            public Coord loc;
            public Coord dir;
            public Blizzard(int x, int y, char c)
            {
                loc = new Coord() with { x=x , y=y };
                dir = new Coord() with { x=0 , y=0 };
                switch(c)
                {
                    case '<':
                        dir.x--;
                        break;
                    case '>':
                        dir.x++;
                        break;
                    case '^':
                        dir.y--;
                        break;                    
                    case 'v':
                        dir.y++;
                        break;
                }
            }
            public void Move()
            {
                loc += dir;
                if (loc.x < 0)
                    loc.x = X_map - 1;
                else if (loc.x == X_map)
                    loc.x = 0;
                else if (loc.y < 0)
                    loc.y = Y_map - 1;
                else if (loc.y == Y_map)
                    loc.y = 0;
            }
        }
        public void WeatherForecast()
        {
            List<Coord> list = new List<Coord>();
            list.Add(Entrance);

            for (int minute = 0; minute < t_max; minute++)
            {
                List<string> BlizzardCoordinates = new List<string>();
                AtMinute.Add(minute, BlizzardCoordinates);

                foreach (Blizzard B in blizzards)
                {
                    BlizzardCoordinates.Add(B.loc.ToString());
                    B.Move();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startCoord"></param>
        /// <param name="endCorrd"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public int BeginJourney(Coord startCoord, Coord endCorrd, int t)
        {
            for (int retry = 0; retry < t_max; retry++)
            {
                while (AtMinute[t % t_max].Contains(startCoord.ToString()))
                    t++;
                PossibleLocationsAt.Add(t, new List<Coord>());
                PossibleLocationsAt[t].Add(startCoord);


                while (PossibleLocationsAt[t].Count > 0)
                {
                    t++;
                    PossibleLocationsAt.Add(t, new List<Coord>());
                    foreach (Coord C in PossibleLocationsAt[t - 1].Distinct())
                    {
                        if (!AtMinute[t % t_max].Contains(C.ToString())) PossibleLocationsAt[t].Add(C);
                        Coord N = new Coord() with { x = C.x, y = C.y - 1 };
                        if (!AtMinute[t % t_max].Contains(N.ToString()) && N.y >= 0) PossibleLocationsAt[t].Add(N);
                        Coord S = new Coord() with { x = C.x, y = C.y + 1 };
                        if (!AtMinute[t % t_max].Contains(S.ToString()) && S.y < Y_map) PossibleLocationsAt[t].Add(S);
                        Coord E = new Coord() with { x = C.x - 1, y = C.y };
                        if (!AtMinute[t % t_max].Contains(E.ToString()) && E.x >= 0) PossibleLocationsAt[t].Add(E);
                        Coord W = new Coord() with { x = C.x + 1, y = C.y };
                        if (!AtMinute[t % t_max].Contains(W.ToString()) && W.x < X_map) PossibleLocationsAt[t].Add(W);
                    }
                    if (!AtMinute[t % t_max].Contains(startCoord.ToString()))
                        PossibleLocationsAt[t].Add(startCoord);

                    //Console.WriteLine(WeatherReport(t));
                    //foreach (Coord C in PossibleLocationsAt[t].Distinct())
                    //    Console.WriteLine(C.ToString());

                    if (PossibleLocationsAt[t].Contains(endCorrd))
                        return t + 1;
                }
            }
            return -1;
        }
    }
    struct Coord
    {
        public int x;
        public int y;
        public static Coord operator +(Coord A, Coord B)
            { return new Coord { x = A.x+B.x, y = A.y+B.y }; }
        public override string ToString()
        {
            return $"{x},{y}";
        }
    }
}