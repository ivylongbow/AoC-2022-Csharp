namespace AoC2022
{    
    public class cDay_7 : WeekN
    {
        readonly string[] inputLines;
        readonly int x = 7; // x = [1..25]
        CDirectory RootDirectory;
        readonly CDirectory currentDir;
        public cDay_7()
        {
            Title = $"--- Day {x}: No Space Left On Device ---";
            inputLines = ReadInput($"input_Day{x}.txt");
            RootDirectory = new CDirectory("/", null);
        
            currentDir = RootDirectory;
            foreach (string line in inputLines)
            {
                if (line.StartsWith("$ cd"))
                    currentDir = currentDir.CD(line.Substring(5));
                else if (!line.StartsWith("$ ls"))
                    currentDir.AddItem(line);
            }
        }
        public string[] ReadInput(string? fileName)
        {
            if (fileName == null)
                fileName = "input_sample.txt";

            return File.ReadAllLines(fileName);
        }
        public override string Part1()
        {
            long totalSize = RootDirectory.Total_size_below_100k();

            return $"{x}.1 - {totalSize}";
        }
        public override string Part2()
        {
            long requiredSpace = RootDirectory.Size() - 40000000;

            List<CDirectory> qualifiedList = RootDirectory.ListOfDirectoriesLargerThan(requiredSpace);

            long minSize = RootDirectory.Size();
            foreach (CDirectory qualified in qualifiedList)
                if (minSize > qualified.Size())
                    minSize = qualified.Size();

            return $"{x}.2 - {minSize}"; 
        }
    }
    class CDirectory
    {
        readonly string name = "";
        public CDirectory? UpperDirectory;
        readonly Dictionary<string,CDirectory> subFolders;
        readonly List<CFile> files;
        public CDirectory(string name, CDirectory? upperDirectory)
        {
            this.name = name;
            subFolders = new Dictionary<string, CDirectory>();
            files = new List<CFile>();
            UpperDirectory = upperDirectory;
        }
        public CDirectory CD(string folderName)
        {
            try
            {
                if (folderName == "/")
                    return UpperDirectory.CD("/");
                else if (folderName == "..")
                    return UpperDirectory;
                else
                    return subFolders[folderName]; 
            }
            catch
            { 
                return this; 
            }            
        }
        public void AddItem(string inputLine)
        {
            string[] fileSizeWithName = inputLine.Split(' ');
            if (fileSizeWithName.Length == 2)
            { if (fileSizeWithName[0] == "dir")
                {
                    string subFolderName = fileSizeWithName[1];
                    subFolders.Add(subFolderName, new CDirectory(subFolderName, this));
                }
            else
                {
                    long fileSize = Convert.ToInt64(fileSizeWithName[0]);
                    string fileName = fileSizeWithName[1];
                    files.Add(new CFile(fileName, fileSize));
                }
            }
        }
        public long Size()
        {
            long _size = 0;
            foreach(CFile _File in files)
                _size += _File.Size;
            foreach(CDirectory _subFolder in subFolders.Values)
                _size += _subFolder.Size();
            return _size;
        }
        public long Total_size_below_100k()
        {
            long _total_size = 0;
            foreach (CDirectory _subFolder in subFolders.Values)
                _total_size += _subFolder.Total_size_below_100k();
            long mySize = Size();
            if (mySize <= 100000)
                _total_size += mySize;
            return _total_size;
        }
        public List<CDirectory> ListOfDirectoriesLargerThan(long size)
        {
            List<CDirectory> _qualifiedList = new();
            foreach(CDirectory _subFolder in subFolders.Values)
                _qualifiedList.AddRange(_subFolder.ListOfDirectoriesLargerThan(size)); 
            if (Size() >= size && _qualifiedList.Count == 0)
                _qualifiedList.Add(this);
            return _qualifiedList;
        }
        public override string ToString()
        {
            return string.Join(" ", name, Size().ToString());
        }
        class CFile
        {
            readonly string Name = "";
            public long Size = 0;
            public CFile(string name, long  size)
            {
                this.Name = name;
                this.Size = size;
            }
            public override string ToString()
            {
                return string.Join(" ", Name, Size.ToString());
            }
        }
    }
}