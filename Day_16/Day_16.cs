﻿using System.Text.RegularExpressions;
namespace AoC2022
{    
    public class cDay_16 : WeekN
    {
        readonly string[] inputLines;
        readonly int x = 16; // x = [1..25]
        readonly Rooms TheCave;
        public cDay_16()
        {
            Title = $"--- Day {x}: Proboscidea Volcanium ---";
            inputLines = ReadInput($"input_Day{x}.txt");
            //inputLines = ReadInput("");
            TheCave= new Rooms(inputLines);
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
            TheCave.SingleMode();
            return $"{x}.1 - {TheCave.BestResult}";
        }
        public override string Part2()
        {
            TheCave.DualMode();
            return $"{x}.2 - {TheCave.BestResult}";
        }
        class Valve
        {
            public readonly string Name;
            public readonly int FlowRate;
            public readonly List<string> Tunnels;
            public readonly Dictionary<string, int> Distance;
            public readonly Rooms Cave;
            public Valve(string name, int flowRate, List<string> tunnels, Rooms cave)
            {
                Name = name;
                FlowRate = flowRate;
                Tunnels = tunnels;
                Cave = cave;
                Distance = new();
                Distance.Add(name, 0);
            }
            public bool Explore()
            {
                bool done = true;
                //Distance.Clear();
                foreach(string tunnel in Tunnels)
                {
                    if (!Distance.ContainsKey(tunnel))
                    {
                        
                        Distance.Add(tunnel, 1);
                        done = false;
                    }
                    foreach(string cave in Cave[tunnel].Distance.Keys)
                    {
                        if (Distance.ContainsKey(cave))
                        {
                            if (Distance[cave] > Cave[tunnel].Distance[cave] + 1)
                            {
                                Distance[cave] = Cave[tunnel].Distance[cave] + 1;
                                done = false;
                            }                                    
                        }
                        else
                        {
                            Distance.Add(cave, Cave[tunnel].Distance[cave] + 1);
                            done = false;
                        }
                    }                    
                }               
                return done;
            }

            public int TryPart1(Stack<string> path,int MinutesLeft)
            {
                int MaxPressure = 0;
                foreach (string room in Distance.Keys)
                {
                    if(!path.Contains(room) && MinutesLeft - Distance[room] > 1)
                    {
                        path.Push(room);
                        int newPressure = (MinutesLeft - Distance[room] - 1) * Cave[room].FlowRate;
                        newPressure += Cave[room].TryPart1(path, MinutesLeft - Distance[room] - 1);
                        if (MaxPressure < newPressure)
                            MaxPressure = newPressure;
                        path.Pop();
                    }
                }
                return MaxPressure;// + MinutesLeft * FlowRate;
            }

        }
        class Rooms : Dictionary<string, Valve>
        {
            Dictionary<string, Valve> WithValve = new Dictionary<string, Valve>();
            List<string> WithoutValve = new List<string>();
            Stack<string> Path1 = new Stack<string>();
            Stack<string> Path2 = new Stack<string>();
            public int BestResult;
            public Rooms(string[] inputLines)
            {
                foreach(string line in inputLines)
                {
                    //Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
                    Match m = Regex.Match(line, @"Valve ([A-Z]+) has flow rate=(\d+); \w+ \w+ to \w+ ([A-Z,\s]+)");
                    string _name = m.Groups[1].Value;
                    int _flowRate = int.Parse(m.Groups[2].Value);
                    string[] _tunnels = m.Groups[3].Value.Split(',',StringSplitOptions.TrimEntries);
                    this.Add(_name, new Valve( _name, _flowRate, _tunnels.ToList(), this));
                }
                bool done = false;
                while (!done)
                {
                    done = true;
                    foreach (string room in this.Keys)
                        done &= this[room].Explore();
                }
                foreach(string room in this.Keys)
                {
                    if (this[room].FlowRate > 0)
                        WithValve.Add(room, this[room]);
                    else
                        WithoutValve.Add(room);
                }
                foreach (string roomWithoutValve in WithoutValve)                
                {
                    this["AA"].Distance.Remove(roomWithoutValve);
                    foreach (string roomWithValve in WithValve.Keys)
                        WithValve[roomWithValve].Distance.Remove(roomWithoutValve);
                }                
            }

            public void SingleMode()
            {
                BestResult = this["AA"].TryPart1(Path1, 30);
            }
            public void DualMode()
            {

                Path1.Clear();
                Path2.Clear();
                Path1.Push("AA");
                Path2.Push("AA");
                BestResult = TryPart2(26,26);

            }
            public void PlotCave()
            {
                foreach (string room in WithValve.Keys)
                {
                    foreach (string connection in this[room].Distance.Keys)
                        Console.WriteLine($"{room}, {connection}, {this[room].Distance[connection]}");
                }
            }
            public void Prooning()
            {
                foreach(string room1 in WithValve.Keys)
                    foreach(string room2 in WithValve.Keys)
                        if(room1 != room2 && this[room1].Distance.ContainsKey(room2))
                            foreach(string midRoom in WithValve.Keys)
                                if (room1 != midRoom && room2 != midRoom && this[room1].Distance.ContainsKey(midRoom) && this[room2].Distance.ContainsKey(midRoom))
                                    if (this[room1].Distance[room2] == this[room1].Distance[midRoom] + this[room2].Distance[midRoom])
                                    {
                                        this[room1].Distance.Remove(room2);
                                        this[room2].Distance.Remove(room1);
                                        Console.WriteLine($"{room1}-{room2}");
                                        break;
                                    }
            }
            public int TryPart2(int MinutesLeft1, int MinutesLeft2)
            {
                int MaxPressure = 0;
                string currentRoom1 = Path1.Peek();
                string currentRoom2 = Path2.Peek();

                foreach (string room in WithValve.Keys.Except(Path1).Except(Path2))
                {
                    int _minutesLeft1 = MinutesLeft1 - this[currentRoom1].Distance[room] -1;
                    int _minutesLeft2 = MinutesLeft2 - this[currentRoom2].Distance[room] -1;
                    if (_minutesLeft1 > _minutesLeft2)
                    {
                        if (_minutesLeft1 > 0)
                        {
                            Path1.Push(room);
                            int newPressure = _minutesLeft1 * this[room].FlowRate;
                            newPressure +=    TryPart2(_minutesLeft1, MinutesLeft2);
                            if (MaxPressure < newPressure)
                                MaxPressure = newPressure;
                            Path1.Pop();
                        }
                    }
                    else
                    {
                        if (_minutesLeft2 > 0)
                        {
                            Path2.Push(room);
                            int newPressure = (_minutesLeft2) * this[room].FlowRate;
                            newPressure += TryPart2(MinutesLeft1, _minutesLeft2);
                            if (MaxPressure < newPressure)
                                MaxPressure = newPressure;
                            Path2.Pop();
                        }
                    }

                }
                if (MinutesLeft1 > 20 || MinutesLeft1 > 20)
                {
                    Console.WriteLine("");
                    Console.WriteLine("1");
                    foreach (string s in Path1)
                        Console.Write($">>{s}");
                    Console.WriteLine("");
                    Console.WriteLine("2");
                    foreach (string s in Path2)
                        Console.Write($">>{s}");
                    Console.WriteLine("");
                }


                return MaxPressure;// + MinutesLeft * FlowRate;
            }

        }
    }
}