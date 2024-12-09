using System.Text;

namespace AdventOfCode;

public sealed class Day9 : IDay
{
    public string SolvePartOne()
    {
        var stringBuilder = new StringBuilder();
        var id = 0;
        var freeSpaceIndices = new Queue<int>();
        for (var i = 0; i < _diskMap.Length; i++)
        {
            var digit = _diskMap[i] - '0';
            if (i % 2 == 0)
            {
                for (var j = 0; j < digit; j++)
                {
                    stringBuilder.Append(id);
                }
                id++;
            }
            else
            {
                for (var j = 0; j < digit; j++)
                {
                    freeSpaceIndices.Enqueue(stringBuilder.Length);
                    stringBuilder.Append('.');
                }
            }
        }

        var transformedMap = stringBuilder.ToString().ToCharArray();
        Console.WriteLine(stringBuilder.ToString());
        Console.WriteLine("----");
        for (var i = transformedMap.Length - 1; i >= 0; i--)
        {
            if (transformedMap[i] == '.')
            {
                continue;
            }

            if (freeSpaceIndices.Count == 0)
            {
                break;
            }
            var freeSpaceIndex = freeSpaceIndices.Dequeue();
            if (freeSpaceIndex > i)
            {
                break;
            }
            (transformedMap[freeSpaceIndex], transformedMap[i]) = (transformedMap[i], transformedMap[freeSpaceIndex]);
        }

        Console.WriteLine(new string(transformedMap));

        return transformedMap.Where(char.IsDigit).Select((x, i) => 1L * (x - '0') * i).Sum().ToString();
    }

    public string SolvePartTwo()
    {
        throw new NotImplementedException();
    }

    private readonly string _diskMap = DiskMap;
    private static string DiskMap = "2333133121414131402";
}