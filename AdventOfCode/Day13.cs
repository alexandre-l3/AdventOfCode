using System.Text.RegularExpressions;

namespace AdventOfCode;

public sealed class Day13 : IDay
{
    public string SolvePartOne()
    {
        var pattern = "\\d+";

        var result = 0L;
        var line = 0;
        var pairs = new List<Pair>();
        foreach (var input in _inputs)
        {
            var matches = Regex.Matches(input, pattern)
                .Select(m => int.Parse(m.Value))
                .ToArray();
            pairs.Add(new Pair(matches[0], matches[1]));
            line++;
            if (line == 3)
            {
                result += CalculateCost(pairs[0], pairs[1], pairs[2]);
                line = 0;
                pairs.Clear();
            }
        }

        return result.ToString();
    }

    public string SolvePartTwo()
    {
        var pattern = "\\d+";

        var result = 0L;
        var line = 0;
        var pairs = new List<Pair>();
        foreach (var input in _inputs)
        {
            var matches = Regex.Matches(input, pattern)
                .Select(m => int.Parse(m.Value))
                .ToArray();
            pairs.Add(new Pair(matches[0], matches[1]));
            line++;
            if (line == 3)
            {
                var correctedTarget = new Pair(CorrectiveTerm + pairs[2].X, CorrectiveTerm + pairs[2].Y);
                result += CalculateCostV2(pairs[0], pairs[1], correctedTarget);
                line = 0;
                pairs.Clear();
            }
        }

        return result.ToString();
    }

    private const long CorrectiveTerm = 10000000000000L;

    private sealed record Pair(long X, long Y);

    private static long CalculateCost(Pair a, Pair b, Pair target)
    {
        var determinant = a.X * b.Y - b.X * a.Y;
        if (determinant == 0)
        {
            return 0L;
        }

        var aPresses = b.Y * target.X - b.X * target.Y;
        var bPresses = -a.Y * target.X + a.X * target.Y;
        if (aPresses % determinant == 0 && bPresses % determinant == 0)
        {
            aPresses /= determinant;
            bPresses /= determinant;

            if (aPresses is > 0 and < 100 && bPresses is > 0 and < 100)
            {
                return 3 * aPresses + bPresses;
            }
        }

        return 0L;
    }

    private static long CalculateCostV2(Pair a, Pair b, Pair target)
    {
        var determinant = a.X * b.Y - b.X * a.Y;
        if (determinant == 0)
        {
            return 0L;
        }

        var aPresses = b.Y * target.X - b.X * target.Y;
        var bPresses = -a.Y * target.X + a.X * target.Y;
        if (aPresses % determinant == 0 && bPresses % determinant == 0)
        {
            aPresses /= determinant;
            bPresses /= determinant;

            return 3 * aPresses + bPresses;
        }

        return 0L;
    }

    private readonly string[] _inputs =
    [
        "Button A: X+94, Y+34",
        "Button B: X+22, Y+67",
        "Prize: X=8400, Y=5400",
        "Button A: X+26, Y+66",
        "Button B: X+67, Y+21",
        "Prize: X=12748, Y=12176",
        "Button A: X+17, Y+86",
        "Button B: X+84, Y+37",
        "Prize: X=7870, Y=6450",
        "Button A: X+69, Y+23",
        "Button B: X+27, Y+71",
        "Prize: X=18641, Y=10279"
    ];
}