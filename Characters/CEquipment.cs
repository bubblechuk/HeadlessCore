using HeadlessCore.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessCore.Characters
{
    public class CEquipment
    {
        public Dictionary<EquipmentSlot, EquipmentItem> _slots = new();
        public IReadOnlyDictionary<EquipmentSlot, EquipmentItem> Slots => _slots.AsReadOnly();
        public bool TryEquip(EquipmentItem item, out EquipmentItem? unequippedItem)
        {
            unequippedItem = null;
            if (_slots.TryGetValue(item.Slot, out var current))
            {
                unequippedItem = current;
            }
            _slots[item.Slot] = item;
            return true;
        }
        public bool TryUnequip(EquipmentSlot slot, out EquipmentItem? unequippedItem)
        {
            return _slots.Remove(slot, out unequippedItem);
        }
        public Stats GetTotalEquipmentStats()
        {
            Stats total = new Stats();
            foreach (var item in _slots.Values)
            {
                total += item.BonusStats;
            }
            return total;
        }
    }
}
