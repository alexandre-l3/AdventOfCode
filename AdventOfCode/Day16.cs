using System.Numerics;

namespace AdventOfCode;

public sealed class Day16 : IDay
{
    private const int TurnCost = 1000;
    private const int MoveCost = 1;

    public string SolvePartOne()
    {
        var start = FindStart();
        var estimatedCost = new Dictionary<(Complex, Complex), int>();
        for (var i = 0; i < _maze.Length; i++)
        {
            for (var j = 0; j < _maze[0].Length; j++)
            {
                foreach (var direction in _directions)
                {
                    estimatedCost.Add((i + Complex.ImaginaryOne * j, direction), int.MaxValue);
                }
            }
        }

        var facing = Complex.ImaginaryOne;
        estimatedCost[(start, facing)] = 0;
        var priorityQueue = new PriorityQueue<(Complex, Complex), int>([((start, facing), 0)]);
        var end = FindEnd();

        while (priorityQueue.Count > 0)
        {
            var (node, orientation) = priorityQueue.Dequeue();
            var next = node + orientation;
            var value = _maze[(int)next.Real][(int)next.Imaginary];
            if (value != '#')
            {
                var translationCost = estimatedCost[(node, orientation)] + MoveCost;
                if (translationCost < estimatedCost[(next, orientation)])
                {
                    estimatedCost[(next, orientation)] = translationCost;
                    priorityQueue.Enqueue((next, orientation), translationCost);
                }
            }

            foreach (var rotation in new[] { -Complex.ImaginaryOne, Complex.ImaginaryOne })
            {
                var rotationCost = estimatedCost[(node, orientation)] + TurnCost;
                var orthogonalDirection= orientation * rotation;
                if (rotationCost < estimatedCost[(node, orthogonalDirection)])
                {
                    estimatedCost[(node, orthogonalDirection)] = rotationCost;
                    priorityQueue.Enqueue((node, orthogonalDirection), rotationCost);
                }
            }
        }

        return _directions.Select(d => estimatedCost[(end, d)]).Min().ToString();
    }

    public string SolvePartTwo()
    {
        var start = FindStart();
        var estimatedCost = new Dictionary<(Complex, Complex), int>();
        var previous = new Dictionary<(Complex, Complex), List<(Complex, Complex)>>();
        for (var i = 0; i < _maze.Length; i++)
        {
            for (var j = 0; j < _maze[0].Length; j++)
            {
                var current = i + Complex.ImaginaryOne * j;
                foreach (var direction in _directions)
                {
                    previous[(current, direction)] = [];
                    estimatedCost.Add((current, direction), int.MaxValue);
                }
            }
        }

        var facing = Complex.ImaginaryOne;
        estimatedCost[(start, facing)] = 0;
        var priorityQueue = new PriorityQueue<(Complex, Complex), int>([((start, facing), 0)]);
        var end = FindEnd();

        while (priorityQueue.Count > 0)
        {
            var (node, orientation) = priorityQueue.Dequeue();
            var next = node + orientation;
            var value = _maze[(int)next.Real][(int)next.Imaginary];
            if (value != '#')
            {
                var cost = estimatedCost[(node, orientation)] + MoveCost;
                if (cost < estimatedCost[(next, orientation)])
                {
                    estimatedCost[(next, orientation)] = cost;
                    priorityQueue.Enqueue((next, orientation), cost);
                    previous[(next, orientation)] = [(node, orientation)];
                }
                else if (cost == estimatedCost[(next, orientation)])
                {
                    previous[(next, orientation)].Add((node, orientation));
                }
            }

            foreach (var rotation in new[] { -Complex.ImaginaryOne, Complex.ImaginaryOne })
            {
                var rotationCost = estimatedCost[(node, orientation)] + TurnCost;
                var orthogonalDirection= orientation * rotation;
                if (rotationCost < estimatedCost[(node, orthogonalDirection)])
                {
                    estimatedCost[(node, orthogonalDirection)] = rotationCost;
                    priorityQueue.Enqueue((node, orthogonalDirection), rotationCost);
                    previous[(node, orthogonalDirection)] = [(node, orientation)];
                }
                else if (rotationCost == estimatedCost[(node, orthogonalDirection)])
                {
                    previous[(node, orthogonalDirection)].Add((node, orientation));
                }
            }
        }

        var minimalDirection = _directions.MinBy(d => estimatedCost[(end, d)]);
        var paths = ReconstructPaths(previous, (end, minimalDirection));

        return paths.Select(p => p)
            .SelectMany(p => p)
            .Select(p => p.Tile)
            .ToHashSet()
            .Count
            .ToString();
    }

    private static List<List<(Complex Tile, Complex)>> ReconstructPaths(
        Dictionary<(Complex, Complex), List<(Complex, Complex)>> previous,
        (Complex, Complex) end)
    {
        var results = new List<List<(Complex, Complex)>>();
        Backtrack(previous, results, [end], end);
        return results;
    }

    private static void Backtrack(
        Dictionary<(Complex, Complex), List<(Complex, Complex)>> previous,
        List<List<(Complex, Complex)>> paths,
        List<(Complex, Complex)> path,
        (Complex, Complex) node)
    {
        if (previous[node].Count == 0)
        {
            paths.Add([..path]);
            return;
        }

        foreach (var predecessor in previous[node])
        {
            path.Add(predecessor);
            Backtrack(previous, paths, path, predecessor);
            path.Remove(predecessor);
        }
    }

    private readonly Complex[] _directions = [-Complex.ImaginaryOne, Complex.ImaginaryOne, -1, 1];

    private Complex FindStart()
    {
        for (var i = 0; i < _maze.Length; i++)
        {
            for (var j = 0; j < _maze[i].Length; j++)
            {
                if (_maze[i][j] == 'S')
                {
                    return new Complex(i, j);
                }
            }
        }

        throw new InvalidOperationException("Start not found");
    }

    private Complex FindEnd()
    {
        for (var i = 0; i < _maze.Length; i++)
        {
            for (var j = 0; j < _maze[i].Length; j++)
            {
                if (_maze[i][j] == 'E')
                {
                    return new Complex(i, j);
                }
            }
        }

        throw new InvalidOperationException("Start not found");
    }

    private readonly char[][] _maze = _grid.Select(row => row.ToCharArray()).ToArray();

    private static readonly string[] _grid =
    [
        "###############",
        "#.......#....E#",
        "#.#.###.#.###.#",
        "#.....#.#...#.#",
        "#.###.#####.#.#",
        "#.#.#.......#.#",
        "#.#.#####.###.#",
        "#...........#.#",
        "###.#.#####.#.#",
        "#...#.....#.#.#",
        "#.#.#.###.#.#.#",
        "#.....#...#.#.#",
        "#.###.#.#.#.#.#",
        "#S..#.....#...#",
        "###############"
    ];
}