namespace AoC2022
{
    
    public class cDay_18 : WeekN
    {
        readonly string[] inputLines;
        readonly int x = 18; // x = [1..25]
        Lava? Droplet;
        Pond? WaterSurround;
        public cDay_18()
        {
            Title = $"--- Day {x}: Boiling Boulders ---";
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
            Droplet = new(inputLines);
            Droplet.CalculateConnections();
            return $"{x}.1 - {Droplet.Count * 6 - Droplet.ConnectionList.Count}";
        }
        public override string Part2()
        {
            WaterSurround = new(Droplet!);
            WaterSurround.CalculateConnections();            
            int result = WaterSurround.Count * 6 - WaterSurround.ConnectionList.Count - WaterSurround.CubicOuterSurfacesArea;
            return $"{x}.2 - {result}";
        }

        class Cube
        {
            public int x { get; set; }
            public int y { get; set; }
            public int z { get; set; }
            public bool isExternal = false;
            public Cube(string input)
            {
                string[] Coordinates = input.Split(',');
                x=int.Parse(Coordinates[0]);
                y=int.Parse(Coordinates[1]);
                z=int.Parse(Coordinates[2]);
            }
            public override string ToString()
            {
                return $"{x},{y},{z}";
            }
            public bool IsAdjacentTo(Cube another)
            {
                return (x == another.x && y == another.y && Math.Abs(z - another.z) == 1 ||
                    x == another.x && Math.Abs(y - another.y) == 1 && z == another.z ||
                    Math.Abs(x - another.x) == 1 && y == another.y && z == another.z);
            }
            public List<string> Adjacents()
            {
                string[] neighbors = { $"{x},{y},{z + 1}", $"{x},{y},{z - 1}", $"{x},{y + 1},{z}", $"{x},{y - 1},{z}", $"{x + 1},{y},{z}", $"{x - 1},{y},{z}" };
                return neighbors.ToList();
            }

        }
        class Lava : Dictionary<string, Cube>
        {
            public (int Max, int Min) X_range = (0, 100);
            public (int Max, int Min) Y_range = (0, 100);
            public (int Max, int Min) Z_range = (0, 100);
            public List<string> ConnectionList = new List<string>();

            public Lava(string[] inputLines)
            {
                foreach(string line in inputLines)
                {
                    this.Add(line, new Cube(line));
                }
                foreach(Cube cube in this.Values)
                {
                    if (cube.x >= X_range.Max)
                        X_range.Max = cube.x + 1;
                    if (cube.y >= Y_range.Max)
                        Y_range.Max = cube.y + 1;
                    if (cube.z >= Z_range.Max)
                        Z_range.Max = cube.z + 1;
                    if (cube.x <= X_range.Min)
                        X_range.Min = cube.x - 1;
                    if (cube.y <= Y_range.Min)
                        Y_range.Min = cube.y - 1;
                    if (cube.z <= Z_range.Min)
                        Z_range.Min = cube.z - 1;
                }
            }
            public Lava()
            { }
            public void CalculateConnections()
            {
                foreach (Cube A in this.Values)
                    foreach (Cube B in this.Values)
                        if (A.IsAdjacentTo(B))
                            ConnectionList.Add($"[{A}]-[{B}]");
            }
        }
        class Pond : Lava
        {
            public int CubicOuterSurfacesArea;
            public Pond(Lava _droplet) 
            {
                X_range = _droplet.X_range;
                Y_range = _droplet.Y_range;
                Z_range = _droplet.Z_range;
                for (int x = _droplet.X_range.Min; x <= _droplet.X_range.Max; x++)
                    for (int y = _droplet.Y_range.Min; y <= _droplet.Y_range.Max; y++)
                        for (int z = _droplet.Z_range.Min; z <= _droplet.Z_range.Max; z++)
                            this.Add($"{x},{y},{z}", new Cube($"{x},{y},{z}"));
                foreach (string cube in _droplet.Keys)
                    this.Remove (cube);

                int X_Length = (_droplet.X_range.Max - _droplet.X_range.Min + 1);
                int Y_Length = (_droplet.Y_range.Max - _droplet.Y_range.Min + 1);
                int Z_Length = (_droplet.Z_range.Max - _droplet.Z_range.Min + 1);
                CubicOuterSurfacesArea = X_Length * Y_Length * 2 + Y_Length * Z_Length * 2 + X_Length * Z_Length * 2;
                
                // Scan Exterier 

                string StartingCorner = $"{X_range.Min},{Y_range.Min},{Z_range.Min}";
                this[StartingCorner].isExternal = true;
                List<string> PendingCheck = this[StartingCorner].Adjacents().FindAll(InRange);
                List<string> PendingCheckAppend = new();
                while (PendingCheck.Count > 0)
                {
                    foreach (string key in PendingCheck)
                    {
                        this[key].isExternal = true;
                        PendingCheckAppend.AddRange(this[key].Adjacents());
                    }
                    PendingCheck.Clear();
                    PendingCheck.AddRange(PendingCheckAppend.Distinct().Where(InRange));
                }
                foreach( string key in this.Keys.Where(IsInternal))
                    this.Remove(key);
                //
                bool InRange(string Coord)
                {
                    if (!ContainsKey(Coord))
                        return false;
                    else if (this[Coord].isExternal)
                        return false;
                    else 
                        return true;
                }
                bool IsInternal(string key) => !this[key].isExternal;
            }
        }
    }
}