using System.Numerics;

namespace AdventOfCode;

public sealed class Day10 : IDay
{
    public string SolvePartOne()
    {
        var score = 0;
        for (var i = 0; i < _grid.Length; i++)
        {
            for (var j = 0; j < _grid[i].Length; j++)
            {
                if (_grid[i][j] == '0')
                {
                    score += CalculateScore(i + Complex.ImaginaryOne * j);
                }
            }
        }

        return score.ToString();
    }

    public string SolvePartTwo()
    {
        var ratings = 0;
        for (var i = 0; i < _grid.Length; i++)
        {
            for (var j = 0; j < _grid[i].Length; j++)
            {
                if (_grid[i][j] == '0')
                {
                    ratings += CalculateRatings(i + Complex.ImaginaryOne * j);
                }
            }
        }

        return ratings.ToString();
    }

    private int CalculateScore(Complex start)
    {
        var queue = new Queue<Complex>([start]);
        var highestPositions = 0;
        var visited = new HashSet<Complex>();

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            var position = _grid[(int)node.Real][(int)node.Imaginary] - '0';
            if (position == 9)
            {
                highestPositions++;
            }

            foreach (var direction in _direction)
            {
                var next = node + direction;
                if (OutOfBounds(next))
                {
                    continue;
                }
                if (_grid[(int)next.Real][(int)next.Imaginary] - '0' != position + 1)
                {
                    continue;
                }

                if (visited.Add(next))
                {
                    queue.Enqueue(next);
                }
            }
        }

        return highestPositions;
    }

    private int CalculateRatings(Complex start)
    {
        var queue = new Queue<Complex>([start]);
        var ratings = 0;

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            var position = _grid[(int)node.Real][(int)node.Imaginary] - '0';
            if (position == 9)
            {
                ratings++;
            }

            foreach (var direction in _direction)
            {
                var next = node + direction;
                if (OutOfBounds(next))
                {
                    continue;
                }
                if (_grid[(int)next.Real][(int)next.Imaginary] - '0' != position + 1)
                {
                    continue;
                }
                queue.Enqueue(next);
            }
        }

        return ratings;
    }

    private bool OutOfBounds(Complex z) => z.Real < 0 || z.Real >= _grid.Length ||
                                           z.Imaginary < 0 || z.Imaginary >= _grid[0].Length;

    private readonly Complex[] _direction = [Complex.One, Complex.ImaginaryOne, -Complex.One, -Complex.ImaginaryOne];

    private readonly string[] _grid =
    [
        "89010123",
        "78121874",
        "87430965",
        "96549874",
        "45678903",
        "32019012",
        "01329801",
        "10456732"
    ];
}