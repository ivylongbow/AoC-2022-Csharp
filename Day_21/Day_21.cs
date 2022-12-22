namespace AoC2022
{    
    public class cDay_21 : WeekN
    {
        readonly string[] inputLines;
        readonly int x = 21; // x = [1..25]
        Monkeys monkeys;
        public cDay_21()
        {
            Title = $"--- Day {x}: Monkey Math ---";
            inputLines = ReadInput($"input_Day{x}.txt");
            //inputLines = ReadInput("");
            monkeys = new(inputLines);
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
            
            return $"{x}.1 - {monkeys["root"].Yell()}";
        }
        public override string Part2()
        {
            return $"{x}.2 - {monkeys.TraceBack()}";
        }

        class Monkey
        {
            Monkeys MonkeyDict;
            public string Name;
            public long Value = 0;
            public List<string> Others = new();
            string Operator = "";
            public bool contains_Humn = false;
            public Monkey(string input, Monkeys Group)
            {
                MonkeyDict = Group;
                string[] info = input.Split(':', StringSplitOptions.TrimEntries);
                Name = info[0];
                string[] parts = info[1].Split(' ');
                if (parts.Length == 1)
                    Value = long.Parse(parts[0]);
                else
                {
                    Others.Add(parts[0]);
                    Others.Add(parts[2]);
                    Operator= parts[1];
                }
            }
            public long GetValueTahMatchValueTask(long Value)
            { 
            
                if (this.Name == "humn")
                    return Value;
                else if (!contains_Humn)
                    return Value;
                else
                {
                    if (MonkeyDict[Others[0]].contains_Humn)
                    {
                        if (this.Operator == "-")
                        { MonkeyDict[Others[0]].GetValueTahMatchValueTask(Value + MonkeyDict[Others[1]].Value); }
                        else if (Operator == "+")
                        { MonkeyDict[Others[0]].GetValueTahMatchValueTask(Value - MonkeyDict[Others[1]].Value); }
                        else if (Operator == "*")
                        { MonkeyDict[Others[0]].GetValueTahMatchValueTask(Value / MonkeyDict[Others[1]].Value); }
                        else if (Operator == "/")
                        { MonkeyDict[Others[0]].GetValueTahMatchValueTask(Value * MonkeyDict[Others[1]].Value); }
                        else
                            return Value;
                    }
                    else
                    {
                        if (this.Operator == "-")
                        { MonkeyDict[Others[1]].GetValueTahMatchValueTask(MonkeyDict[Others[0]].Value - Value); }
                        else if (Operator == "+")
                        { MonkeyDict[Others[1]].GetValueTahMatchValueTask(Value - MonkeyDict[Others[0]].Value); }
                        else if (Operator == "*")
                        { MonkeyDict[Others[1]].GetValueTahMatchValueTask(Value / MonkeyDict[Others[0]].Value); }
                        else if (Operator == "/")
                        { MonkeyDict[Others[1]].GetValueTahMatchValueTask(MonkeyDict[Others[0]].Value / Value); }
                        else
                            return Value;
                    }
                }
                return Value;
            }
            public (bool,long) Yell()
            {

                if (this.Name == "humn")
                    this.contains_Humn=true;
                if (Others.Count == 0)
                    return (contains_Humn, Value);
                else
                    return Operation();
            }
            (bool, long) Operation()
            {
                (bool, long) result1;
                (bool, long) result2;
                switch (Operator)
                {
                    case "+":
                        result1 = MonkeyDict[Others[0]].Yell(); 
                        result2 = MonkeyDict[Others[1]].Yell();
                        this.contains_Humn= result1.Item1 || result2.Item1;
                        this.Value = result1.Item2 + result2.Item2;
                        break;

                    case "-":
                        result1 = MonkeyDict[Others[0]].Yell();
                        result2 = MonkeyDict[Others[1]].Yell();
                        this.contains_Humn = result1.Item1 || result2.Item1;
                        this.Value = result1.Item2 - result2.Item2;
                        break;

                    case "*":
                        result1 = MonkeyDict[Others[0]].Yell();
                        result2 = MonkeyDict[Others[1]].Yell();
                        this.contains_Humn = result1.Item1 || result2.Item1;
                        this.Value = result1.Item2 * result2.Item2;
                        break;

                    case "/":
                        result1 = MonkeyDict[Others[0]].Yell();
                        result2 = MonkeyDict[Others[1]].Yell();
                        this.contains_Humn = result1.Item1 || result2.Item1;
                        this.Value = result1.Item2 / result2.Item2;
                        break;
                }
                return (contains_Humn, Value);
            }
        }
        class Monkeys:Dictionary<string,Monkey>
        {
            public Monkeys(string[] inputLines)
            {
                foreach (string line in inputLines)
                {
                    Monkey NewMonkey = new Monkey(line, this);
                    this.Add(NewMonkey.Name, NewMonkey);
                }
            }
            public long TraceBack()
            {
                string m1 = this["root"].Others[0];
                string m2 = this["root"].Others[1];

                Monkey M1 = this[m1];
                Monkey M2 = this[m2];
                if (M1.contains_Humn) 
                {
                    return M1.GetValueTahMatchValueTask(M2.Value);
                }
                else
                {
                    return M2.GetValueTahMatchValueTask(M1.Value);
                }
            }
        }
    }
}