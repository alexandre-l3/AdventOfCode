namespace AdventOfCode;

public sealed class Day11 : IDay
{
    public string SolvePartOne()
    {
        var stones = Arrangement.Split(' ').Select(long.Parse).ToList();

        for (var i = 0; i < TwentyFive; i++)
        {
            var newStones = new List<long>();

            foreach (var stone in stones)
            {
                var text = $"{stone}";
                if (stone == 0)
                {
                    newStones.Add(1);
                }
                else if (text.Length % 2 == 0)
                {
                    var left = long.Parse(text[..(text.Length / 2)]);
                    var right = long.Parse(text[(text.Length / 2)..]);
                    newStones.AddRange([left, right]);
                }
                else
                {
                    newStones.Add(stone * 2024);
                }
            }

            stones = [..newStones];
        }

        return stones.Count.ToString();
    }

    public string SolvePartTwo()
    {
        var cache = Arrangement.Split(' ')
            .Select(x => new KeyValuePair<long, long>(long.Parse(x), 1))
            .ToDictionary();

        for (var i = 0; i < SeventyFive; i++)
        {
            var nextCache = new Dictionary<long, long>();
            foreach (var item in cache)
            {
                var text = $"{item.Key}";
                if (item.Key == 0)
                {
                    nextCache[1] = nextCache.GetValueOrDefault(1) + item.Value;
                }
                else if (text.Length % 2 == 0)
                {
                    var left = long.Parse(text[..(text.Length / 2)]);
                    var right = long.Parse(text[(text.Length / 2)..]);
                    nextCache[left] = nextCache.GetValueOrDefault(left) + item.Value;
                    nextCache[right] = nextCache.GetValueOrDefault(right) + item.Value;
                }
                else
                {
                    var product = item.Key * 2024;
                    nextCache[product] = nextCache.GetValueOrDefault(product) + item.Value;
                }
            }

            cache = nextCache;
        }

        return cache.Values.Sum().ToString();
    }

    private const int TwentyFive = 25;
    private const int SeventyFive = 75;

    private const string Arrangement = "125 17";
}