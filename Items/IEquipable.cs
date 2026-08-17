using HeadlessCore.Characters;

namespace HeadlessCore.Items
{
    public enum EquipmentSlot
    {
        Weapon,
        Armor,
        Accessory
    }
    public interface IEquipable
    {
        EquipmentSlot Slot { get; }
        Stats BonusStats { get; }
        bool CanEquip(ITargetable target);
    }
}
