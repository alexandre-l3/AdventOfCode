namespace AdventOfCode;

public sealed class Day2 : IDay
{
    public string SolvePartOne() => _reports.Select(report => report.Split(' ').Select(int.Parse).ToArray())
        .Count(IsSafeSequence)
        .ToString();

    public string SolvePartTwo()
    {
        var result = 0;
        foreach (var report in _reports)
        {
            var sequence = report.Split(' ').Select(int.Parse).ToList();
            if (IsSafeSequence(sequence))
            {
                result++;
                continue;
            }

            for (var i = 0; i < sequence.Count; i++)
            {
                var current = sequence[i];
                sequence.RemoveAt(i);
                if (IsSafeSequence(sequence))
                {
                    result++;
                    break;
                }
                sequence.Insert(i, current);
            }
        }

        return result.ToString();
    }

    private static bool IsSafeSequence(IList<int> sequence)
    {
        var isSafe = true;
        var previousDiff = 0;
        for (var i = 1; i < sequence.Count; i++)
        {
            var diff = sequence[i] - sequence[i - 1];
            if (previousDiff != 0 && Math.Sign(previousDiff) != Math.Sign(diff))
            {
                isSafe = false;
                break;
            }

            previousDiff = diff;

            if (Math.Abs(diff) is 0 or > 3)
            {
                isSafe = false;
            }
        }

        return isSafe;
    }

    private readonly IReadOnlyList<string> _reports =
    [
        "7 6 4 2 1",
        "1 2 7 8 9",
        "9 7 6 2 1",
        "1 3 2 4 5",
        "8 6 4 4 1",
        "1 3 6 7 9"
    ];
}
