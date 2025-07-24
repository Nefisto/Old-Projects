namespace Loot
{
    public partial class Drop
    {
        public object Clone()
            => new Drop(this, true);
    }
}