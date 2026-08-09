using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessCore.Items
{
    public class CInventory
    {
        private readonly List<CBaseItem> _items = new();
        public int Capacity { get; }
        public event Action<CBaseItem>? OnItemAdded;
        public event Action<CBaseItem>? OnItemRemoved;
        public event Action? OnInventoryFull;

        public CInventory(int capacity = 20)
        {
            Capacity = capacity;
        }
    }
}
