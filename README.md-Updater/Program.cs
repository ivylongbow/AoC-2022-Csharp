﻿// See https://aka.ms/new-console-template for more information

namespace AoC2022
{
    class Program
    {
        static void Main()
        {
            int MaxLength = 0;
            List<string> Days = new List<string>();
            string[] solution = File.ReadAllLines("../../../../AoC-2022-private.sln");
            foreach(string s in solution)
            {
                if (s.StartsWith("Project("))
                {
                    string project = s.Split('=', StringSplitOptions.TrimEntries)[1].Split(',')[0];
                    string[] StoryName = project[1..(project.Length - 1)].Split('_');
                    if (StoryName.Length > 2)
                    { 
                        if (StoryName[0] == "Day" && StoryName[2].Length > 0)
                        {
                            Days.Add(string.Join('_', StoryName[1..]));
                            if (MaxLength < project.Length)
                                MaxLength = project.Length;
                        }
                    }
                }
                else if (s.StartsWith("Global"))
                    break;
            }

            
            int DayProgress = Days.Count;
            List<string> ReadMe = new()
            {
                "# Advent of Code 2022 by Troy in C#",
                "This README is inspired by [Marcus Shu](https://github.com/shulkx/advent-of-code/tree/main/adventofcode2022)",
                "",
                $"## Progression and Navigation    ![Progress](https://progress-bar.dev/{DayProgress}/?scale=25&title=Days&width=240&suffix=/25)",
                "",
                "| DAY                                                          | STARS | C#                            | Solution Description |",
                "| ------------------------------------------------------------ | ----- | ----------------------------- | -------------------- |"
            };
            for (int i = 1; i <= 6; i++)
            {
                string[] StoryName = Days[i - 1].Split('_');
                ReadMe.Add($"| [Day {StoryName[0]}: {string.Join(' ', StoryName[1..])}](https://adventofcode.com/2022/day/{i}){new string(' ', MaxLength - Days[i - 1].Length - i.ToString().Length)}| ⭐️⭐️ | [Solution](./Day_{i.ToString("00")}/Day_{Days[i - 1]}.cs){new string(' ', MaxLength - Days[i - 1].Length)}|                      |");
            }
            for (int i = 7; i<= DayProgress; i++)
            {
                string[] StoryName = Days[i-1].Split('_');
                ReadMe.Add($"| [Day {StoryName[0]}: {string.Join(' ', StoryName[1..])}](https://adventofcode.com/2022/day/{i}){new string(' ', MaxLength - Days[i - 1].Length - i.ToString().Length)}| ⭐️⭐️ | [Solution](./Day_{i.ToString("00")}/Day_{i}.cs){new string(' ', MaxLength - i.ToString().Length)}|                      |");
            }
            for (int i = DayProgress + 1; i<= 25; i++)
                ReadMe.Add($"| [Day {i}](https://adventofcode.com/2022/day/{i})              |       |                               |                      |");

            File.WriteAllLines("../../../../Readme_Test.md", ReadMe);
        }
    }
}