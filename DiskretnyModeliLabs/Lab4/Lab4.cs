namespace DiskretnyModeliLabs.Lab4
{
    public class Lab4
    {
        public static void Execute()
        {
            int[][] array = FileHelper.ReadFromFile("../../../Lab4/Data/l4-1.txt", true);
            char[] headers = StringHelper.GetNames(array.Length);

            var graph = new ChannelSolver();
            graph.FillFromMatrix(headers, array);

            graph.FordFulkerson();
        }
    }
}
