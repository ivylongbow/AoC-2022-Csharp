namespace AoC2022
{
    
    public class cDay_5 : WeekN
    {
        readonly string[] inputLines;
        string[]? stacks;
        public cDay_5()
        {
            int x = 5;
            Title = $"--- Day {x}: Supply Stacks ---";
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
            stacks = new string[10];
            for (int i = 7; i >= 0; i--)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (inputLines[i][j * 4 + 1] != ' ')
                    { stacks[j + 1] += inputLines[i][j * 4 + 1]; }
                }
            }
            string[] separator = { "move", "from", "to" };
            for (int i = 10; i < inputLines.Length; i++)
            {
                string[] instructions = inputLines[i].Split(separator, StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);
                int count = Convert.ToInt32(instructions[1]);
                int from = Convert.ToInt32(instructions[2]);
                int to = Convert.ToInt32(instructions[3]);

                stacks[to] = string.Concat(stacks[to].Concat(stacks[from].Reverse().Take(count)));
                stacks[from] = stacks[from].Remove(stacks[from].Length - count);
            }
            string top = "";
            for (int i = 1; i < 10; i++)
            {
                top += stacks[i].Last();
            }
            return $"5.1 - {top}";
        }
        public override string Part2()
        {
            stacks = new string[10];
            for (int i = 7; i >= 0; i--)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (inputLines[i][j * 4 + 1] != ' ')
                    { stacks[j + 1] += inputLines[i][j * 4 + 1]; }
                }
            }
            string[] separator = { "move", "from", "to" };
            for (int i = 10; i < inputLines.Length; i++)
            {
                string[] instructions = inputLines[i].Split(separator, StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);
                int count = Convert.ToInt32(instructions[1]);
                int from = Convert.ToInt32(instructions[2]);
                int to = Convert.ToInt32(instructions[3]);

                stacks[to] = string.Concat(stacks[to].Concat(stacks[from].TakeLast(count)));
                stacks[from] = stacks[from].Remove(stacks[from].Length - count);
            }
            string top = "";
            for (int i = 1; i < 10; i++)
            {
                top += stacks[i].Last();
            }
            return $"5.2 - {top}";
        }
    }
}