namespace AdventOfCode._2025;

public sealed class Day8 : IDay
{
    private sealed record JunctionBox(long X, long Y, long Z)
    {
        public long SquaredDistanceFrom(JunctionBox other)
        {
            return (X - other.X) * (X - other.X)
                   + (Y - other.Y) * (Y - other.Y)
                   + (Z - other.Z) * (Z - other.Z);
        }
    }
    
    public string SolvePartOne()
    {
        var junctionBoxes = File.ReadAllLines("2025/input1.txt")
            .Select(x => x.Split(','))
            .Select(x => new JunctionBox(int.Parse(x[0]), int.Parse(x[1]), int.Parse(x[2])))
            .ToArray();

        var disjointSetUnion = new DisjointSetUnion();

        foreach (var junctionBox in junctionBoxes)
        {
            disjointSetUnion.MakeSet(junctionBox);
        }

        var distances = new List<(JunctionBox, JunctionBox)>();
        for (var i = 0; i < junctionBoxes.Length; i++)
        {
            for (var j = i + 1; j < junctionBoxes.Length; j++)
            {
                distances.Add((junctionBoxes[i], junctionBoxes[j]));
            }
        }

        var closestPairs = distances.OrderBy(x => x.Item1.SquaredDistanceFrom(x.Item2))
            .Take(1000); // change this to 10 for the sample.

        foreach (var pair in closestPairs)
        {
            disjointSetUnion.Union(pair.Item1, pair.Item2);
        }

        return junctionBoxes.GroupBy(x => disjointSetUnion.Find(x))
            .OrderByDescending(x => x.Count())
            .Take(3)
            .Aggregate(1L, (x, y) => x * y.Count())
            .ToString();
    }

    public string SolvePartTwo()
    {
        var junctionBoxes = File.ReadAllLines("2025/input1.txt")
            .Select(x => x.Split(','))
            .Select(x => new JunctionBox(int.Parse(x[0]), int.Parse(x[1]), int.Parse(x[2])))
            .ToArray();

        var disjointSetUnion = new DisjointSetUnion();

        foreach (var junctionBox in junctionBoxes)
        {
            disjointSetUnion.MakeSet(junctionBox);
        }

        var distances = new List<(JunctionBox, JunctionBox)>();
        for (var i = 0; i < junctionBoxes.Length; i++)
        {
            for (var j = i + 1; j < junctionBoxes.Length; j++)
            {
                distances.Add((junctionBoxes[i], junctionBoxes[j]));
            }
        }

        var closestPairs = distances.OrderBy(x => x.Item1.SquaredDistanceFrom(x.Item2));

        var result = 0L;
        foreach (var pair in closestPairs)
        {
            disjointSetUnion.Union(pair.Item1, pair.Item2);
            if (disjointSetUnion.SetCount == 1)
            {
                result = pair.Item1.X * pair.Item2.X;
                break;
            }
        }

        return result.ToString();
    }

    private sealed class DisjointSetUnion
    {
        private readonly Dictionary<JunctionBox, JunctionBox> _parent = new();
        private readonly Dictionary<JunctionBox, int> _size = new();

        public int SetCount { get; private set; }

        public void MakeSet(JunctionBox junctionBox)
        {
            _parent[junctionBox] = junctionBox;
            _size[junctionBox] = 1;
            SetCount++;
        }

        public JunctionBox Find(JunctionBox junctionBox)
        {
            if (!_parent.TryGetValue(junctionBox, out var value))
            {
                return junctionBox;
            }

            return junctionBox == value ? junctionBox : _parent[junctionBox] = Find(value);
        }

        public void Union(JunctionBox a, JunctionBox b)
        {
            var rootA = Find(a);
            var rootB = Find(b);
            if (rootA == rootB)
            {
                return;
            }

            if (_size[rootA] < _size[rootB])
            {
                (rootA, rootB) = (rootB, rootA);
            }

            _parent[rootB] = rootA;
            _size[rootA] += _size[rootB];
            SetCount--;
        }
    }
}