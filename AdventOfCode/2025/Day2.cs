namespace AdventOfCode._2025;

public sealed class Day2 : IDay
{
    private sealed record IdRange(long Start, long End);

    public string SolvePartOne()
    {
        using var reader = new StreamReader("2025/input1.txt");
        var line = reader.ReadLine();

        var ranges = line.Split(',')
            .Select(l => l.Split('-'))
            .Select(x => new IdRange(long.Parse(x[0]), long.Parse(x[1])));

        var result = 0L;

        foreach (var range in ranges)
        {
            for (var i = 1; i < 100_000; i++)
            {
                var invalidId = long.Parse($"{i}{i}");
                if (invalidId > range.End)
                {
                    break;
                }
                if (range.Start <= invalidId && range.End >= invalidId)
                {
                    result += invalidId;
                }
            }
        }

        return result.ToString();
    }

    public string SolvePartTwo()
    {
        using var reader = new StreamReader("2025/input1.txt");
        var line = reader.ReadLine();

        var ranges = line.Split(',')
            .Select(l => l.Split('-'))
            .Select(x => new IdRange(long.Parse(x[0]), long.Parse(x[1])));

        var result = 0L;

        foreach (var range in ranges)
        {
            var seen = new HashSet<long>();
            for (var i = 1; i < 100_000; i++)
            {
                var pattern = $"{i}";
                for (var j = 0; j < 10; j++)
                {
                    if (!long.TryParse($"{pattern}{i}", out var invalidId))
                    {
                        break;
                    }

                    if (invalidId > range.End)
                    {
                        break;
                    }
                    pattern = invalidId.ToString();
                    if (range.Start <= invalidId && range.End >= invalidId && seen.Add(invalidId))
                    {
                        result += invalidId;
                    }
                }
            }
        }

        return result.ToString();
    }
}