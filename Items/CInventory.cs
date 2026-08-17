namespace HeadlessCore.Items
{
    public class CInventory
    {
        public IReadOnlyList<CBaseItem> Items => _items.AsReadOnly();
        private readonly List<CBaseItem> _items = new();
        public int Capacity { get; }
        public int CurrentCapacity => _items.Count;
        public event Action<CBaseItem>? OnItemAdded;
        public event Action<CBaseItem>? OnItemRemoved;
        public event Action? OnInventoryFull;

        public CInventory(int capacity = 20)
        {
            Capacity = capacity;
        }
        public bool TryAddItem(CBaseItem item)
        {
            if (item == null || item.CurrentStack <= 0) return false;
            if (item.IsStackable)
            {
                var availableStacks = _items.Where(i => i.Id == item.Id && i.CurrentStack < i.MaxStackSize);

                foreach (var existingItem in availableStacks)
                {
                    int spaceInStack = existingItem.MaxStackSize - existingItem.CurrentStack;
                    int amountToAdd = Math.Min(spaceInStack, item.CurrentStack);

                    existingItem.CurrentStack += amountToAdd;
                    item.CurrentStack -= amountToAdd;

                    OnItemAdded?.Invoke(existingItem);
                    if (item.CurrentStack <= 0)
                    {
                        return true;
                    }
                }
            }
            if (CurrentCapacity >= Capacity)
            {
                OnInventoryFull?.Invoke();
                return false;
            }
            _items.Add(item);
            OnItemAdded?.Invoke(item);
            return true;
        }
        public bool RemoveItem(CBaseItem item)
        {
            if (_items.Remove(item))
            {
                OnItemRemoved?.Invoke(item);
                return true;
            }
            return false;
        }
        public void CleanEmptyStacks()
        {
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                if (_items[i].CurrentStack == 0)
                {
                   var removedItem = _items[i];
                   _items.RemoveAt(i);
                    OnItemRemoved?.Invoke(removedItem);
                }
            }
        }
    }
}
