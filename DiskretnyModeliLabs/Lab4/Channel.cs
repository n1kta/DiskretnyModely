using DiskretnyModeliLabs.Lab2;

namespace DiskretnyModeliLabs.Lab4
{
    public class Channel : Node
    {
        public int UsedWeight { get; set; } = 0;
        public int RemainingWeight => Weight - UsedWeight;
    }
}
