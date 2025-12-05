namespace AdventOfCode._2025;

public sealed class Day5 : IDay
{
    private sealed record IdRange(long Start, long End);
    
    public string SolvePartOne()
    {
        using var stream = new StreamReader("2025/input1.txt");
        var ranges = new List<IdRange>();

        var isRange = true;
        var result = 0;
        while (!stream.EndOfStream)
        {
            var line = stream.ReadLine()!;
            if (line == "")
            {
                isRange = false;
                continue;
            }

            if (isRange)
            {
                var split = line.Split("-");
                ranges.Add(new IdRange(long.Parse(split[0]), long.Parse(split[1])));
            }
            else
            {
                var id = long.Parse(line);
                if (ranges.Any(l => l.Start <= id && l.End >= id))
                {
                    result++;
                }
            }
        }

        return result.ToString();
    }

    public string SolvePartTwo()
    {
        using var stream = new StreamReader("2025/input1.txt");
        var ranges = new HashSet<IdRange>();

        while (!stream.EndOfStream)
        {
            var line = stream.ReadLine()!;
            if (line == "")
            {
                break;
            }

            var split = line.Split("-");
            var range = new IdRange(long.Parse(split[0]), long.Parse(split[1]));

            var overlappingRanges = ranges.Where(r => r.Start <= range.End && range.Start <= r.End)
                .ToHashSet();
            if (overlappingRanges.Count > 0)
            {
                overlappingRanges.Add(range);
                var overlappingRange = new IdRange(
                    overlappingRanges.MinBy(r => r.Start)!.Start,
                    overlappingRanges.MaxBy(r => r.End)!.End);
                ranges.RemoveWhere(r => overlappingRanges.Contains(r));
                ranges.Add(overlappingRange);
                continue;
            }
            
            ranges.Add(range);
        }

        return ranges.Sum(x => x.End - x.Start + 1L).ToString();
    }
}