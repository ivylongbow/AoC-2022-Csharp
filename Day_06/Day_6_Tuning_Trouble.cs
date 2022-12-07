namespace AoC2022
{
    
    public class cDay_6 : WeekN
    {
        readonly string[] inputLines;
        readonly int x = 6; // x = [1..25]
        public cDay_6()
        {
            Title = $"--- Day {x}: Tuning Trouble ---";
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
            string sequence = inputLines[0]; 
            for(int i = 0; i<sequence.Length;i++)
            {
                string subSequence = sequence.Substring(i, 4);
                if (subSequence.Distinct().Count() ==4)
                    return (i+4).ToString();
            }
            return $"{x}.1 - {1}";
        }
        public override string Part2()
        {
            string sequence = inputLines[0];
            for (int i = 0; i < sequence.Length; i++)
            {
                string subSequence = sequence.Substring(i, 14);
                if (subSequence.Distinct().Count() == 14)
                    return (i + 14).ToString();
            }
            return $"{x}.2 - {2}";
        }
    }
}