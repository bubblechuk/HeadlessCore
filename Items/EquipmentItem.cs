using HeadlessCore.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessCore.Items
{
    public class EquipmentItem : CBaseItem, IEquipable
    {
        public EquipmentItem(string id, string name, string description, EquipmentSlot slot, Stats bonusStats) : base(id, name, description, maxStackSize: 1, currentStack: 1)
        {
            Slot = slot;
            BonusStats = bonusStats;
        }

        public EquipmentSlot Slot { get; }

        public Stats BonusStats { get; }

        public bool CanEquip(ITargetable target)
        {
            throw new NotImplementedException();
        }
    }
}
