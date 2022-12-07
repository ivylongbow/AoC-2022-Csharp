using System.Text;

namespace AoC2022
{
    
    public class cDay_4 : WeekN
    {
        readonly string[] inputLines;
        public cDay_4()
        {
            int x = 2;
            Title = $"--- Day {x}: Rock Paper Scissors ---";
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
            int totalScore = 0;
            foreach (var line in inputLines)
            {
                cGameRound thisRound = new cGameRound(line);
                totalScore += thisRound.Score(); 
            }
            return $"2.1 - {totalScore}";
        }
        public override string Part2()
        {
            int totalScore = 0;
            foreach (var line in inputLines)
            {
                cGameRound2 thisRound = new cGameRound2(line);
                totalScore += thisRound.Score();
            }
            return $"2.2 - {totalScore}";
        }
    }
    public class cGameRound
    {
        int opponent;
        int you;
        public cGameRound(string strategy)
            {
            opponent = Encoding.ASCII.GetBytes(strategy)[0] - Encoding.ASCII.GetBytes("A")[0] + 1;
            you = Encoding.ASCII.GetBytes(strategy)[2] - Encoding.ASCII.GetBytes("X")[0] + 1;
            }
        public int Score()
        {
            int total = 4 + you - opponent;
            total %= 3;
            total *= 3;
            total += you;
            return total;
        }
    }
    public class cGameRound2
    {
        int opponent;
        int winLose;
        public cGameRound2(string strategy)
        {
            opponent = (int)Encoding.ASCII.GetBytes(strategy)[0] - Encoding.ASCII.GetBytes("A")[0] + 1;
            winLose = Encoding.ASCII.GetBytes(strategy)[2] - Encoding.ASCII.GetBytes("Y")[0];
        }
        public int Score()
        {
            int total = (opponent + winLose + 2)%3 + 1;
            total += (winLose + 1) * 3;
            return total;
        }
    }
}