namespace AoC2022
{    
    public class cDay_25 : WeekN
    {
        readonly string[] inputLines;
        readonly int x = 25; // x = [1..25]
        public cDay_25()
        {
            Title = $"--- Day {x}: Full of Hot Air ---";
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
            long Sum = 0;
            foreach(string line in inputLines)
            {
                Numbers number = new Numbers(line);
                Sum += number.Decimal;
                Console.WriteLine(number.Decimal);
            }
            Console.WriteLine(Sum);
            return $"{x}.1 - {Numbers.ToSNAFU(Sum)}";
        }
        public override string Part2()
        {
            return $"{x}.2 - {"Merry Christmas! Ho~Ho~Ho~~!"}";
        }
        class Numbers
        {
            public long Decimal = 0;
            string SNAFU = "";
            public Numbers(string SNAFU)
            {
                this.SNAFU = SNAFU;
                foreach(char c in SNAFU)
                {
                    Decimal *= 5;
                    switch (c)
                    {
                        case '2':
                            Decimal += 2;
                            break;
                        case '1':
                            Decimal += 1;
                            break;
                        case '-':
                            Decimal -= 1;
                            break;
                        case '=':
                            Decimal -= 2;
                            break;
                    }
                }
            }
            public static string ToSNAFU(long Decimal)
            {
                string SNAFU = "";
                while (Decimal != 0)
                {
                    long r = (Decimal + 2) % 5 - 2;
                    Decimal = (Decimal - r) / 5;
                    switch (r)
                    {
                        case 0:
                            SNAFU = "0" + SNAFU;
                            break;
                        case 1:
                            SNAFU = "1" + SNAFU;
                            break;
                        case 2:
                            SNAFU = "2" + SNAFU;
                            break;
                        case -1:
                            SNAFU = "-" + SNAFU;
                            break;
                        case -2:
                            SNAFU = "=" + SNAFU;
                            break;
                    }
                }
                return SNAFU;
            }
        }
    }
}