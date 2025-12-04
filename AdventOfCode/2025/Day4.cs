namespace AdventOfCode._2025;

public sealed class Day4 : IDay
{
    public string SolvePartOne()
    {
        var grid = File.ReadAllLines("2025/input1.txt")
            .Select(x => x.ToCharArray())
            .ToArray();

        var directions = new int[][]
        {
            [0, 1],
            [1, 0],
            [-1, 0],
            [0, -1],
            [-1, 1],
            [1, -1],
            [-1, -1],
            [1, 1]
        };

        var result = 0;
        for (var i = 0; i < grid.Length; i++)
        {
            for (var j = 0; j < grid[0].Length; j++)
            {
                if (grid[i][j] != '@')
                {
                    continue;
                }
                var adjacentRolls = 0;
                foreach (var direction in directions)
                {
                    var nr = i + direction[0];
                    var nc = j + direction[1];
                    if (nr >= 0 && nr < grid.Length && nc >= 0 && nc < grid[0].Length)
                    {
                        if (grid[nr][nc] == '@')
                        {
                            adjacentRolls++;
                        }
                    }
                }

                if (adjacentRolls > 3)
                {
                    continue;
                }

                result++;
            }
        }

        return result.ToString();
    }

    public string SolvePartTwo()
    {
        var grid = File.ReadAllLines("2025/input1.txt")
            .Select(x => x.ToCharArray())
            .ToArray();

        var directions = new int[][]
        {
            [0, 1],
            [1, 0],
            [-1, 0],
            [0, -1],
            [-1, 1],
            [1, -1],
            [-1, -1],
            [1, 1]
        };

        var result = 0;
        
        bool canRemove;
        do
        {
            canRemove = false;
            for (var i = 0; i < grid.Length; i++)
            {
                for (var j = 0; j < grid[0].Length; j++)
                {
                    if (grid[i][j] != '@')
                    {
                        continue;
                    }

                    var adjacentRolls = 0;
                    foreach (var direction in directions)
                    {
                        var nr = i + direction[0];
                        var nc = j + direction[1];
                        if (nr >= 0 && nr < grid.Length && nc >= 0 && nc < grid[0].Length)
                        {
                            if (grid[nr][nc] == '@')
                            {
                                adjacentRolls++;
                            }
                        }
                    }

                    if (adjacentRolls > 3)
                    {
                        continue;
                    }

                    canRemove = true;
                    grid[i][j] = 'X';
                    result++;
                }
            }

        } while (canRemove);

        return result.ToString();
    }
}