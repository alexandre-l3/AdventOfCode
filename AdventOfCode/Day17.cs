using System.Text.RegularExpressions;

namespace AdventOfCode;

public sealed partial class Day17 : IDay
{
    [GeneratedRegex("\\d+")]
    private static partial Regex NumberRegex();

    public string SolvePartOne()
    {
        var registers = _registers.Select(x => NumberRegex().Match(x))
            .Select(m => m.Value)
            .Select(long.Parse)
            .ToArray();

        var program = NumberRegex().Matches(_program).Select(m => m.Value)
            .Select(int.Parse)
            .ToArray();

        return RunProgram(program, registers);
    }

    public string SolvePartTwo()
    {
        var index = 1L;
        var program = NumberRegex().Matches(_program).Select(m => m.Value)
            .Select(int.Parse)
            .ToArray();
        var input = string.Join(',', program);
        string result;
        while (true)
        {
            var output = RunProgram(program, [index, 0, 0]);

            if (output == input)
            {
                result = index.ToString();
                break;
            }

            if (input.EndsWith(output))
            {
                index *= 8;
            }
            else
            {
                index++;
            }
        }

        return result;
    }
    
    private static string RunProgram(int[] program, long[] registers)
    {
        var output = new List<long>();

        var i = 0;
        while (i < program.Length)
        {
            var instruction = program[i];
            if (instruction == 0)
            {
                registers[0] >>= (int)GetComboOperand(program[i + 1], registers);
                i += 2;
            }
            else if (instruction == 1)
            {
                registers[1] ^= program[i + 1];
                i += 2;
            }
            else if (instruction == 2)
            {
                registers[1] = GetComboOperand(program[i + 1], registers) % 8;
                i += 2;
            }
            else if (instruction == 3)
            {
                if (registers[0] != 0)
                {
                    i = program[i + 1];
                }
                else
                {
                    i += 2;
                }
            }
            else if (instruction == 4)
            {
                registers[1] ^= registers[2];
                i += 2;
            }
            else if (instruction == 5)
            {
                output.Add(GetComboOperand(program[i + 1], registers) % 8);
                i += 2;
            }
            else if (instruction == 6)
            {
                registers[1] = registers[0] >> (int)GetComboOperand(program[i + 1], registers);
                i += 2;
            }
            else
            {
                registers[2] = registers[0] >> (int)GetComboOperand(program[i + 1], registers);
                i += 2;
            }
        }

        return string.Join(',', output.Select(x => $"{x}"));
    }

    private static long GetComboOperand(int n, long[] registers)
    {
        return n switch
        {
            >= 0 and < 4 => n,
            >= 4 and < 7 => registers[n - 4],
            _ => throw new ArgumentOutOfRangeException(nameof(n), n, null)
        };
    }

    private readonly string[] _registers =
    [
        "Register A: 2024",
        "Register B: 0",
        "Register C: 0"
    ];

    private const string _program = "Program: 0,3,5,4,3,0";
}