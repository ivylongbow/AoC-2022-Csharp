namespace AoC2022
{
    
    public class cDay_13 : WeekN
    {
        readonly string[] inputLines;
        readonly int x = 13; // x = [1..25]
        readonly Dictionary<int, Packet> packets = new(); 
        public cDay_13()
        {
            Title = $"--- Day {x}: Distress Signal ---";
            inputLines = ReadInput($"input_Day{x}.txt");
            //inputLines = ReadInput("");
            for (int i = 0; i < inputLines.Length; i+=3)
            {
                packets.Add(i / 3, new Packet(inputLines[i..(i+2)]));
            }
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
            int sum = 0;
            foreach(int key in packets.Keys)
            {
                if (packets[key].L.CompareTo(packets[key].R) <0)
                { sum += key+1; }
            }
            return $"{x}.1 - {sum}";
        }
        public override string Part2()
        {
            List<Data> allPackets = new();
            foreach(Packet packet in packets.Values)
            {
                allPackets.Add(packet.L);
                allPackets.Add(packet.R);
            }
            Data Divider2 = new Data("[[2]]");
            Data Divider6 = new Data("[[6]]");

            allPackets.Add(Divider2);
            allPackets.Add(Divider6);
            allPackets.Sort();

            int Idx2 = allPackets.IndexOf(Divider2) + 1;
            int Idx6 = allPackets.IndexOf(Divider6) + 1;
            return $"{x}.2 - {Idx2 * Idx6}";
        }
        class Packet
        {
            public Data L;
            public Data R;

            public Packet(string[] input)
            {
                L = new Data(input[0]);
                R = new Data(input[1]);
            }
        }
        class Data:IComparable<Data>
        {
            public List<Data> Value = new();
            //public Data super;
            public int intValue = -1;
            readonly string remainingString;
            public Data(string input)
            {
                if (input.StartsWith("["))
                {
                    for (remainingString = input; !remainingString.StartsWith("]"); remainingString = Value.LastOrDefault().remainingString)
                    {
                        remainingString = remainingString.Substring(1);
                        Value.Add(new Data(remainingString));
                    }
                    remainingString = remainingString.Substring(1);
                }
                else
                {
                    int length = input.IndexOfAny(",]".ToCharArray());
                    if (length > 0)
                        intValue = Convert.ToInt32(input[..length]);
                    remainingString = input[length..];
                }
                //input = input[1..];
            }
            public override string ToString()
            {
                if(Value.Count == 0)
                    return intValue.ToString();
                else
                {
                    string ListValue = "[";
                    foreach(Data data in Value)
                    {
                        ListValue += data.ToString();
                        ListValue += ",";
                    }
                    ListValue += "]";
                    return ListValue.Replace(",]","]").Replace("-1","");
                }                
            }
            public int CompareTo(Data? other)
            {
                if (this.Value.Count == 0 && other.Value.Count == 0)
                {
                    return this.intValue.CompareTo(other.intValue);
                }
                else if (this.Value.Count > 0 && other.Value.Count > 0)
                {
                    for (int i = 0; i < this.Value.Count; i++)
                        if (i == other.Value.Count)
                            return this.Value.Count - other.Value.Count;
                        else if (this.Value[i].CompareTo(other.Value[i]) != 0)
                            return this.Value[i].CompareTo(other.Value[i]);
                    return this.Value.Count - other.Value.Count;
                }
                else if (this.Value.Count > 0)
                {
                    return this.CompareTo(new Data($"[{other.intValue}]"));
                }
                else if (this.intValue == -1)
                    return -1;
                else
                    return -other.CompareTo(new Data($"[{this.intValue}]"));
            }
        }
    }
}