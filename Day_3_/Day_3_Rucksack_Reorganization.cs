using System.Text;

namespace AoC2022
{
    
    public class cDay_3 : WeekN
    {
        readonly string[] inputLines;
        public cDay_3()
        {
            int x = 3;
            Title = $"--- Day {x}: Rucksack Reorganization ---";
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
            int sumOfPriority = 0;
            foreach (string line in inputLines)
            {
                int length=line.Length;
                char[] leftCompartment = line[..(length / 2)].ToCharArray();
                char[] rightCompartment = line[(length / 2)..].ToCharArray();
                char[] commonItems = leftCompartment.Intersect(rightCompartment).ToArray();
                if (commonItems.Count() == 1)
                {
                    int priority = Encoding.ASCII.GetBytes(commonItems)[0] - Encoding.ASCII.GetBytes("a")[0] + 1;
                    if (priority < 0)
                        priority = Encoding.ASCII.GetBytes(commonItems)[0] - Encoding.ASCII.GetBytes("A")[0] + 27;
                    sumOfPriority += priority;
                }
            }
            return $"3.1 - {sumOfPriority}";
        }
        public override string Part2()
        {
            int sumOfPriority = 0;
            int elf = 0;
            char[][] rucksack = new char[4][];
            foreach (string line in inputLines)
            { 
                if (elf++ < 2)
                {
                    rucksack[elf] = line.ToCharArray();
                }
                else //if(elf++ == 2)
                {
                    rucksack[elf] = line.ToCharArray();
                    elf=0;
                    // 
                    char[] commonItems = rucksack[1].Intersect(rucksack[2]).Intersect(rucksack[3]).ToArray();
                    if (commonItems.Count() == 1)
                    {
                        int priority = Encoding.ASCII.GetBytes(commonItems)[0] - Encoding.ASCII.GetBytes("a")[0] + 1;
                        if (priority < 0)
                            priority = Encoding.ASCII.GetBytes(commonItems)[0] - Encoding.ASCII.GetBytes("A")[0] + 27;

                        //int total = rucksack[1].ToList().FindAll(x => x == commonItems[0]).Count();
                        //total += rucksack[2].ToList().FindAll(x => x == commonItems[0]).Count();
                        //total += rucksack[3].ToList().FindAll(x => x == commonItems[0]).Count();
                        sumOfPriority += priority;
                    }
                }
            }
            return $"3.2 - {sumOfPriority}";
        }
    }
}