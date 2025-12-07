namespace AdventOfCode._2025;

public sealed class Day7 : IDay
{
    public string SolvePartOne()
    {
        var grid = File.ReadAllLines("2025/input1.txt")
            .Select(x => x.ToCharArray())
            .ToArray();

        ConstructPaths(grid);

        var result = 0;
        for (var i = 1; i < grid.Length; i++)
        {
            for (var j = 0; j < grid[0].Length; j++)
            {
                if (grid[i][j] != '^')
                {
                    continue;
                }

                if (grid[i - 1][j] == '|')
                {
                    result++;
                }
            }
        }

        return result.ToString();
    }

    public string SolvePartTwo()
    {
        var grid = File.ReadAllLines("2025/input1.txt")
            .Select(x => x.ToCharArray())
            .ToArray();

        ConstructPaths(grid);

        var (startRow, startColumn) = GetSLocation(grid);
        var cache = new Dictionary<(int, int), long>
        {
            [(startRow, startColumn)] = 1
        };

        for (var i = 0; i < grid.Length; i++)
        {
            for (var j = 0; j < grid[0].Length; j++)
            {
                if (grid[i][j] != '|')
                {
                    continue;
                }
                cache[(i, j)] = cache.GetValueOrDefault((i - 1, j), 0);
                if (j - 1 >= 0 && grid[i][j - 1] == '^')
                {
                    cache[(i, j)] += cache.GetValueOrDefault((i - 1, j - 1), 0);
                }

                if (j + 1 < grid[0].Length && grid[i][j + 1] == '^')
                {
                    cache[(i, j)] += cache.GetValueOrDefault((i - 1, j + 1), 0);
                }
            }
        }

        return Enumerable.Range(0, grid.Length)
            .Sum(x => cache.GetValueOrDefault((grid.Length - 1, x), 0L))
            .ToString();
    }

    private static void ConstructPaths(char[][] grid)
    {
        var queue = new Queue<(int, int)>();
        var (startRow, startColumn) = GetSLocation(grid);

        queue.Enqueue((startRow, startColumn));
        var seen = new HashSet<(int, int)>();

        while (queue.Count > 0)
        {
            var (currentRow, currentColumn) = queue.Dequeue();

            if (grid[currentRow][currentColumn] == '^')
            {
                if (currentColumn - 1 >= 0 && currentColumn + 1 < grid[0].Length)
                {
                    grid[currentRow][currentColumn - 1] = '|';
                    grid[currentRow][currentColumn + 1] = '|';
                    if (seen.Add((currentRow, currentColumn - 1)))
                    {
                        queue.Enqueue((currentRow, currentColumn - 1));
                    }

                    if (seen.Add((currentRow, currentColumn + 1)))
                    {
                        queue.Enqueue((currentRow, currentColumn + 1));
                    }
                }
            }
            else if (grid[currentRow][currentColumn] == '|' || grid[currentRow][currentColumn] == 'S')
            {
                if (currentRow + 1 >= grid.Length)
                {
                    continue;
                }
                if (grid[currentRow + 1][currentColumn] == '.')
                {
                    grid[currentRow + 1][currentColumn] = '|';
                }
                if (seen.Add((currentRow + 1, currentColumn)))
                {
                    queue.Enqueue((currentRow + 1, currentColumn));
                }
            }
        }
    }

    private static (int, int) GetSLocation(char[][] grid)
    {
        for (var i = 0; i < grid.Length; i++)
        {
            for (var j = 0; j < grid[0].Length; j++)
            {
                if (grid[i][j] == 'S')
                {
                    return (i, j);
                }
            }
        }

        throw new InvalidOperationException();
    }
}