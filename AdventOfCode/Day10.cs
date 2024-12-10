using System.Numerics;

namespace AdventOfCode;

public sealed class Day10 : IDay
{
    public string SolvePartOne()
    {
        var result = 0;
        for (var i = 0; i < _grid.Length; i++)
        {
            for (var j = 0; j < _grid[i].Length; j++)
            {
                if (_grid[i][j] == '0')
                {
                    result += Search(new Complex(i, j));
                }
            }
        }

        return result.ToString();
    }

    private int Search(Complex start)
    {
        var queue = new Queue<Complex>([start]);
        var highestPositions = 0;
        var visited = new HashSet<Complex>();

        while (queue.Any())
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
                if (next.Real < 0 || next.Real >= _grid.Length || next.Imaginary < 0 || next.Imaginary >= _grid[0].Length)
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

    private int SearchRating(Complex start)
    {
        var queue = new Queue<Complex>([start]);
        var lowestPositions = 0;

        while (queue.Any())
        {
            var node = queue.Dequeue();
            var position = _grid[(int)node.Real][(int)node.Imaginary] - '0';
            if (position == 0)
            {
                lowestPositions++;
            }

            foreach (var direction in _direction)
            {
                var next = node + direction;
                if (next.Real < 0 || next.Real >= _grid.Length || next.Imaginary < 0 || next.Imaginary >= _grid[0].Length)
                {
                    continue;
                }
                if (_grid[(int)next.Real][(int)next.Imaginary] - '0' != position - 1)
                {
                    continue;
                }
                queue.Enqueue(next);
            }
        }

        return lowestPositions;
    }

    public string SolvePartTwo()
    {
        var result = 0;
        for (var i = 0; i < _grid.Length; i++)
        {
            for (var j = 0; j < _grid[i].Length; j++)
            {
                if (_grid[i][j] == '9')
                {
                    result += SearchRating(new Complex(i, j));
                }
            }
        }

        return result.ToString();
    }

    private readonly Complex[] _direction = [new(1, 0), new(0, 1), new(-1, 0), new(0, -1)];

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