namespace HeadlessCore.Items
{
    public abstract class CBaseItem
    {
        public string Id { get; }
        public string Name { get; }
        public string Description { get; }
        public int MaxStackSize { get; protected set; }
        public int CurrentStack { get; set; }
        public bool IsStackable => MaxStackSize > 1;
        protected CBaseItem(string id, string name, string description, int maxStackSize = 1, int currentStack = 1)
        {
            Id = id;
            Name = name;
            Description = description;
            MaxStackSize = Math.Max(1, maxStackSize);
            CurrentStack = Math.Clamp(currentStack, 1, MaxStackSize);
        }
    }
}
