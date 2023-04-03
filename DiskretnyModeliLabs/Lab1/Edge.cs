namespace DiskretnyModeliLabs.Lab1
{
    public class Edge
    {
        public int Weight { get; set; }
        public string Name { get; set; }

        public bool Selected { get; set; } = false;
        public readonly bool Searchable;

        public Edge(int weight, string name, bool searchable)
        {
            Weight = weight;
            Name = name;
            Searchable = searchable;
        }

        public bool IsNeeded(List<Edge> usedNodes)
        {
            var names = usedNodes.Select(x => x.Name).ToList();
            names.Add(Name);

            var sets = new List<List<char>>
            {
                new List<char>()
            };


            foreach (string name in names)
            {
                var concat = new List<int>();

                for (int i = 0; i < sets.Count; i++)
                {
                    var list = sets[i];

                    var containsFrom = list.Contains(name[0]);
                    var containsTo = list.Contains(name[1]);

                    if (containsFrom && containsTo)
                        return false;

                    if (containsFrom || containsTo)
                    {
                        concat.Add(i);

                        if (containsTo)
                            list.Add(name[0]);

                        if (containsFrom)
                            list.Add(name[1]);

                        break;
                    }
                }

                if (concat.Count == 0)
                {
                    sets.Add(new List<char>()
                    {
                        name[0], 
                        name[1]
                    });
                }
                else
                {
                    sets = CreateNewSet(sets, name, concat);
                }
            }

            return true;
        }

        private static List<List<char>> CreateNewSet(List<List<char>> sets, string name, List<int> concat)
        {
            for (int i = concat[0]; i < sets.Count; i++)
            {
                var list = sets[i];
                var containsFrom = list.Contains(name[0]);
                var containsTo = list.Contains(name[1]);

                if (containsFrom || containsTo)
                    concat.Add(i);
            }

            var newSets = new List<List<char>>();

            var concatedList = new List<char>();

            for (int i = 0; i < sets.Count; i++)
            {
                if (concat.Contains(i))
                    concatedList.AddRange(sets[i]);
                else
                    newSets.Add(sets[i]);
            }

            newSets.Add(concatedList);
            sets = newSets;
            return sets;
        }
    }

    public static class NodeExtension
    {
        public static List<Edge> GetSelected(this List<List<Edge>> array)
        {
            return array
                .SelectMany(row => row.Select(x => x))
                .Where(edge => edge.Selected)
                .ToList();
        }
    }
}
