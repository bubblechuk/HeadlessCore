using HeadlessCore.Characters;
using HeadlessCore.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessCore.Items
{
    public class EquipmentItem : CBaseItem, IEquipable
    {
        public EquipmentItem(string id, EquipmentSlot slot, Stats bonusStats) : base(id, maxStackSize: 1, currentStack: 1)
        {
            Slot = slot;
            BonusStats = bonusStats;
        }

        public EquipmentSlot Slot { get; }

        public Stats BonusStats { get; }

        public bool CanEquip(CBaseEntity target)
        {
            if (target == null) return false;
            foreach (var status in target.Statuses)
            {
                if (status.ShouldSuppressItem(this))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
