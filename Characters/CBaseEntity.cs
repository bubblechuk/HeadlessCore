using HeadlessCore.Actions;
using HeadlessCore.Effects;

namespace HeadlessCore.Characters
{
    public abstract class CBaseEntity : ITargetable
    {
        public string Name { get; protected set; }
        public string Nickname { get; protected set; }
        public int Level { get; protected set; } = 1;
        public int XP { get; protected set; } = 0;
        public int RequiredXP => (int)(100 * Math.Pow(Level, 1.5));
        private readonly List<CStatusEffect> _activeStatuses = new();
        private readonly List<IAction> _actions = new();
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
        protected CBaseEntity(string name, string nickname, Stats baseStats)
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
        public virtual void TakeDamage(int amount)
        {
            if (amount <= 0 || IsDead) return;
            HP -= amount;
            OnDamageTaken?.Invoke(amount);
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
        protected virtual void Death()
        {
        }
        protected virtual void CheckLevelUp()
        {
        }
        public void ApplyStatus(CStatusEffect status)
        {
            _activeStatuses.Add(status);
            status.OnApply(this);
            RecalculateStats();
        }
        public void RemoveStatus(CStatusEffect status)
        {
            if (_activeStatuses.Remove(status))
            {
                status.OnRemove(this);
                RecalculateStats();
            }
        }
        public void TickStatuses()
        {
            foreach (var status in _activeStatuses.ToList())
            {
                status.OnTurnTick(this);
            }

            RecalculateStats();
        }
        public void RecalculateStats()
        {
            Stats accumulatedBonus = Stats.Zero;

            foreach (var status in _activeStatuses)
            {
                accumulatedBonus += status.GetStatModifier();
            }

            BonusStats = accumulatedBonus;

            _currentHP = Math.Clamp(_currentHP, 0, MaxHP);
            _currentSP = Math.Clamp(_currentSP, 0, MaxSP);
        }
        public void UseAction(IAction action, ITargetable target)
        {
            if (action.CanCast(this, target))
            {
                action.Cast(this, target);
            }
        }
    }
}
