using System.Numerics;
using System.Text;

namespace AdventOfCode;

public sealed class Day15 : IDay
{
    public string SolvePartOne()
    {
        var position = FindRobot(_warehouse);

        foreach (var movement in Movements)
        {
            var direction = GetDirection(movement);
            if (direction == Complex.Zero)
            {
                continue;
            }
            var next = position + direction;
            if (!InBounds(next, _warehouse))
            {
                continue;
            }
            var nextItem = _warehouse[(int)next.Real][(int)next.Imaginary];
            if (nextItem == '#')
            {
                continue;
            }

            if (nextItem == 'O')
            {
                var outcome = PushBoxes(next, direction);
                if (!outcome)
                {
                    continue;
                }
            }

            _warehouse[(int)position.Real][(int)position.Imaginary] = '.';
            position = next;
            _warehouse[(int)position.Real][(int)position.Imaginary] = '@';
        }

        return SumGpsCoordinates(_warehouse).ToString();
    }

    public string SolvePartTwo()
    {
        var position = FindRobot(_doubleWarehouse);

        foreach (var movement in Movements)
        {
            var direction = GetDirection(movement);
            if (direction == Complex.Zero)
            {
                continue;
            }
            var next = position + direction;
            if (!InBounds(next, _doubleWarehouse))
            {
                continue;
            }
            var nextItem = _doubleWarehouse[(int)next.Real][(int)next.Imaginary];
            if (nextItem == '#')
            {
                continue;
            }

            if (nextItem is '[' or ']')
            {
                if (IsHorizontal(direction))
                {
                    var outcome = PushBigBoxHorizontally(next, direction);
                    if (!outcome)
                    {
                        continue;
                    }
                }
                else
                {
                    var outcome = PushBigBoxVertically(next, direction);
                    if (!outcome)
                    {
                        continue;
                    }
                }
            }

            _doubleWarehouse[(int)position.Real][(int)position.Imaginary] = '.';
            position = next;
            _doubleWarehouse[(int)position.Real][(int)position.Imaginary] = '@';
        }

        return SumGpsCoordinates(_doubleWarehouse).ToString();
    }

    private static long SumGpsCoordinates(char[][] warehouse)
    {
        var result = 0L;
        for (var i = 0; i < warehouse.Length; i++)
        {
            for (var j = 0; j < warehouse[0].Length; j++)
            {
                if (warehouse[i][j] is 'O' or '[')
                {
                    result += 100 * i + j;
                }
            }
        }

        return result;
    }

    private static bool IsHorizontal(Complex direction) => direction == Complex.ImaginaryOne ||
                                                           direction == -Complex.ImaginaryOne;
    
    private bool PushBigBoxHorizontally(Complex boxPosition, Complex direction)
    {
        var next = boxPosition;
        var emptySpacePosition = Complex.Infinity;
        var boxes = new List<(Complex, char)>();
        while (InBounds(next, _doubleWarehouse))
        {
            if (_doubleWarehouse[(int)next.Real][(int)next.Imaginary] == '#')
            {
                break;
            }

            if (_doubleWarehouse[(int)next.Real][(int)next.Imaginary] == '.')
            {
                emptySpacePosition = next;
                break;
            }

            if (_doubleWarehouse[(int)next.Real][(int)next.Imaginary] is '[' or ']')
            {
                boxes.Add((next, _doubleWarehouse[(int)next.Real][(int)next.Imaginary]));
            }
            next += direction;
        }

        if (emptySpacePosition == Complex.Infinity)
        {
            return false;
        }

        foreach (var (box, value) in boxes)
        {
            _doubleWarehouse[(int)box.Real][(int)(box.Imaginary + direction.Imaginary)] = value;
        }

        return true;
    }

    private bool PushBigBoxVertically(Complex boxPosition, Complex direction)
    {
        var currentPositions = new Dictionary<Complex, char>();
        if (!TraverseConnectedBoxes(boxPosition, direction, currentPositions))
        {
            return false;
        }

        var boxes = currentPositions.Where(i => i.Value is '[' or ']').ToArray();
        foreach (var item in boxes)
        {
            _doubleWarehouse[(int)item.Key.Real][(int)item.Key.Imaginary] = '.';
        }
        foreach (var item in boxes)
        {
            _doubleWarehouse[(int)(item.Key.Real + direction.Real)][(int)item.Key.Imaginary] = item.Value;
        }
        
        return true;
    }

    private static IReadOnlyList<Complex> GetNeighbours(char value, Complex direction)
    {
        return value switch
        {
            '[' => [Complex.ImaginaryOne, direction],
            ']' => [-Complex.ImaginaryOne, direction],
            _ => []
        };
    }

    private bool TraverseConnectedBoxes(Complex root, Complex direction, Dictionary<Complex, char> results)
    {
        var queue = new Queue<Complex>([root]);
        var visited = new HashSet<Complex>();

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            var value = _doubleWarehouse[(int)node.Real][(int)node.Imaginary];
            if (value == '#')
            {
                return false;
            }

            results[node] = value;

            foreach (var dir in GetNeighbours(value, direction))
            {
                var next = node + dir;
                if (visited.Add(next))
                {
                    queue.Enqueue(next);
                }
            }
        }

        return true;
    }

    private bool PushBoxes(Complex boxPosition, Complex direction)
    {
        var next = boxPosition;
        var emptySpacePosition = Complex.Infinity;
        var boxes = new List<Complex>();
        while (InBounds(next, _warehouse))
        {
            if (_warehouse[(int)next.Real][(int)next.Imaginary] == '#')
            {
                break;
            }
            if (_warehouse[(int)next.Real][(int)next.Imaginary] == '.')
            {
                emptySpacePosition = next;
                break;
            }

            if (_warehouse[(int)next.Real][(int)next.Imaginary] == 'O')
            {
                boxes.Add(next);
            }
            next += direction;
        }

        if (emptySpacePosition == Complex.Infinity)
        {
            return false;
        }

        foreach (var box in boxes)
        {
            _warehouse[(int)(box.Real + direction.Real)][(int)(box.Imaginary + direction.Imaginary)] = 'O';
        }

        return true;
    }

    private static bool InBounds(Complex z, char[][] warehouse) => 
        z.Real >= 1 && z.Real < warehouse.Length - 1 && z.Imaginary >= 1 && z.Imaginary < warehouse[0].Length - 1;

    private static Complex GetDirection(char direction)
    {
        return direction switch
        {
            '<' => -Complex.ImaginaryOne,
            '>' => Complex.ImaginaryOne,
            '^' => -Complex.One,
            'v' => Complex.One,
            _ => Complex.Zero
        };
    }

    private static Complex FindRobot(char[][] warehouse)
    {
        for (var i = 0; i < warehouse.Length; i++)
        {
            for (var j = 0; j < warehouse[0].Length; j++)
            {
                if (warehouse[i][j] == '@')
                {
                    return i + Complex.ImaginaryOne * j;
                }
            }
        }

        throw new InvalidOperationException();
    }

    private readonly char[][] _warehouse = _grid.Select(x => x.ToCharArray()).ToArray();

    private readonly char[][] _doubleWarehouse = _grid
        .Select(x => x.Aggregate(new StringBuilder(), (sb, b) => sb.Append(DoubleItem(b))).ToString().ToCharArray())
        .ToArray();

    private static string DoubleItem(char item)
    {
        return item switch
        {
            '#' => "##",
            'O' => "[]",
            '.' => "..",
            '@' => "@.",
            _ => ""
        };
    }

    private static readonly string[] _grid =
    [
        "##########",
        "#..O..O.O#",
        "#......O.#",
        "#.OO..O.O#",
        "#..O@..O.#",
        "#O#..O...#",
        "#O..O..O.#",
        "#.OO.O.OO#",
        "#....O...#",
        "##########"
    ];

    private const string Movements = "<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^\nvvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v\n><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<\n<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^\n^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><\n^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^\n>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^\n<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>\n^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>\nv^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^";
}