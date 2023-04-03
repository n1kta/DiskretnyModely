namespace DiskretnyModeliLabs.Lab2;

public static class DiikstraAlgorithm
{
    public static Dictionary<char, EdgeExtention> FindMinimalDistance(this Dictionary<char, List<Node>> nodes, char from)
    {
        var length = nodes.Count;
        var usedNodes = new List<char> { from };
        var distances = new Dictionary<char, EdgeExtention> { { from, new EdgeExtention(string.Empty, 0) } };

        for (int i = 0; i < length - 1; i++)
        {
            var minimalNodeChar = ' ';
            var minimalNodeDistance = int.MaxValue;
            var path = string.Empty;

            usedNodes.ForEach(x =>
            {
                var temporaryPath = x.ToString();
                var node = nodes[x]
                    .Where(y => !usedNodes.Contains(y.Name))
                    .OrderBy(y => y.Weight)
                    .FirstOrDefault();

                if (node != null)
                {
                    var minimalLength = node.Weight;

                    if (x != from)
                    {
                        temporaryPath = distances[x].Path;
                        minimalLength += distances[x].Length;
                    }

                    if (minimalNodeDistance > minimalLength)
                    {
                        minimalNodeDistance = minimalLength;
                        minimalNodeChar = node.Name;
                        path = temporaryPath;
                    }
                }
            });

            path += minimalNodeChar;
            usedNodes.Add(minimalNodeChar);
            distances.Add(minimalNodeChar, new EdgeExtention(path, minimalNodeDistance));
        }

        return distances;
    }
}