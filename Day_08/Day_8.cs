namespace AoC2022
{    
    public class cDay_8 : WeekN
    {
        readonly string[] inputLines;
        readonly int x = 8; // x = [1..25]
        readonly CForrest Forrest;
        public cDay_8()
        {
            Title = $"--- Day {x}: Treetop Tree House ---";
            inputLines = ReadInput($"input_Day{x}.txt");
            //inputLines = ReadInput(null);
            Forrest = new(inputLines);
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
            Forrest.UpdateVisibility();
            return $"{x}.1 - {Forrest.VisibleCount}";
        }
        public override string Part2()
        {
            Forrest.UpdateViewScore();
            return $"{x}.2 - {Forrest.BestViewScore}";
        }
    }
    public class CForrest
    {
        CTree[][] Tree;
        int iMax;
        int jMax;
        public long VisibleCount = 0;
        CTree? BestViewTree;
        public int BestViewScore = 0;
        public CForrest(string[] inputLines)
        {
            iMax=inputLines.Length;
            jMax = inputLines[0].Length;
            Tree = new CTree[iMax][];
            for (int i = 0; i < iMax; i++)
            {
                Tree[i] = new CTree[jMax];
                for (int j = 0; j < jMax; j++)
                {
                    Tree[i][j] = new CTree(inputLines[i][j],i,j);
                }
            }
        }
        class CTree
        {
            public int Height;
            public bool Visible = false;
            public int  X,Y,L,R,U,D;
            public CTree(char Height, int i, int j)
            {
                this.Height = Convert.ToInt32(Height);
                this.Y = i;
                this.X = j;
            }
            public int ViewScore()
            { 
                return L*R*U*D;
            }               
        }
        public void UpdateVisibility()
        {
            VisibleCount = 4;
            // left - right
            for (int i = 1; i < iMax - 1; i++)
            {
                int lastVisibleHeight = -1;
                for (int j = 0; j < jMax -1; j++)
                {
                    if (Tree[i][j].Height > lastVisibleHeight)
                    {
                        Tree[i][j].Visible = true;
                        lastVisibleHeight = Tree[i][j].Height;
                    }
                }
                lastVisibleHeight = -1;
                for (int j = jMax - 1; j > 0 ; j--)
                {
                    if (Tree[i][j].Height > lastVisibleHeight)
                    {
                        Tree[i][j].Visible = true;
                        lastVisibleHeight = Tree[i][j].Height;
                    }
                }
            }
            // top - bottom
            for (int j = 1; j < jMax - 1; j++)
            {
                int lastVisibleHeight = -1;
                for (int i = 0; i < iMax - 1; i++)
                {
                    if (Tree[i][j].Height > lastVisibleHeight)
                    {
                        Tree[i][j].Visible = true;
                        lastVisibleHeight = Tree[i][j].Height;
                    }
                }
                lastVisibleHeight = -1;
                for (int i = iMax - 1; i > 0; i--)
                {
                    if (Tree[i][j].Height > lastVisibleHeight)
                    {
                        Tree[i][j].Visible = true;
                        lastVisibleHeight = Tree[i][j].Height;
                    }
                }
            }
            for(int i = 0; i < iMax; i++)
            {
                for(int j = 0; j < jMax; j++)
                {
                    if (Tree[i][j].Visible)
                    VisibleCount += 1;
                }
            }
        }
        public void UpdateViewScore()
        {
            BestViewScore = 0;
            for (int i = 1; i < iMax-1; i++)
            {
                for (int j = 1; j < jMax-1; j++)
                {
                    // L
                    for (int x = Tree[i][j].X - 1; x >= 0; x--)
                    {
                        Tree[i][j].L += 1;
                        if (Tree[i][x].Height >= Tree[i][j].Height)
                            break;
                    }
                    // R
                    for (int x = Tree[i][j].X + 1; x <jMax; x++)
                    {
                        Tree[i][j].R += 1;
                        if (Tree[i][x].Height >= Tree[i][j].Height)
                            break;
                    }
                    // U
                    for (int y = Tree[i][j].Y - 1; y >= 0; y--)
                    {
                        Tree[i][j].U += 1;
                        if (Tree[y][j].Height >= Tree[i][j].Height)
                            break;
                    }
                    // D
                    for (int y = Tree[i][j].Y + 1; y < iMax; y++)
                    {
                        Tree[i][j].D += 1;
                        if (Tree[y][j].Height >= Tree[i][j].Height)
                            break;
                    }
                    int ViewScore = Tree[i][j].ViewScore();
                    if (ViewScore > BestViewScore)
                    {
                        BestViewScore = ViewScore;
                        BestViewTree = Tree[i][j];
                    }
                }
            }
        }
    }
}