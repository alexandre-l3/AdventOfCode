using System.Text;

namespace AdventOfCode;

public sealed class Day9 : IDay
{
    public string SolvePartOne()
    {
        var stringBuilder = new StringBuilder();
        var id = 0;
        for (var i = 0; i < DiskMap.Length; i++)
        {
            if (i % 2 == 0)
            {
                for (var j = 0; j < DiskMap[i] - '0'; j++)
                {
                    stringBuilder.Append(id);
                }
                id++;
            }
            else
            {
                for (var j = 0; j < DiskMap[i] - '0'; j++)
                {
                    stringBuilder.Append('.');
                }
            }
        }

        var transformedMap = stringBuilder.ToString().ToCharArray();

        var freeSpaces = transformedMap.Count(x => x == '.');
        var queue = new Queue<int>();

        for (var i = transformedMap.Length - 1; i >= 0; i--)
        {
            if (queue.Count == freeSpaces)
            {
                break;
            }
            if (transformedMap[i] == '.')
            {
                continue;
            }
            queue.Enqueue(transformedMap[i] - '0');
        }

        for (var i = 0; i < transformedMap.Length; i++)
        {
            if (queue.Count == 0)
            {
                transformedMap[i] = '.';
            }
            if (queue.Count > 0 && transformedMap[i] == '.')
            {
                transformedMap[i] = (char) (queue.Dequeue() + '0');
            }
        }

        return new string(transformedMap);
    }

    public string SolvePartTwo()
    {
        throw new NotImplementedException();
    }

    private static string DiskMap = "2333133121414131402";
}