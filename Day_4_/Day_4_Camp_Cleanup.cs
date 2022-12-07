namespace AoC2022
{
    public class cDay_4 : WeekN
    {
        readonly string[] inputLines;
        public cDay_4()
        {
            int x = 4;
            Title = $"--- Day {x}: Camp Cleanup ---";
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
            int countOfOverlapping = 0;
            foreach (string _line in inputLines)
            {
                cAssignment newAssignment = new cAssignment(_line);
                if (newAssignment.FullyContains())
                    countOfOverlapping+=1;
            }
            return $"4.1 - {countOfOverlapping}";
        }
        public override string Part2()
        {
            int countOfOverlapping = 0;
            foreach (string _line in inputLines)
            {
                cAssignment newAssignment = new cAssignment(_line);
                if (newAssignment.Overlapping())
                    countOfOverlapping += 1;
            }
            return $"4.2 - {countOfOverlapping}";
        }
    }
    public class cAssignment
    {
        class cRange
        {
            public int from;
            public int to;
            public cRange(string from_to)
            {
                string[] strings = from_to.Split('-');
                from = Convert.ToInt32(strings[0]);
                to = Convert.ToInt32(strings[1]);
            }
        }
        cRange firstElf, secondElf;

        public cAssignment(string assignemnt)
        {
            string[] strings = assignemnt.Split(',');
            firstElf = new cRange(strings[0]);
            secondElf = new cRange(strings[1]);
        }
        public bool FullyContains()
        {
            if (firstElf.from <= secondElf.from && firstElf.to >= secondElf.to)
                return true;
            else if (firstElf.from >= secondElf.from && firstElf.to <= secondElf.to)
                return true;
            else
                return false;
        }
        public bool Overlapping()
        {
            if (firstElf.from > secondElf.to)
                return false;
            else if (firstElf.to < secondElf.from)
                return false;
            else
                return true;
        }
    } 
}