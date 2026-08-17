using HeadlessCore.Characters;

namespace HeadlessCore
{
    public interface ITargetable
    {
        Stats TotalStats { get; }
        void TakeDamage(int amount);
        void Heal(int amount);
        void RestoreSP(int amount);
        bool ConsumeSP(int amount);
    }
}
