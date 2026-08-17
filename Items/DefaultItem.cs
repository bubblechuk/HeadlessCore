namespace HeadlessCore.Items
{
    public class CDefaultItem : CBaseItem
    {
        public CDefaultItem(
            string id,
            string name,
            string description,
            int maxStackSize = 1,
            int currentStack = 1
        ) : base(id, name, description, maxStackSize, currentStack)
        {
        }
    }
}