namespace AoC2022
{    
    public class cDay_11 : WeekN
    {
        readonly string[] inputLines;
        readonly int x = 11; // x = [1..25]
        public cDay_11()
        {
            Title = $"--- Day {x}: Monkey in the Middle ---";
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
            Dictionary<string, Monkey> monkeyDictionary = new Dictionary<string, Monkey>();
            for (int i = 0; i < inputLines.Length; i += 7)
            {
                monkeyDictionary.Add(inputLines[i][7..8], new Monkey(inputLines[i..(i + 6)]));
            }
            for (int i = 0; i < 20; i++)
            {
                foreach (Monkey monkey in monkeyDictionary.Values)
                {
                    foreach (Item item in monkey.ItemList)
                    {
                        string ReceiverMonkey = monkey.GetReceiverMonkey(item);
                        monkeyDictionary[ReceiverMonkey].ItemList.Add(item);
                        item.Owner = monkeyDictionary[ReceiverMonkey];
                    }
                    monkey.ItemList.Clear();
                }
            }
            List<int> monkeyScoreList = new();
            foreach(Monkey monkey in monkeyDictionary.Values)
            {
                monkeyScoreList.Add(monkey.NumOfInspections);
            }
            monkeyScoreList.Sort();
            monkeyScoreList.Reverse();
            return $"{x}.1 - {monkeyScoreList[0] * monkeyScoreList[1]}";
        }
        public override string Part2()
        {
            Dictionary<string, Monkey> monkeyDictionary = new Dictionary<string, Monkey>();
            for (int i = 0; i < inputLines.Length; i += 7)
            {
                monkeyDictionary.Add(inputLines[i][7..8], new Monkey(inputLines[i..(i + 6)]));
            }
            for (int i = 0; i < 20; i++)
            {
                foreach (Monkey monkey in monkeyDictionary.Values)
                {
                    foreach (Item item in monkey.ItemList)
                    {
                        string ReceiverMonkey = monkey.GetReceiverMonkeyNoRelief(item);
                        monkeyDictionary[ReceiverMonkey].ItemList.Add(item);
                        item.Owner = monkeyDictionary[ReceiverMonkey];
                    }
                    monkey.ItemList.Clear();
                }
            }
            for (int i = 20; i < 10000; i++)
            {
                foreach (Monkey monkey in monkeyDictionary.Values)
                {
                    foreach (Item item in monkey.ItemList)
                    {
                        string ReceiverMonkey = monkey.GetReceiverMonkeyNoRelief(item);
                        monkeyDictionary[ReceiverMonkey].ItemList.Add(item);
                        item.Owner = monkeyDictionary[ReceiverMonkey];
                    }
                    monkey.ItemList.Clear();
                }
            }
            List<long> monkeyScoreList = new();
            foreach (Monkey monkey in monkeyDictionary.Values)
            {
                monkeyScoreList.Add(monkey.NumOfInspections);
            }
            monkeyScoreList.Sort();
            monkeyScoreList.Reverse();
            return $"{x}.2 - {monkeyScoreList[0] * monkeyScoreList[1]}";
        }
    }
    public class Monkey
    {
        public string ID;
        public List<Item> ItemList = new();
        private Func<long, long> Operation;
        private readonly long OPERANT;
        private readonly long DIVISOR;
        private readonly string PositiveBranch;
        private readonly string NegativeBranch;
        public int NumOfInspections = 0;
        
        public Monkey(string[] InputLines)
        {
            ID = InputLines[0];
            string[] StartingItems = InputLines[1][18..].Split(',');
            string[] Operation = InputLines[2][23..].Split(' ');
            if (Operation[0] == "+")
                this.Operation = ADD;
            else// if (Operation[0] == "*")
                this.Operation = MUL;
            OPERANT = 1;
            if (Operation[1] == "old")
                this.Operation = SQR;
            else
                OPERANT= Convert.ToUInt32(Operation[1]);

            DIVISOR = Convert.ToUInt32( InputLines[3][21..]);

            PositiveBranch = InputLines[4][29..];
            NegativeBranch = InputLines[5][30..];

            foreach(string Item in StartingItems)
            {
                ItemList.Add(new Item(Item,this));
            }
        }
        public string GetReceiverMonkey(Item item)
        {
            long worryLevel = Operation(item.Level);
            worryLevel = (worryLevel-worryLevel%3) / 3;
            item.Level=worryLevel;
            NumOfInspections++;
            if (worryLevel % DIVISOR == 0)
                return PositiveBranch;
            else
                return NegativeBranch;
        }
        public string GetReceiverMonkeyNoRelief(Item item)
        {
            long worryLevel = Operation(item.Level);
            if (worryLevel > (2 * 3 * 5 * 7 * 11 * 13 * 17 * 19 * 23))
                worryLevel %= (2 * 3 * 5 * 7 * 11 * 13 * 17 * 19 * 23);
            item.Level = worryLevel;
            NumOfInspections++;
            if (worryLevel % DIVISOR == 0)
                return PositiveBranch;
            else
                return NegativeBranch;
        }
        private long ADD(long oldNum)
        {
            return oldNum + OPERANT;
        }
        private long MUL(long oldNum)
        {
            return oldNum * OPERANT;
        }
        private long SQR(long oldNum)
        {
            return oldNum * oldNum;
        }
        public override string ToString()
        {
            string[] debugDisplay = new string[ItemList.Count];
            for(int i = 0; i< ItemList.Count; i++)
            {
                debugDisplay[i] = (ItemList[i].ToString());
            }
            //return string.Join(',',debugDisplay);
            return $"{NumOfInspections}//{string.Join(',', debugDisplay)}";
        }
    }
    public class Item
    {
        public Monkey Owner;
        public long Level;

        public Item(string item, Monkey monkey)
        {
            Level=Convert.ToUInt32(item);
            Owner = monkey;
        }
        public override string ToString()
        {
            return Level.ToString();
        }
    }
}