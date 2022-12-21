namespace AoC2022
{    
    public class cDay_20 : WeekN
    {
        readonly string[] inputLines;
        readonly int x = 20; // x = [1..25]
        Loop InfiniteLoop;

        public cDay_20()
        {
            Title = $"--- Day {x}: Grove Positioning System ---";
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
            InfiniteLoop = new Loop(inputLines, 1);
            foreach (LoopItem item in InfiniteLoop)
            {
                item.Move(item.Value);
            }
            long x1000 = (InfiniteLoop.GetItemAt(1000).Value);
            long x2000 = (InfiniteLoop.GetItemAt(2000).Value);
            long x3000 = (InfiniteLoop.GetItemAt(3000).Value);

            return $"{x}.1 - {x1000+x2000+x3000}";
        }
        public override string Part2()
        {
            InfiniteLoop = new(inputLines, 811589153);
            for (int i = 0; i < 10; i++)
            foreach (LoopItem item in InfiniteLoop)
            {
                item.Move(item.Value);
            }
            long x1000 = (InfiniteLoop.GetItemAt(1000).Value);
            long x2000 = (InfiniteLoop.GetItemAt(2000).Value);
            long x3000 = (InfiniteLoop.GetItemAt(3000).Value);

            return $"{x}.2 - {x1000 + x2000 + x3000}";
        }
        class LoopItem
        {
            public long Value;
            public LoopItem Previous;
            public LoopItem Next;
            public LoopItem(long value)
            {
                Value = value;
                this.Previous = this;
                this.Next = this;
            }
            public LoopItem(long value, LoopItem before, LoopItem after)
            {
                Value = value;
                this.Previous = before;
                this.Next = after;
            }
            void MoveBackward(long steps)
            {
                LoopItem Before = this;
                for( int i = 0; i < steps; i++)
                    Before= Before.Previous;
                LoopItem After = Next;

                this.Previous.Next = this.Next;
                this.Next.Previous = this.Previous;
                //
                this.Previous = Before.Previous;
                this.Next = Before;
                //
                Before.Previous.Next = this;
                Before.Previous = this;


            }
            void MoveForeward(long steps)
            {
                LoopItem Before = Previous;
                LoopItem After = this;
                for (int i = 0; i < steps; i++)
                    After = After.Next;
                //
                this.Next.Previous = this.Previous;
                this.Previous.Next = this.Next;
                //
                this.Next = After.Next;
                this.Previous = After;
                //
                After.Next.Previous = this;                
                After.Next = this;
            }
            public void Move(long steps)
            {
                steps %= 4999;         
                if (steps < 0)
                    MoveBackward(-steps);                   
                else if (steps > 0)
                    MoveForeward(steps);
            }
        }
        class Loop : List<LoopItem>
        {
            public LoopItem Head;
            public Loop(string[] inputLines,long Ratio )
            {
                Head = new LoopItem(long.Parse(inputLines[0]) * Ratio);
                Add(Head);
                for (int i = 1; i < inputLines.Length; i++)
                {
                    InsertItem(long.Parse(inputLines[i]) * Ratio);
                }
            }
            public void InsertItem(long n)
            {
                LoopItem N = new LoopItem(n,Head.Previous,Head);
                Head.Previous.Next = N;
                Head.Previous = N;
                Add(N);
            }
            public LoopItem GetItemAt(int Index)
            {
                LoopItem I = Head;
                while (I.Value != 0)
                    I = I.Next;
                for (int i = 0; i < Index % (this.Count); i++)
                    I = I.Next;
                return I;
            }
            public override string ToString()
            {
                string output = $"{Head.Value}";
                for (LoopItem i = Head.Next; i != Head; i = i.Next)
                    output += $", {i.Value}";
                return output;
            }
        }
    }
}