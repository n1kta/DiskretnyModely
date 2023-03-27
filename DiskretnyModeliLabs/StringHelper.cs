namespace DiskretnyModeliLabs
{
    public static class StringHelper
    {
        public static char[] GetNames(int count)
        {
            return Enumerable.Range('A', count).Select(x => (char)x).ToArray();
        }
    }
}
