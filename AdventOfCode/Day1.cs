namespace AdventOfCode;

public sealed class Day1 : IDay
{
    public string SolveFirst()
    {
        var left = _locations.Select(l => l.LeftId).OrderBy(x => x);
        var right = _locations.Select(l => l.RightId).OrderBy(x => x);

        return left.Zip(right)
            .Sum(s => Math.Abs(s.First - s.Second))
            .ToString();
    }

    public string SolveSecond()
    {
        var frequencies = _locations.Select(l => l.RightId).GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

        return _locations.Select(l => l.LeftId)
            .Where(l => frequencies.ContainsKey(l))
            .Sum(l => l * frequencies[l])
            .ToString();
    }

    private sealed record Location(int LeftId, int RightId);

    private readonly IReadOnlyList<Location> _locations =
    [
        new(3, 4),
        new(4, 3),
        new(2, 5),
        new(1, 3),
        new(3, 9),
        new(3, 3)
    ];
}