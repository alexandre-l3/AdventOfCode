namespace AdventOfCode;

public sealed class Day5 : IDay
{
    public string SolvePartOne()
    {
        var relations = GetRelations();
        var greaterThan = GetRelationsMap(relations);

        return _updates.Select(update => update.Split(',').Select(int.Parse).ToArray())
            .Where(pageNumbers => IsInOrder(pageNumbers, greaterThan))
            .Sum(pageNumbers => pageNumbers[pageNumbers.Length / 2])
            .ToString();
    }

    public string SolvePartTwo()
    {
        var relations = GetRelations();
        var greaterThan = GetRelationsMap(relations);
        var comparer = new PageComparer(greaterThan);

        return _updates.Select(update => update.Split(',').Select(int.Parse).ToArray())
            .Where(pageNumbers => !IsInOrder(pageNumbers, greaterThan))
            .Sum(pageNumbers =>
            {
                Array.Sort(pageNumbers, comparer);
                return pageNumbers[pageNumbers.Length / 2];
            }).ToString();
    }

    private IEnumerable<(int, int)> GetRelations()
    {
        return _orderingRules.Select(rule => rule.Split('|'))
            .Select(rule => (int.Parse(rule[0]), int.Parse(rule[1])));
    }

    private static Dictionary<int, HashSet<int>> GetRelationsMap(IEnumerable<(int, int)> relations)
    {
        var greaterThan = new Dictionary<int, HashSet<int>>();
        foreach (var (a, b) in relations)
        {
            if (greaterThan.TryGetValue(a, out var value))
            {
                value.Add(b);
            }
            else
            {
                greaterThan.Add(a, [b]);
            }
        }

        return greaterThan;
    }

    private static bool IsInOrder(int[] pageNumbers, Dictionary<int, HashSet<int>> greaterThan)
    {
        var previous = pageNumbers[0];
        var isInOrder = true;
        for (var i = 1; i < pageNumbers.Length; i++)
        {
            var current = pageNumbers[i];
            if (greaterThan.TryGetValue(previous, out var value) && value.Contains(current))
            {
                previous = current;
                continue;
            }

            isInOrder = false;
            break;
        }

        return isInOrder;
    }

    private sealed class PageComparer(Dictionary<int, HashSet<int>> greaterThan) : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            if (greaterThan.TryGetValue(x, out var value) && value.Contains(y))
            {
                return 1;
            }

            return -1;
        }
    }

    private readonly string[] _orderingRules =
    [
        "47|53",
        "97|13",
        "97|61",
        "97|47",
        "75|29",
        "61|13",
        "75|53",
        "29|13",
        "97|29",
        "53|29",
        "61|53",
        "97|53",
        "61|29",
        "47|13",
        "75|47",
        "97|75",
        "47|61",
        "75|61",
        "47|29",
        "75|13",
        "53|13"
    ];

    private readonly string[] _updates =
    [
        "75,47,61,53,29",
        "97,61,53,29,13",
        "75,29,13",
        "75,97,47,61,53",
        "61,13,29",
        "97,13,75,29,47"
    ];
}