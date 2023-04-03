namespace DiskretnyModeliLabs.Lab2
{
    public static class GraphSolverExstension
    {
        public static List<char> GetOddNodes(this Dictionary<char, List<Node>> nodes)
        {
            return nodes.Where(i => i.Value.Count % 2 != 0).Select(x => x.Key).ToList();
        }

        public static bool CheckIfRepeats(this IReadOnlyList<EdgeExtention> edges)
        {
            foreach (var edge in edges)
            {
                foreach (var secondEdge in edges)
                {
                    if (edge.Path != secondEdge.Path
                        && edge.Intersect(secondEdge.Path))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static int GetSum(this IReadOnlyList<EdgeExtention> edges)
        {
            return edges.Sum(x => x.Length);
        }
    }
}
