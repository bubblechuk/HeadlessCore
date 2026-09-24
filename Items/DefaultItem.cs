namespace HeadlessCore.Items
{
    public class CDefaultItem : CBaseItem
    {
        public CDefaultItem(
            string id,
            int maxStackSize = 1,
            int currentStack = 1
        ) : base(id, maxStackSize, currentStack)
        {
        }
    }
}