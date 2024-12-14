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
            var x = (int)(finalPosition.Real % GridWidth < 0
                ? (finalPosition.Real % GridWidth) + GridWidth
                : finalPosition.Real % GridWidth);
            var y = (int)(finalPosition.Imaginary % GridHeight < 0
                ? (finalPosition.Imaginary % GridHeight) + GridHeight
                : finalPosition.Imaginary % GridHeight);

            if (y is >= 0 and < GridHeight / 2 && x is >= 0 and < GridWidth / 2)
            {
                quadrants[0]++;
            }
            else if (y >= (GridHeight / 2) + 1 && x is >= 0 and < GridWidth /2)
            {
                quadrants[1]++;
            }
            else if (y >= (GridHeight / 2) + 1 && x >= (GridWidth / 2) + 1)
            {
                quadrants[2]++;
            }
            else if (y is >= 0 and < GridHeight / 2 && x >= (GridWidth / 2) + 1)
            {
                quadrants[3]++;
            }
        }

        return quadrants.Aggregate(1, (x, y) => x * y, z => z.ToString());
    }

    public string SolvePartTwo()
    {
        var iteration = 0;
        while (true)
        {
            iteration++;
            var positions = new HashSet<Complex>();
            var count = 0;
                    
            foreach (var robot in _robots)
            {
                var values = NumberRegex().Matches(robot)
                    .Select(m => m.Value)
                    .Select(int.Parse)
                    .ToArray();
                var position = values[0] + Complex.ImaginaryOne * values[1];
                var velocity = values[2] + Complex.ImaginaryOne * values[3];
    
                var finalPosition = position + iteration * velocity;
                var x = (int)(finalPosition.Real % GridWidth < 0
                    ? (finalPosition.Real % GridWidth) + GridWidth
                    : finalPosition.Real % GridWidth);
                var y = (int)(finalPosition.Imaginary % GridHeight < 0
                    ? (finalPosition.Imaginary % GridHeight) + GridHeight
                    : finalPosition.Imaginary % GridHeight);
                positions.Add(x + Complex.ImaginaryOne * y);
                count++;
            }

            if (positions.Count != count)
            {
                continue;
            }
            break;
        }
        return iteration.ToString();
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