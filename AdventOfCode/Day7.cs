namespace AdventOfCode;

public sealed class Day7 : IDay
{
    public string SolvePartOne()
    {
        var results = 0L;
        foreach (var calibrationEquation in _calibrationEquations)
        {
            var equation = calibrationEquation.Split(':');
            var testValue = long.Parse(equation[0]);
            var rightTerms = equation[1].Trim().Split(' ').Select(long.Parse).ToArray();
            var permutations = GetPermutations(rightTerms.Length - 1, ['+', '*']);
            var isCorrectlyCalibrated = false;
            foreach (var permutation in permutations)
            {
                var calculation = rightTerms[0];
                for (var i = 0; i < permutation.Count; i++)
                {
                    calculation = permutation[i] switch
                    {
                        '+' => calculation + rightTerms[i + 1],
                        '*' => calculation * rightTerms[i + 1],
                        _ => throw new ArgumentOutOfRangeException()
                    };
                }
                if (calculation == testValue)
                {
                    isCorrectlyCalibrated = true;
                    break;
                }
            }

            if (isCorrectlyCalibrated)
            {
                results += testValue;
            }
        }

        return results.ToString();
    }

    public string SolvePartTwo()
    {
        var results = 0L;
        foreach (var calibrationEquation in _calibrationEquations)
        {
            var equation = calibrationEquation.Split(':');
            var testValue = long.Parse(equation[0]);
            var rightTerms = equation[1].Trim().Split(' ').Select(long.Parse).ToArray();
            var permutations = GetPermutations(rightTerms.Length - 1, ['|', '*', '+']);
            var isCorrectlyCalibrated = false;
            foreach (var permutation in permutations)
            {
                var calculation = rightTerms[0];
                for (var i = 0; i < permutation.Count; i++)
                {
                    calculation = permutation[i] switch
                    {
                        '+' => calculation + rightTerms[i + 1],
                        '*' => calculation * rightTerms[i + 1],
                        '|' => long.Parse($"{calculation}{rightTerms[i + 1]}"),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                }
                if (calculation == testValue)
                {
                    isCorrectlyCalibrated = true;
                    break;
                }
            }

            if (isCorrectlyCalibrated)
            {
                results += testValue;
            }
        }

        return results.ToString();
    }

    private static List<List<char>> GetPermutations(int n, char[] operations)
    {
        var results = new List<List<char>>();
        Backtrack(operations, results, [], 0, n);
        return results;
    }

    private static void Backtrack(char[] operations,
        List<List<char>> results,
        List<char> candidate,
        int currentIndex,
        int target)
    {
        if (candidate.Count == target)
        {
            results.Add([..candidate]);
            return;
        }

        foreach (var operation in operations)
        {
            candidate.Add(operation);
            Backtrack(operations, results, candidate, currentIndex + 1, target);
            candidate.RemoveAt(currentIndex);
        }
    }

    private readonly string[] _calibrationEquations =
    [
        "190: 10 19",
        "3267: 81 40 27",
        "83: 17 5",
        "156: 15 6",
        "7290: 6 8 6 15",
        "161011: 16 10 13",
        "192: 17 8 14",
        "21037: 9 7 18 13",
        "292: 11 6 16 20"
    ];
}