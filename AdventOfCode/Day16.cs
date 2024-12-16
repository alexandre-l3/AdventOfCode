using System.Numerics;

namespace AdventOfCode;

public sealed class Day16 : IDay
{
    public string SolvePartOne()
    {
        var start = FindStart();
        var estimatedDistances = new Dictionary<Complex, int>();
        var previous = new Dictionary<Complex, Complex?>();
        for (var i = 0; i < _maze.Length; i++)
        {
            for (var j = 0; j < _maze[0].Length; j++)
            {
                estimatedDistances.Add(i + Complex.ImaginaryOne * j, int.MaxValue);
            }
        }
        estimatedDistances[start] = 0;
        var priorityQueue = new PriorityQueue<(Complex, Complex), int>();
        var facing = Complex.ImaginaryOne;
        priorityQueue.Enqueue((start, facing), 0);
        previous[start] = null;
        var end = FindEnd();

        while (priorityQueue.Count > 0)
        {
            var (node, orientation) = priorityQueue.Dequeue();

            foreach (var direction in new [] {orientation, orientation * Complex.ImaginaryOne, orientation * (-Complex.ImaginaryOne)})
            {
                var next = node + direction;
                Console.WriteLine(next);
                var value = _maze[(int)next.Real][(int)next.Imaginary];
                if (value == '#')
                {
                    continue;
                }

                var distance = estimatedDistances[node];
                if (direction == orientation)
                {
                    distance += 1;
                }
                else
                {
                    distance += 1000;
                }

                if (distance < estimatedDistances[next])
                {
                    estimatedDistances[next] = distance;
                    priorityQueue.Enqueue((next, direction), distance);
                    previous[next] = node;
                }
            }
        }

        Console.WriteLine(estimatedDistances[end]);
        var current = end;
        var directionChange = 0;
        var count = 0;
        var currentDirection = current - previous[current];
        while (previous[current] != null)
        {
            _maze[(int)current.Real][(int)current.Imaginary] = 'X';
            Console.WriteLine(current);
            if (current - previous[current] != currentDirection)
            {
                directionChange++;
            }
            else
            {
                count++;
            }

            currentDirection = current - previous[current];

            //Console.WriteLine($"Count = {count} | Direction changes = {directionChange}");
            current = previous[current].Value;
        }

        for (var i = 0; i < _maze.Length; i++)
        {
            for (var j = 0; j < _maze[0].Length; j++)
            {
                if (_maze[i][j] == 'X')
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.Write(_maze[i][j]);
                Console.ResetColor();
            }
            Console.Write('\n');
        }

        return (count + directionChange + (directionChange + 1) * 1000).ToString();
    }

    public string SolvePartTwo()
    {
        throw new NotImplementedException();
    }

    private Complex[] _directions = [-Complex.ImaginaryOne, Complex.ImaginaryOne, -1, 1];

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

    private static readonly string[] _grid1 =
    [
        "#################",
        "#...#...#...#..E#",
        "#.#.#.#.#.#.#.#.#",
        "#.#.#.#...#...#.#",
        "#.#.#.#.###.#.#.#",
        "#...#.#.#.....#.#",
        "#.#.#.#.#.#####.#",
        "#.#...#.#.#.....#",
        "#.#.#####.#.###.#",
        "#.#.#.......#...#",
        "#.#.###.#####.###",
        "#.#.#...#.....#.#",
        "#.#.#.#####.###.#",
        "#.#.#.........#.#",
        "#.#.#.#########.#",
        "#S#.............#",
        "#################"
    ];
}