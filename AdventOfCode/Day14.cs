using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public sealed partial class Day14 : IDay
{
    [GeneratedRegex("[-]{0,1}\\d+")]
    private static partial Regex NumberRegex();

    private const int GridWidth = 11;
    private const int GridHeight = 7;
    
    //private const int GridWidth = 101;
    //private const int GridHeight = 103;
    public string SolvePartOne()
    {
        var quadrants = new int[4];
        foreach (var robot in _robots)
        {
            var values = NumberRegex().Matches(robot)
                .Select(m => m.Value)
                .Select(int.Parse)
                .ToArray();
            var position = values[0] + Complex.ImaginaryOne * values[1];
            var velocity = values[2] + Complex.ImaginaryOne * values[3];

            var finalPosition = position + 100 * velocity;
            var x = Modulo(finalPosition.Real, GridWidth);
            var y = Modulo(finalPosition.Imaginary, GridHeight);

            switch (y)
            {
                case >= 0 and < GridHeight / 2 when x is >= 0 and < GridWidth / 2:
                    quadrants[0]++;
                    break;
                case >= GridHeight / 2 + 1 when x is >= 0 and < GridWidth /2:
                    quadrants[1]++;
                    break;
                case >= GridHeight / 2 + 1 when x >= GridWidth / 2 + 1:
                    quadrants[2]++;
                    break;
                case >= 0 and < GridHeight / 2 when x >= GridWidth / 2 + 1:
                    quadrants[3]++;
                    break;
            }
        }

        return quadrants.Aggregate(1, (x, y) => x * y, z => z.ToString());
    }

    public string SolvePartTwo()
    {
        var iteration = 0;
        var movements = new List<(Complex Position, Complex Velocity)>();
        foreach (var robot in _robots)
        {
            var values = NumberRegex().Matches(robot)
                .Select(m => m.Value)
                .Select(int.Parse)
                .ToArray();
            var position = values[0] + Complex.ImaginaryOne * values[1];
            var velocity = values[2] + Complex.ImaginaryOne * values[3];
            movements.Add((position, velocity));
        }

        while (true)
        {
            iteration++;
            var distinctPositions = movements.Select(m => m.Position + iteration * m.Velocity)
                .Select(z => Modulo(z.Real, GridWidth) + Complex.ImaginaryOne * Modulo(z.Imaginary, GridHeight))
                .ToHashSet()
                .Count;

            if (distinctPositions != _robots.Length)
            {
                continue;
            }
            break;
        }

        return iteration.ToString();
    }

    private static int Modulo(double value, int modulus)
    {
        var result = value % modulus;
        return (int)(result < 0 ? result + modulus : result);
    }

    private readonly string[] _robots =
    [
        "p=0,4 v=3,-3",
        "p=6,3 v=-1,-3",
        "p=10,3 v=-1,2",
        "p=2,0 v=2,-1",
        "p=0,0 v=1,3",
        "p=3,0 v=-2,-2",
        "p=7,6 v=-1,-3",
        "p=3,0 v=-1,-2",
        "p=9,3 v=2,3",
        "p=7,3 v=-1,2",
        "p=2,4 v=2,-3",
        "p=9,5 v=-3,-3"
    ];
}