using HeadlessCore.Effects;
using System.Net.NetworkInformation;
using static System.Net.Mime.MediaTypeNames;

namespace HeadlessCore.Characters
{
    public abstract class CBasePlayer : ITarget
    {
        public string Name { get; protected set; }
        public string Nickname { get; protected set; }
        public int Level { get; protected set; } = 1;
        public int XP { get; protected set; } = 0;
        public int RequiredXP => (int)(100 * Math.Pow(Level, 1.5));
        public Stats BaseStats { get; protected set; }
        public Stats BonusStats { get; protected set; }
        public Stats TotalStats => BaseStats + BonusStats;
        public bool IsDead => HP <= 0;
        public bool HasEnoughSP(int amount) => SP >= amount;
        public virtual int MaxHP => 100;
        public virtual int MaxSP => 100;
        private int _currentHP;
        private int _currentSP;
        public int HP
        {
            get => _currentHP;
            protected set => _currentHP = Math.Clamp(value, 0, MaxHP);
        }
        public int SP
        {
            get => _currentSP;
            protected set => _currentSP = Math.Clamp(value, 0, MaxSP);
        }

        public event Action<int>? OnLevelUp;
        public event Action<int>? OnDamageTaken;
        public event Action? OnDeath;
        public event Action<int>? OnHealed;
        protected CBasePlayer(string name, string nickname, Stats baseStats)
        {
            Name = name;
            Nickname = nickname;
            BaseStats = baseStats;
            BonusStats = new Stats();
            _currentHP = MaxHP;
            _currentSP = MaxSP;
        }
        public virtual void AddXP(int amount)
        {
            if (amount <= 0 || IsDead) return;
            XP += amount;
            while (XP >= RequiredXP)
            {
                XP -= RequiredXP;
                Level++;

                HP = MaxHP;
                SP = MaxSP;

                OnLevelUp?.Invoke(Level);
            }
        }
        public virtual void TakeDamage(int damage)
        {
            if (damage <= 0 || IsDead) return;
            HP -= damage;
            OnDamageTaken?.Invoke(damage);
            if (IsDead)
            {
                OnDeath?.Invoke();
            }
        }
        public virtual void Heal(int amount)
        {
            if (amount <= 0 || IsDead) return;
            int oldHp = HP;
            HP += amount;

            int actualHeal = HP - oldHp;
            if (actualHeal > 0)
            {
                OnHealed?.Invoke(actualHeal);
            }
        }
        public virtual bool ConsumeSP(int amount)
        {
            if (amount <= 0) return true;
            if (!HasEnoughSP(amount) || IsDead) return false;

            SP -= amount;
            return true;
        }
        public virtual void RestoreSP(int amount)
        {
            if (amount <= 0 || IsDead) return;
            SP += amount;
        }
        public void ApplyEffect(IEffect effect)
        {
            //...
        }
    }
}
