namespace AoC2022
{    
    public class cDay_17 : WeekN
    {
        readonly string[] inputLines;
        readonly int x = 17; // x = [1..25]
        readonly Chamber TheChamber;
        public cDay_17()
        {
            Title = $"--- Day {x}: Pyroclastic Flow ---";
            inputLines = ReadInput($"input_Day{x}.txt");
            //inputLines = ReadInput("");
            TheChamber = new Chamber(inputLines[0]);
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
            while (TheChamber.NextShapeID < 2022)
                TheChamber.DropOneRock();
            return $"{x}.1 - {TheChamber.Height}";
        }
        public override string Part2()
        {
            // loop until find the repeating pattern of dropping Rocks
            while (TheChamber.PatternRepeat == (0,0))
                TheChamber.DropOneRock();
            // calculating how many 'Extra' Rock need to be dropped except the repeating parts
            long modulo = 1000000000000 % TheChamber.PatternRepeat.Drop;
            // continue to the next point where it could be the history of 1000000000000.
            while (TheChamber.NextShapeID % TheChamber.PatternRepeat.Drop != modulo)
                TheChamber.DropOneRock();
            // calculating the pattern repeat count
            long repeats = (1000000000000 - TheChamber.NextShapeID) / TheChamber.PatternRepeat.Drop;
            // result will be current Height + repeat count * pattern's Height
            return $"{x}.2 - {TheChamber.Height + repeats * TheChamber.PatternRepeat.Height}";
        }
        class Chamber
        {
            public long Height = 0;
            Dictionary<long, Rock> Rocks = new();
            Dictionary<string, long> Bricks = new();    
            string[] Shape = { "-","+","L","I","o" };   // Rock generating Sequence, the same order repeats.
            char[] JetPattern;                          // Gas jet pattern, from the input.
            public long NextShapeID = 0;                // index of Rock Shape
            public long NextJetID = 0;                  // index of JetPattern
            public (long Height, long Drop) PatternRepeat =(0, 0);      // for recording the repeating pattern of dropping Rocks.

            Dictionary<string, (long Height, long Drop)> Signitures = new();
            public Chamber(string input)
            {
                JetPattern = input.ToCharArray();
            }
            public void DropOneRock()
            {
                Rock _newRock = new Rock(Height, Shape[(NextShapeID++ % 5)]);
                while (true)
                {
                    // before actual move, I need to check if I have faced same conditions in history
                    // conditions include
                    //              1. top x lines of Chamber, set x = 10 in my case
                    //              2. next Rock index in Rock queue
                    //              3. Next move index in jet queue [this is filtered by always do check at the beginning of input sequence]
                    // if all 3 criteria match, it will repeat the history, and I will get the repeatition properties: count of Rocks & increase of Height
                    if (NextJetID >0 && NextJetID % JetPattern.Length == 0)
                    {                        
                        string signiture = $"NextShape: {NextShapeID % 5}" + Environment.NewLine;
                        for (long i = Height + 5; i > Height - 5; i--)
                        {
                            signiture += "|";
                            for (int j = 0; j < 7; j++)
                                if (_newRock.Bricks.Contains((j, i)))
                                    signiture += "@";
                                else if (Bricks.ContainsKey($"{j},{i}"))
                                    signiture += "#";
                                else
                                    signiture += ".";
                            signiture += "|" + Environment.NewLine;
                        }
                        if (!Signitures.ContainsKey(signiture))
                            Signitures.Add(signiture, (-Height, -NextShapeID));
                        else if (PatternRepeat==(0,0))
                        {
                            PatternRepeat = Signitures[signiture];
                            PatternRepeat.Height += Height;
                            PatternRepeat.Drop += NextShapeID;
                            // Console.WriteLine($"DeltaHeight = {PatternRepeat.Height}, DeltaShape = {PatternRepeat.Drop}, found on {NextShapeID}");
                        }
                        // Console.WriteLine($"{signiture} {Environment.NewLine} NextShape: {NextShapeID}");
                    }                        
                    // step 1. Try move horizontal
                    Rock _PossibleMove = _newRock.PushToward(JetPattern[(NextJetID++ % JetPattern.Length)]);                    
                    if (IsValid(_PossibleMove))
                        _newRock= _PossibleMove;
                    // step 2. Try fall
                    _PossibleMove = _newRock.Fall();
                    if (IsValid(_PossibleMove))
                        _newRock = _PossibleMove;
                    else 
                    // stop condition met, merge the Rock into the Chamber wall, and update the Chamber's Height.
                    {
                        Rocks.Add(NextShapeID, _newRock);
                        foreach (var B in _newRock.Bricks)
                        {
                            Bricks.Add($"{B.X},{B.Y}", NextShapeID);
                            if (Height <= B.Y)
                                Height = B.Y + 1;
                        }
                        break;
                    }                        
                }
            }
            /// <summary>
            /// this private function checks if the Rock's location is acceptable by the Chamber.
            /// </summary>
            /// <param name="rock"></param>
            /// <returns>true if Rock can fit in.</returns>
            bool IsValid(Rock rock)
            {
                foreach (var B in rock.Bricks)
                    if (Bricks.ContainsKey($"{B.X},{B.Y}") || B.X < 0 || B.X > 6 || B.Y < 0)
                        return false;
                
                return true;
            }
        }
        class Rock
        {
            /// <summary>
            /// record the locations of occupation
            /// </summary>
            public List<(int X, long Y)> Bricks = new();
            /// <summary>
            /// Constructor of Rock class, this function is for creating a new Rock and drop it where 
            /// its left edge is two units away from the left wall and its bottom edge is three 
            /// units above the highest rock in the room (or the floor, if there isn't one).
            /// </summary>
            /// <param name="InitialHeight">Rock will be created at three units above the InitialHeight</param>
            /// <param name="Shape">one of '-' '+' 'L' 'I' 'o'</param>
            public Rock(long InitialHeight, string Shape)
            {
                switch(Shape)
                {
                    case ("-"):
                        Bricks.Add((2, 3 + InitialHeight));
                        Bricks.Add((3, 3 + InitialHeight));
                        Bricks.Add((4, 3 + InitialHeight));
                        Bricks.Add((5, 3 + InitialHeight));
                        break;
                    case ("+"):
                        Bricks.Add((3, 3 + InitialHeight));
                        Bricks.Add((2, 4 + InitialHeight));
                        Bricks.Add((3, 4 + InitialHeight));
                        Bricks.Add((4, 4 + InitialHeight));
                        Bricks.Add((3, 5 + InitialHeight));
                        break;
                    case ("L"):
                        Bricks.Add((2, 3 + InitialHeight));
                        Bricks.Add((3, 3 + InitialHeight));
                        Bricks.Add((4, 3 + InitialHeight));
                        Bricks.Add((4, 4 + InitialHeight));
                        Bricks.Add((4, 5 + InitialHeight));
                        break;
                    case ("I"):
                        Bricks.Add((2, 3 + InitialHeight));
                        Bricks.Add((2, 4 + InitialHeight));
                        Bricks.Add((2, 5 + InitialHeight));
                        Bricks.Add((2, 6 + InitialHeight));
                        break;
                    case ("o"):
                        Bricks.Add((2, 3 + InitialHeight));
                        Bricks.Add((3, 3 + InitialHeight));
                        Bricks.Add((2, 4 + InitialHeight));
                        Bricks.Add((3, 4 + InitialHeight));
                        break;
                }
            }
            /// <summary>
            /// this constructor is for moving the Rock: first created at the new location then check if the new one is valid.
            /// </summary>
            public Rock()
            { }
            /// <summary>
            /// Rock gets pushed around by jets of hot gas
            /// </summary>
            /// <param name="Direction"></param>
            /// <returns></returns>
            public Rock PushToward(char Direction)
            {
                Rock newRock = new();
                switch(Direction)
                {
                    case ('<'):
                        foreach(var _brick in Bricks)
                            newRock.Bricks.Add((_brick.X-1, _brick.Y));
                        break;
                    case ('>'):
                        foreach (var _brick in Bricks)
                            newRock.Bricks.Add((_brick.X+1, _brick.Y));
                        break;
                }
                return newRock;
            }
            /// <summary>
            /// fall one unit down, this logically can be merged to the above "PushToward()", but I'm lazy...
            /// </summary>
            /// <returns></returns>
            public Rock Fall()
            {
                Rock newRock = new();
                foreach (var _brick in Bricks)
                    newRock.Bricks.Add((_brick.X, _brick.Y-1));
                return newRock;
            }
        }
    }
}