using System.Text.RegularExpressions;
namespace AoC2022
{    
    public class cDay_19 : WeekN
    {
        readonly string[] inputLines;
        readonly int x = 19; // x = [1..25]
        public cDay_19()
        {
            Title = $"--- Day {x}: xxxxxxxx xxxxxxxx ---";
            inputLines = ReadInput($"input_Day{x}.txt");
            //inputLines = ReadInput("");
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
            int part1 = 0;
            for (int i = 0; i < inputLines.Length; i++)
            {
                Blueprint B = new(inputLines[i], 24);
                B.Play();
                part1 += B.BestScore() * (i+1);
                //part1 = B.GetBestResult();    // Obsolited, works much slower than class, don't know why...
                //Console.WriteLine(part1);
            }
            return $"{x}.1 - {part1}";
        }
        public override string Part2()
        {
            int part2 = 1;
            for (int i = 0; i < 3; i++)
            {
                Blueprint B = new(inputLines[i], 32);
                B.Play();
                part2 *= B.BestScore();
                //part2 = B.GetBestResult();    // Obsolited, works much slower than class, don't know why...
                //Console.WriteLine(part2);
            }
            return $"{x}.2 - {part2}";
        }
        struct resource
        {
            public int ore;
            public int clay;
            public int obsidian;
            public int geode;
            public resource(int ore, int clay, int obsidian, int geode)
            {
                this.ore = ore;
                this.clay = clay;
                this.obsidian = obsidian;
                this.geode = geode;
            }
            public override string ToString()
            {
                return $"{ore},{clay},{obsidian},{geode}";
            }
            public bool Affordable(resource cost)
            {
                return !(ore < cost.ore || clay < cost.clay || obsidian < cost.obsidian);
            }
            public static resource operator +(resource A, resource B)
            {
                resource result = new resource(A.ore+B.ore, A.clay+B.clay, A.obsidian+B.obsidian, A.geode+B.geode);
                return result;
            }
            public static resource operator -(resource A, resource B)
            {
                resource result = new resource(A.ore - B.ore, A.clay - B.clay, A.obsidian - B.obsidian, A.geode - B.geode);
                return result;
            }
            public static resource operator *(resource A, int f)
            {
                resource result = new resource(A.ore *f, A.clay *f, A.obsidian *f, A.geode *f);
                return result;
            }
            public static resource operator ^(resource A, int x)
            {
                if (x % 4 == 0)
                    return new resource(A.ore + 1, A.clay, A.obsidian, A.geode);
                else if (x % 4 == 1)
                    return new resource(A.ore, A.clay + 1, A.obsidian, A.geode);
                else if (x % 4 == 2)
                    return new resource(A.ore, A.clay, A.obsidian + 1, A.geode);
                else if (x % 4 == 3)
                    return new resource(A.ore, A.clay, A.obsidian, A.geode + 1);
                else
                    return A;
            }
            public static int operator /(resource A, resource B)
            {
                int result_ore;
                if (B.ore != 0)
                    result_ore = (int)Math.Ceiling((double)A.ore / B.ore);
                else if (A.ore != 0)
                    result_ore = 9999;
                else
                    result_ore = 0;
                //
                int result_clay;
                if (B.clay != 0)
                    result_clay = (int)Math.Ceiling((double)A.clay / B.clay);
                else if (A.clay != 0)
                    result_clay = 9999;
                else
                    result_clay = 0; 
                //
                int result_obsidian;
                if (B.obsidian != 0)
                    result_obsidian = (int)Math.Ceiling((double)A.obsidian / B.obsidian);
                else if (A.obsidian != 0)
                    result_obsidian = 9999;
                else
                    result_obsidian = 0;

                return Math.Max(Math.Max(result_ore, result_clay), result_obsidian);
            }
        }
        class Blueprint
        {
            string ID;
            int time;
            static Dictionary<string, Blueprint> History = new();
            static Dictionary<string, resource> RobotPrice =new();
            static int MaxNeedForOre;
            static int MaxNeedForClay;
            static int MaxNeedForObsidian;
            static int TimeLimit = 0;
            public resource Balance;
            public resource Robots;
            Dictionary<string,Blueprint> MultiUniverse =new();
            public Blueprint(string line, int PlayTime)
            {
                //Blueprint 1: Each ore robot costs 3 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 16 clay. Each geode robot costs 3 ore and 9 obsidian.
                Match m = Regex.Match(line, @"Blueprint (\d+): Each ore robot costs (\d+) ore. Each clay robot costs (\d+) ore. Each obsidian robot costs (\d+) ore and (\d+) clay. Each geode robot costs (\d+) ore and (\d+) obsidian.");
                //ID = int.Parse(m.Groups[1].Value);
                int ore_ore = int.Parse(m.Groups[2].Value);
                int clay_ore = int.Parse(m.Groups[3].Value);
                int obsidian_ore = int.Parse(m.Groups[4].Value);
                int obsidian_clay = int.Parse(m.Groups[5].Value);
                int geode_ore = int.Parse(m.Groups[6].Value);
                int geode_obsidian = int.Parse(m.Groups[7].Value);

                time = 0;
                Balance = new(0, 0, 0, 0);
                Robots = new(1, 0, 0, 0);
                ID = ToString();

                History.Clear();
                RobotPrice = new Dictionary<string, resource>();
                RobotPrice.Add("ore", new resource(ore_ore, 0, 0, 0));
                RobotPrice.Add("clay", new resource(clay_ore, 0, 0, 0));
                RobotPrice.Add("obsidian", new resource(obsidian_ore, obsidian_clay, 0, 0));
                RobotPrice.Add("geode", new resource(geode_ore, 0, geode_obsidian, 0));
                MaxNeedForOre=Math.Max(ore_ore, clay_ore);
                MaxNeedForOre = Math.Max(MaxNeedForOre, obsidian_ore);
                MaxNeedForOre = Math.Max(MaxNeedForOre, geode_ore);
                MaxNeedForClay = obsidian_clay;
                MaxNeedForObsidian = geode_obsidian;

                TimeLimit = PlayTime;
            }
            public Blueprint(Blueprint lastMinute, resource robotBuilt, resource resourceChange, int time)
            {
                this.time = time;
                Balance = lastMinute.Balance - resourceChange - robotBuilt;
                Robots  = lastMinute.Robots + robotBuilt;
                // key point is to limit the unlimited possibilities
                // here if Balance is enough to cover building of a robot each day, then extra balance should be ignored!!!
                // insight from https://github.com/jonathanpaulson/AdventOfCode/blob/master/2022/19.py
                if (Balance.ore > MaxNeedForOre * (TimeLimit - time - 1) - Robots.ore * (TimeLimit - time - 1))
                    Balance.ore = MaxNeedForOre * (TimeLimit - time - 1) - Robots.ore * (TimeLimit - time - 1);
                if (Balance.clay > MaxNeedForClay * (TimeLimit - time - 1) - Robots.clay * (TimeLimit - time - 1))
                    Balance.clay = MaxNeedForClay * (TimeLimit - time - 1) - Robots.clay * (TimeLimit - time - 1);
                if (Balance.obsidian > MaxNeedForObsidian * (TimeLimit - time - 1) - Robots.obsidian * (TimeLimit - time - 1))
                    Balance.obsidian = MaxNeedForObsidian * (TimeLimit - time - 1) - Robots.obsidian * (TimeLimit - time - 1);


                ID = ToString();
            }
            public override string ToString()
            {
                return $"{time};{Balance};{Robots}";
            }
            public string ToString(int time)
            {
                return $"{time};{Balance};{Robots}";
            }
            public void Play()
            {
                List<Blueprint> PossibleFutures = new List<Blueprint>();
                for (int t = time; t < TimeLimit; t++)// && !MultiUniverse.Contains(ToString()))
                {
                    bool Affordable_geode = Balance.Affordable(RobotPrice["geode"]);
                    bool Affordable_obsidian = Balance.Affordable(RobotPrice["obsidian"]);
                    bool Affordable_clay = Balance.Affordable(RobotPrice["clay"]);
                    bool Affordable_ore = Balance.Affordable(RobotPrice["ore"]);
                    string NewID;

                    if (Affordable_clay && Affordable_ore && Affordable_obsidian && Affordable_geode)
                        break;

                    Balance += Robots;
                    NewID = ToString(t + 1);
                    if (!History.ContainsKey(NewID))
                        History.Add(NewID, this);
                    else
                        break;

                    if (!Affordable_geode && Balance.Affordable(RobotPrice["geode"]))
                    {
                        Blueprint Universe = new Blueprint(this, new resource(0, 0, 0, 1), RobotPrice["geode"], t + 1);
                        NewID = Universe.ToString();
                        MultiUniverse.Add(NewID, Universe);
                        Universe.Play();
                    }
                    //
                    if (!Affordable_obsidian && Balance.Affordable(RobotPrice["obsidian"]) && Robots.ore < MaxNeedForObsidian)
                    {
                        Blueprint Universe = new Blueprint(this, new resource(0, 0, 1, 0), RobotPrice["obsidian"], t + 1);
                        NewID = Universe.ToString();
                        MultiUniverse.Add(NewID, Universe);
                        Universe.Play();
                    }
                    //
                    if (!Affordable_clay && Balance.Affordable(RobotPrice["clay"]) && Robots.ore < MaxNeedForClay)
                    {
                        Blueprint Universe = new Blueprint(this, new resource(0, 1, 0, 0), RobotPrice["clay"], t + 1);
                        NewID = Universe.ToString();
                        MultiUniverse.Add(NewID, Universe);
                        //if (!History.ContainsKey(NewID))
                        //    History.Add(NewID, this);
                        Universe.Play();
                    }
                    //
                    if (!Affordable_ore && Balance.Affordable(RobotPrice["ore"]) && Robots.ore < MaxNeedForOre)
                    {
                        Blueprint Universe = new Blueprint(this, new resource(1, 0, 0, 0), RobotPrice["ore"], t + 1);
                        NewID = Universe.ToString();
                        MultiUniverse.Add(NewID, Universe);
                        //if (!History.ContainsKey(NewID))
                        //    History.Add(NewID, this);
                        Universe.Play();
                    }

                }
            }
            public int BestScore()
            {
                int this_best = Balance.geode;
                foreach (Blueprint Universe in MultiUniverse.Values)
                {
                    int another_best = Universe.BestScore();
                    if (another_best > this_best)
                        this_best = another_best;
                }
                return this_best;
            }

            /// <summary>
            /// Obsolited code, works much slower than class, don't know why...
            /// </summary>
            /// <returns></returns>
            public int GetBestResult()
            {
                Queue<(int time, resource Balance, resource Robots )> Q = new();
                
                Q.Enqueue((0, this.Balance, this.Robots));
                int bestresult = 0;

                while(Q.Count > 0)
                {
                    int t;
                    resource Balance;
                    resource Robots;
                    (t,Balance,Robots) = Q.Dequeue();

                    // if(seen) continue
                    string NewID = $"{t},{Balance},{Robots}";
                    if (!History.ContainsKey(NewID))
                        History.Add(NewID, this);
                    else
                        continue;

                    int t_geo = (RobotPrice["geode"] - Balance) / Robots + 1;
                    if (t + t_geo < TimeLimit)
                        Q.Enqueue((t + t_geo, Balance + Robots * t_geo - RobotPrice["geode"], Robots ^ 3));

                    int t_obs = (RobotPrice["obsidian"] - Balance) / Robots + 1;
                    if (t + t_obs < TimeLimit - 2)
                        Q.Enqueue((t + t_obs, Balance + Robots * t_obs - RobotPrice["obsidian"], Robots ^ 2));
                    
                    int t_cly = (RobotPrice["clay"] - Balance) / Robots + 1;
                    if (t + t_cly < TimeLimit - 4)
                        Q.Enqueue((t + t_cly, Balance + Robots * t_cly - RobotPrice["clay"], Robots ^ 1));

                    int t_ore = (RobotPrice["ore"] - Balance) / Robots + 1;
                    if (t + t_ore < TimeLimit - 2)
                        Q.Enqueue((t + t_ore, Balance + Robots * t_ore - RobotPrice["ore"], Robots ^ 0));

                    Balance += Robots * (TimeLimit - t);
                    if (Balance.geode > bestresult)
                        bestresult = Balance.geode;
                }
                return bestresult;
            }
        }
    }
}