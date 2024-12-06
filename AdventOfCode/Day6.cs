namespace AdventOfCode;

public sealed class Day6 : IDay
{
    public string SolvePartOne() => PatrolLab(FindGuard()).Count.ToString();

    public string SolvePartTwo()
    {
        var initialPosition = FindGuard();
        var patrolledPositions = PatrolLab(initialPosition);
        patrolledPositions.Remove(initialPosition);

        return patrolledPositions.Count(position => IsStuckInALoop(initialPosition, position)).ToString();
    }

    private HashSet<Position> PatrolLab(Position position)
    {
        var currentPosition = position;
        var currentDirection = Direction.North;
        var patrolledPositions = new HashSet<Position>();
        while (InBounds(currentPosition))
        {
            var nextPosition = new Position(
                currentPosition.Row + currentDirection.Row,
                currentPosition.Column + currentDirection.Column);
            patrolledPositions.Add(currentPosition);
            if (InBounds(nextPosition))
            {
                if (_grid[nextPosition.Row][nextPosition.Column] == '#')
                {
                    currentDirection = currentDirection.RotateNinetyDegreesRight();
                    continue;
                }
            }

            currentPosition = nextPosition;
        }

        return patrolledPositions;
    }

    private bool IsStuckInALoop(Position guardPosition, Position obstacle)
    {
        _grid[obstacle.Row][obstacle.Column] = '#';

        var currentPosition = guardPosition;
        var currentDirection = Direction.North;
        var count = 0;
        var isInfinite = false;
        while (InBounds(currentPosition))
        {
            if (count > _grid.Length * _grid[0].Length)
            {
                isInfinite = true;
                break;
            }
            var nextPosition = new Position(
                currentPosition.Row + currentDirection.Row,
                currentPosition.Column + currentDirection.Column);
            count++;
            if (InBounds(nextPosition))
            {
                if (_grid[nextPosition.Row][nextPosition.Column] == '#')
                {
                    currentDirection = currentDirection.RotateNinetyDegreesRight();
                    continue;
                }
            }

            currentPosition = nextPosition;
        }

        _grid[obstacle.Row][obstacle.Column] = '.';
        return isInfinite;
    }
    
    private sealed record Direction
    {
        public int Row { get; }
        public int Column { get; }

        private Direction(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public static Direction North => new(-1, 0);

        /*
         * Just the product of a rotation matrix of angle -90° and the current direction vector.
         */
        public Direction RotateNinetyDegreesRight() => new(Column, -Row);
    }

    private sealed record Position(int Row, int Column);

    private Position FindGuard()
    {
        for (var i = 0; i < _grid.Length; i++)
        {
            for (var j = 0; j < _grid[0].Length; j++)
            {
                if (_grid[i][j] == '^')
                {
                    return new Position(i, j);
                }
            }
        }

        throw new InvalidOperationException();
    }

    private bool InBounds(Position position) => position.Row >= 0 && position.Row < _grid.Length &&
                                                position.Column >= 0 && position.Column < _grid[0].Length;
    
    private readonly char[][] _grid = _gridInput.Select(s => s.ToCharArray()).ToArray();
    
    private static readonly string[] _gridInput =
    [
        "....#.....",
        ".........#",
        "..........",
        "..#.......",
        ".......#..",
        "..........",
        ".#..^.....",
        "........#.",
        "#.........",
        "......#..."
    ];
}