namespace AoC2022
{    
    public class cDay_10 : WeekN
    {
        readonly string[] inputLines;
        readonly int x = 10; // x = [1..25]
        public cDay_10()
        {
            Title = $"--- Day {x}: Cathode-Ray Tube ---";
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
            int i = 0;
            int x = 1;
            int result=0;
            foreach (string instruction in inputLines)
            {
                i++;
                if (i%40 == 20)
                {
                    result += x*i;
                }
                if (instruction.StartsWith("addx"))
                {
                    i++;
                    if (i % 40 == 20)
                    {
                        result += x*i;
                    }
                    x += Convert.ToInt32(instruction[5..]);                    
                }
            }
            return $"{x}.1 - {result}";
        }
        public override string Part2()
        {
            CRT _CRT = new CRT();
            int i = 0;
            int x = 1;
            foreach (string instruction in inputLines)
            {
                //i++;
                _CRT.Step(i++,x);
                if (instruction.StartsWith("addx"))
                {
                    //i++;
                    _CRT.Step(i++,x);
                    x += Convert.ToInt32(instruction[5..]);
                }
            }
            return $"{x}.2 - {_CRT.ToString()}";
        }
    }
    class CRT
    {
        char[] Dots;// = new char[240];
        public CRT()
        {
            Dots = new string('.',240).ToCharArray();
        }
        public void Step(int clk, int x)
        {
            if (Math.Abs(x-clk%40) < 2)
            {
                Dots[clk] = '#';
            }
        }
        public override string ToString()
        {
            string[] lines = new string[7];
            for (int i = 0; i<6;i++)
            {
                lines[i+1] = new string(Dots[(i*40)..(i*40+40)]);
            }
            return string.Join(System.Environment.NewLine, lines);
        }
    }
}