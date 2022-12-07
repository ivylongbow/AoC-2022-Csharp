namespace AoC2022
{    
    public class cDay_7 : WeekN
    {
        readonly string[] inputLines;
        readonly int x = 7; // x = [1..25]
        public cDay_7()
        {
            Title = $"--- Day {x}: xxxxxxxx xxxxxxxx ---";
            inputLines = ReadInput($"input_Day{x}.txt");
        }
        public string[] ReadInput(string? fileName)
        {
            if (fileName == null)
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