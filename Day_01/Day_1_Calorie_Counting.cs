namespace AoC2022
{
    public class cDay_1 : WeekN
    {
        readonly string[] inputCalories;
        Dictionary<int, long> ElvesCalorieRecord;
        public cDay_1()
        {
            int x = 1;
            Title = $"--- Day {x}: Calorie Counting ---";
            inputCalories = ReadInput($"input_Day{x}.txt");
            ElvesCalorieRecord = new Dictionary<int, long>();
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
            int currentElf = 0;
            ElvesCalorieRecord.Add(currentElf, 0);
            foreach (string input in inputCalories)
            {
                if (!string.IsNullOrEmpty(input))
                {
                    ElvesCalorieRecord[currentElf] += Convert.ToInt64(input);
                }
                else
                {
                    ElvesCalorieRecord.Add(++currentElf, 0);
                }
            }
            return $"1.1 - {ElvesCalorieRecord.Values.Max()}";
        }
        public override string Part2()
        {
            List<long> sortedValues = ElvesCalorieRecord.Values.ToList();
            sortedValues.Sort();
            sortedValues.Take(3).Sum().ToString();

            return $"1.2 - {sortedValues.TakeLast(3).Sum()}";
        }
    }
}