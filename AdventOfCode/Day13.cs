using System.Text.RegularExpressions;

namespace AdventOfCode;

public sealed partial class Day13 : IDay
{
    private const long CorrectiveTerm = 10000000000000L;

    [GeneratedRegex("\\d+")]
    private static partial Regex NumberRegex();

    public string SolvePartOne() => Solve(CalculateCost).ToString();

    public string SolvePartTwo() => Solve((a, b, target) =>
        {
            var correctedTarget = new Pair(CorrectiveTerm + target.X, CorrectiveTerm + target.Y);
            return CalculateCost(a, b, correctedTarget, _ => true);
        }).ToString();

    private long Solve(Func<Pair, Pair, Pair, long> costCalculator) => _inputs
        .Select(input => NumberRegex().Matches(input).Select(m => int.Parse(m.Value)).ToArray())
        .Select(matches => new Pair(matches[0], matches[1]))
        .Chunk(3)
        .Sum(chunk => costCalculator(chunk[0], chunk[1], chunk[2]));

    private static long CalculateCost(Pair a, Pair b, Pair target)
    {
        return CalculateCost(a, b, target, p => p.aPresses is > 0 and < 100 && p.bPresses is > 0 and < 100);
    }

    private static long CalculateCost(Pair a, Pair b, Pair target, Func<(long aPresses, long bPresses), bool> validate)
    {
        var determinant = a.X * b.Y - b.X * a.Y;
        if (determinant == 0L)
        {
            return 0L;
        }

        var aPresses = Math.DivRem(b.Y * target.X - b.X * target.Y, determinant, out var remainderA);
        var bPresses = Math.DivRem(-a.Y * target.X + a.X * target.Y, determinant, out var remainderB);
        if (remainderA != 0 || remainderB != 0)
        {
            return 0L;
        }
        if (validate((aPresses, bPresses)))
        {
            return 3 * aPresses + bPresses;
        }

        return 0L;
    }

    private sealed record Pair(long X, long Y);

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