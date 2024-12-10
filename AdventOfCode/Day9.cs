namespace AdventOfCode;

public sealed class Day9 : IDay
{
    public string SolvePartOne()
    {
        var map = new List<string>();
        var id = 0;
        var freeSpaceIndices = new Queue<int>();
        for (var i = 0; i < _diskMap.Length; i++)
        {
            var digit = _diskMap[i] - '0';
            if (i % 2 == 0)
            {
                for (var j = 0; j < digit; j++)
                {
                    map.Add($"{id}");
                }
                id++;
            }
            else
            {
                for (var j = 0; j < digit; j++)
                {
                    freeSpaceIndices.Enqueue(map.Count);
                    map.Add(".");
                }
            }
        }

        for (var i = map.Count - 1; i >= 0; i--)
        {
            if (map[i] == ".")
            {
                continue;
            }

            if (freeSpaceIndices.Count == 0)
            {
                break;
            }
            var freeSpaceIndex = freeSpaceIndices.Dequeue();
            if (freeSpaceIndex > i)
            {
                break;
            }
            (map[freeSpaceIndex], map[i]) = (map[i], map[freeSpaceIndex]);
        }

        return map.Where(x => x != ".").Select((x, i) => long.Parse(x) * i).Sum().ToString();
    }

    public string SolvePartTwo()
    {
        var map = new List<string>();
        var id = 0;
        var freeSpaces = new List<FreeSpace>();
        var files = new List<File>();
        for (var i = 0; i < _diskMap.Length; i++)
        {
            var digit = _diskMap[i] - '0';
            if (i % 2 == 0)
            {
                var startIndex = map.Count;
                for (var j = 0; j < digit; j++)
                {
                    map.Add($"{id}");
                }
                files.Insert(0, new File(digit, startIndex));
                id++;
            }
            else
            {
                var index = map.Count;
                for (var j = 0; j < digit; j++)
                {
                    map.Add(".");
                }

                if (digit > 0)
                {
                    freeSpaces.Add(new FreeSpace(digit, index));
                }
            }
        }

        foreach (var (size, fileIndex) in files)
        {
            var eligibleFreeSpace = freeSpaces.TakeWhile(freeSpace => freeSpace.Index < fileIndex)
                .Where(freeSpace => freeSpace.Size >= size);
            foreach (var freeSpace in eligibleFreeSpace)
            {
                for (var i = 0; i < size; i++)
                {
                    (map[freeSpace.Index + i], map[fileIndex + i]) = (map[fileIndex + i], map[freeSpace.Index + i]);
                }
                
                freeSpace.Size -= size;
                freeSpace.Index += size;
                break;
            }
        }

        return map.Select((x, i) => new { Block = x, Index = i })
            .Aggregate(0L, (checksum, file) => file.Block == "."
                    ? checksum
                    : checksum + file.Index * long.Parse(file.Block))
            .ToString();
    }

    private sealed record File(int Size, int Index);

    private sealed record FreeSpace(int Size, int Index)
    {
        public int Size { get; set; } = Size;
        public int Index { get; set; } = Index;
    }

    private readonly string _diskMap = DiskMap;
    private static readonly string DiskMap = "2333133121414131402";
}