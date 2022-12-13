namespace AoC2022
{    
    public class cDay_14 : WeekN
    {
        readonly string[] inputLines;
        readonly int x = 14; // x = [1..25]
        public cDay_14()
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
            return $"{x}.1 - {1}";
        }
        public override string Part2()
        {
            return $"{x}.2 - {2}";
        }
    }
}