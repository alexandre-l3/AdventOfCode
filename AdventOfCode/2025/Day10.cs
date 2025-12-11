using Google.OrTools.Sat;

namespace AdventOfCode._2025;

public sealed class Day10 : IDay
{
    public string SolvePartOne()
    {
        using var stream = new StreamReader("2025/input1.txt");

        var result = 0;
        while (!stream.EndOfStream)
        {
            var line = stream.ReadLine()!
                .Split(' ');
            var target = line[0][1..^1];

            var buttonPresses = line.Skip(1).SkipLast(1)
                .Select(x => x.Replace("(", "").Replace(")", ""))
                .Select(x => x.Split(',').Select(int.Parse).ToArray())
                .ToArray();

            var start = new string(Enumerable.Range(0, target.Length).Select(_ => '.')
                .ToArray());
            var queue = new Queue<string>([start]);

            var level = 0;
            var seen = new HashSet<string>([start]);
            while (queue.Count > 0)
            {
                var k = queue.Count;
                for (var i = 0; i < k; i++)
                {
                    var node = queue.Dequeue();
                    if (target == node)
                    {
                        result += level;
                        break;
                    }

                    foreach (var press in buttonPresses)
                    {
                        var next = node.ToCharArray();
                        foreach (var button in press)
                        {
                            next[button] = node[button] == '.' ? '#' : '.';
                        }
                        var str = new string(next);
                        if (seen.Add(str))
                        {
                            queue.Enqueue(str);
                        }
                    }
                }

                level++;
            }
        }

        return result.ToString();
    }

    public string SolvePartTwo()
    {
        using var stream = new StreamReader("2025/input1.txt");

        var result = 0L;
        while (!stream.EndOfStream)
        {
            var line = stream.ReadLine()!
                .Split(' ');
            var target = line.Last()[1..^1];
            var joltages = target.Split(',').Select(long.Parse).ToArray();
            var n = joltages.Length;

            var buttonPresses = line.Skip(1).SkipLast(1)
                .Select(x => x.Replace("(", "").Replace(")", ""))
                .Select(x => x.Split(',').Select(long.Parse).ToArray())
                .ToArray();
            var m = buttonPresses.Length;

            /*
             * The LP problem we are trying to solve it the following:
             * Let m be the number of possible button presses
             * Let X_k a vector of size n, with coefficients in {0, 1}
             * Let Y a vector of size n representing the joltage level counters of a machine
             * Let a_k as integers be the number of button pushes for the k-th button press.
             *
             * Under the following constraints:
             * a_0 * X_0 + ... + a_(m-1) * X_(m-1) = Y
             * a_0,..., a_(m-1) >= 0
             *
             * We need to find min(a_0 + ... + a_(m-1))
             */

            var vectors = new bool[m][];
            for (var i = 0; i < m; i++)
            {
                vectors[i] = new bool[n];
                for (var j = 0; j < buttonPresses[i].Length; j++)
                {
                    vectors[i][buttonPresses[i][j]] = true;
                }
            }

            var model = new CpModel();
            var variables = Enumerable.Range(0, m)
                .Select(i => model.NewIntVar(0, 1_000, $"a_{i}"))
                .ToArray();

            for (var i = 0; i < n; i++)
            {
                var activeVariables = Enumerable.Range(0, m)
                    .Where(j => vectors[j][i])
                    .Select(j => variables[j])
                    .ToArray();

                model.Add(LinearExpr.Sum(activeVariables) == joltages[i]);
            }

            model.Minimize(LinearExpr.Sum(variables));
            var solver = new CpSolver();
            var status = solver.Solve(model);

            if (status is CpSolverStatus.Infeasible or CpSolverStatus.ModelInvalid or CpSolverStatus.Unknown)
            {
                Console.WriteLine("solver unable to find a solution to this LP problem");
                result += 0;
            }

            result += Enumerable.Range(0, m).Sum(k => solver.Value(variables[k]));
        }

        return result.ToString();
    }
}