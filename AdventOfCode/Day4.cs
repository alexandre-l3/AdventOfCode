namespace AdventOfCode;

public sealed class Day4 : IDay
{
    public string SolvePartOne()
    {
        var n = _words.Length;
        var m = _words[0].Length;
        var result = 0;

        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < m; j++)
            {
                if (_words[i][j] != 'X')
                {
                    continue;
                }
                foreach (var direction in _allDirections)
                {
                    var nextRows = Enumerable.Range(1, 3).Select(x => i + x * direction[0]).ToArray();
                    var nextColumns = Enumerable.Range(1, 3).Select(x => j + x * direction[1]).ToArray();
                    if (!nextRows.All(x => x >= 0 && x < n) || !nextColumns.All(x => x >= 0 && x < m))
                    {
                        continue;
                    }
                    if (_words[nextRows[0]][nextColumns[0]] == 'M' &&
                        _words[nextRows[1]][nextColumns[1]] == 'A' &&
                        _words[nextRows[2]][nextColumns[2]] == 'S')
                    {
                        result++;
                    }
                }
            }
        }

        return result.ToString();
    }

    public string SolvePartTwo()
    {
        var words = _words;
        var n = words.Length;
        var m = words[0].Length;
        var result = 0;

        for (var i = 1; i < n - 1; i++)
        {
            for (var j = 1; j < m - 1; j++)
            {
                if (words[i][j] != 'A')
                {
                    continue;
                }
                var topLeft = words[i - 1][j - 1];
                var bottomRight = words[i + 1][j + 1];
                var topRight = words[i - 1][j + 1];
                var bottomLeft = words[i + 1][j - 1];

                if ($"{topLeft}A{bottomRight}" is "MAS" or "SAM" && $"{topRight}A{bottomLeft}" is "MAS" or "SAM")
                {
                    result++;
                }
            }
        }

        return result.ToString();
    }

    private readonly int[][] _allDirections =
    [
        [1, 0],
        [-1, 0],
        [0, -1],
        [0, 1],
        [1, 1],
        [-1, 1],
        [1, -1],
        [-1, -1]
    ];

    private readonly string[] _words =
    [
        "MMMSXXMASM",
        "MSAMXMSMSA",
        "AMXSXMAAMM",
        "MSAMASMSMX",
        "XMASAMXAMM",
        "XXAMMXXAMA",
        "SMSMSASXSS",
        "SAXAMASAAA",
        "MAMMMXMMMM",
        "MXMXAXMASX"
    ];
}