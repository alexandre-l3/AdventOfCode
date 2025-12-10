namespace AdventOfCode._2025;

public sealed class Day9 : IDay
{
    private sealed record Point(long X, long Y);

    private sealed record Edge(Point A, Point B);
    

    public string SolvePartOne()
    {
        var tiles = File.ReadAllLines("2025/input1.txt")
            .Select(x => x.Split(','))
            .Select(x => new Point(int.Parse(x[0]), int.Parse(x[1])))
            .ToArray();

        return tiles.Aggregate(long.MinValue,
                (current, topCorner) => (from lowerCorner in tiles
                    let width = Math.Abs(topCorner.Y - lowerCorner.Y + 1L)
                    let height = Math.Abs(topCorner.X - lowerCorner.X + 1L)
                    select height * width).Prepend(current).Max())
            .ToString();
    }

    public string SolvePartTwo()
    {
        var tiles = File.ReadAllLines("2025/input1.txt")
            .Select(x => x.Split(','))
            .Select(x => new Point(int.Parse(x[0]), int.Parse(x[1])))
            .ToArray();

        var maxArea = long.MinValue;

        var edges = tiles.Select((t, i) => new Edge(t, tiles[(i + 1) % tiles.Length]))
            .ToArray();

        for (var i = 0; i < tiles.Length; i++)
        {
            for (var j = 0; j < tiles.Length; j++)
            {
                var topCorner = tiles[i];
                var bottomCorner = tiles[j];
                if (!IsRectangleInRegion(edges, topCorner, bottomCorner))
                {
                    continue;
                }

                var width = Math.Abs(topCorner.Y - bottomCorner.Y) + 1L;
                var height = Math.Abs(topCorner.X - bottomCorner.X) + 1L;
                maxArea = Math.Max(height * width, maxArea);
            }
        }

        return maxArea.ToString();
    }

    private static bool IsRectangleInRegion(Edge[] edges, Point topCorner, Point bottomCorner)
    {
        var maxX = Math.Max(topCorner.X, bottomCorner.X);
        var minX = Math.Min(topCorner.X, bottomCorner.X);
        var maxY = Math.Max(topCorner.Y, bottomCorner.Y);
        var minY = Math.Min(topCorner.Y, bottomCorner.Y);

        var rectangle = new Point[]
        {
            new(minX, minY),
            new(maxX, minY),
            new(maxX, maxY),
            new(minX, maxY)
        };

        return rectangle.All(x => IsTileInsideRegion(x, edges)) && AreEdgesInside(minX, maxX, minY, maxY, edges);
    }

    private static bool AreEdgesInside(long minX, long maxX, long minY, long maxY, Edge[] edges)
    {
        foreach (var edge in edges)
        {
            if (edge.A.X == edge.B.X)
            {
                var edgeYMax = Math.Max(edge.A.Y, edge.B.Y);
                var edgeYMin = Math.Min(edge.A.Y, edge.B.Y);
                if (edge.A.X > minX &&
                    edge.A.X < maxX &&
                    edgeYMax > minY &&
                    edgeYMin < maxY)
                {
                    return false;
                }
            }
            else if (edge.A.Y == edge.B.Y)
            {
                var edgeXMax = Math.Max(edge.A.X, edge.B.X);
                var edgeXMin = Math.Min(edge.A.X, edge.B.X);
                if (edge.A.Y > minY &&
                    edge.A.Y < maxY &&
                    edgeXMax > minX &&
                    edgeXMin < maxX)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static bool IsTileInsideRegion(Point point, Edge[] edges)
    {
        if (edges.Any(e => IsPointOnSegment(point, e)))
        {
            return true;
        }

        return edges.Where(edge => point.X < edge.A.X)
            .Where(edge => point.Y >= Math.Min(edge.A.Y, edge.B.Y))
            .Where(edge => point.Y < Math.Max(edge.A.Y, edge.B.Y))
            .Aggregate(false, (current, _) => !current);
    }

    private static bool IsPointOnSegment(Point point, Edge edge) =>
        Math.Min(edge.A.X, edge.B.X) <= point.X &&
        point.X <= Math.Max(edge.A.X, edge.B.X) &&
        Math.Min(edge.A.Y, edge.B.Y) <= point.Y &&
        point.Y <= Math.Max(edge.A.Y, edge.B.Y);
}