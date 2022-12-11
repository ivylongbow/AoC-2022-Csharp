// See https://aka.ms/new-console-template for more information

namespace AoC2022
{
    class Program
    {
        static void Main()
        {
            string[] solution = File.ReadAllLines("../../../../AoC-2022-private.sln");
            foreach(string s in solution)
                Console.WriteLine(s);
        }
    }
}