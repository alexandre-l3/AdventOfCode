using System.Numerics;

namespace AdventOfCode;

public sealed class Day18(int corruptedBytes) : IDay
{
    public string SolvePartOne()
    {
        var height = (int)_positions.MaxBy(z => z.Real).Real;
        var width = (int)_positions.MaxBy(z => z.Imaginary).Imaginary;

        var grid = new char[height + 1][];
        for (var i = 0; i < height + 1; i++)
        {
            grid[i] = new char[width + 1];
        }

        for (var i = 0; i < height + 1; i++)
        {
            for (var j = 0; j < width + 1; j++)
            {
                grid[i][j] = '.';
            }
        }
        foreach (var position in _positions.Take(corruptedBytes))
        {
            grid[(int)position.Real][(int)position.Imaginary] = '#';
        }
        return FindShortestPathLength(height, width, grid).ToString();
    }

    public string SolvePartTwo()
    {
        var height = (int)_positions.MaxBy(z => z.Real).Real;
        var width = (int)_positions.MaxBy(z => z.Imaginary).Imaginary;

        var grid = new char[height + 1][];
        for (var i = 0; i < height + 1; i++)
        {
            grid[i] = new char[width + 1];
        }

        for (var i = 0; i < height + 1; i++)
        {
            for (var j = 0; j < width + 1; j++)
            {
                grid[i][j] = '.';
            }
        }
        foreach (var position in _positions.Take(corruptedBytes))
        {
            grid[(int)position.Real][(int)position.Imaginary] = '#';
        }

        var index = corruptedBytes;
        var currentByte = -Complex.One;
        while (FindShortestPathLength(height, width, grid) != int.MaxValue)
        {
            currentByte = _positions[index];
            grid[(int)currentByte.Real][(int)currentByte.Imaginary] = '#';
            index++;
        }

        return $"{currentByte.Real},{currentByte.Imaginary}";
    }

    private int FindShortestPathLength(int height, int width, char[][] grid)
    {
        var estimatedDistance = new Dictionary<Complex, int>();
        for (var i = 0; i < height + 1; i++)
        {
            for (var j = 0; j < width + 1; j++)
            {
                if (grid[i][j] == '#')
                {
                    continue;
                }
                estimatedDistance[i + Complex.ImaginaryOne * j] = int.MaxValue;
            }
        }

        estimatedDistance[0] = 0;
        var queue = new Queue<Complex>([0]);
        while (queue.TryDequeue(out var node))
        {
            foreach (var direction in _directions)
            {
                var next = node + direction;
                if (OutOfBounds(next, height, width))
                {
                    continue;
                }
                if (grid[(int)next.Real][(int)next.Imaginary] == '#')
                {
                    continue;
                }

                var distance = estimatedDistance[node] + 1;
                if (distance < estimatedDistance[next])
                {
                    estimatedDistance[next] = distance;
                    queue.Enqueue(next);
                }
            }
        }

        return estimatedDistance[height + Complex.ImaginaryOne * width];
    }
    
    private bool OutOfBounds(Complex z, int height, int width) => z.Real < 0 || z.Real >= height + 1 || 
                                                                  z.Imaginary < 0 || z.Imaginary >= width + 1;

    private readonly Complex[] _directions = [-Complex.ImaginaryOne, Complex.ImaginaryOne, -1, 1];

    private readonly Complex[] _positions = _bytePositions.Select(s => s.Split(',').Select(int.Parse).ToArray())
        .Select(z => new Complex(z[0], z[1]))
        .ToArray();

    private static readonly string[] _bytePositions =
    [
        "5,4",
        "4,2",
        "4,5",
        "3,0",
        "2,1",
        "6,3",
        "2,4",
        "1,5",
        "0,6",
        "3,3",
        "2,6",
        "5,1",
        "1,2",
        "5,5",
        "2,5",
        "6,5",
        "1,4",
        "0,4",
        "6,4",
        "1,1",
        "6,1",
        "1,0",
        "0,5",
        "1,6",
        "2,0"
    ];
}