using HeadlessCore.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessCore.Items
{
    public enum EquipmentSlot
    {
        Weapon,
        Armor,
        Accessory
    }
    internal interface IEquipable
    {
        EquipmentSlot Slot { get; }
        Stats BonusStats { get; }
        bool CanEquip(CBasePlayer target);
    }
}
