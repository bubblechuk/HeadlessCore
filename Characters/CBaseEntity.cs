using HeadlessCore.Actions;
using HeadlessCore.Configurations;
using HeadlessCore.Effects;

namespace HeadlessCore.Characters
{
    public class CBaseEntity : ITargetable
    {
        public string Name { get; protected set; } = "SampleName";
        public string Nickname { get; protected set; } = "SampleNickname";
        public int Level { get; protected set; } = 1;
        public int XP { get; protected set; } = 0;
        public int RequiredXP => (int)(100 * Math.Pow(Level, 1.5));
        private readonly List<CStatusEffect> _activeStatuses = new();
        public IReadOnlyList<CBaseAction> Actions => _actions;
        private readonly List<CBaseAction> _actions = new List<CBaseAction>(8);
        public Stats BaseStats { get; protected set; }
        public Stats BonusStats { get; protected set; }
        public Stats TotalStats => BaseStats + BonusStats;
        public bool IsDead => HP <= 0;
        public bool HasEnoughSP(int amount) => SP >= amount;
        public virtual int MaxHP => 100 + (Level - 1) * 5;
        public virtual int MaxSP => 100 + (Level - 1) * 3;
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
        public event Action? OnActionsChanged;
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
        public CBaseEntity(EntityConfig config) 
        {
            Name = config.Name;
            Nickname = config.Nickname;
            BaseStats = config.BaseStats.ToDomainStats();
            Level = config.StartingLevel;

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
        public void ApplyStatus(CStatusEffect status)
        {
            if (status == null || IsDead) return;
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
            if (IsDead) return;
            foreach (var status in _activeStatuses.ToList())
            {
                status.OnTurnTick(this);
                if (IsDead) break;
            }

            RecalculateStats();
            var expiredStatuses = _activeStatuses.Where(s => s.IsExpired).ToList();
            foreach (var status in expiredStatuses)
            {
                RemoveStatus(status);
            }
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
        public void UseAction(int slotId, ITargetable target)
        {
            if (IsDead || slotId < 0 || slotId >= _actions.Count || target == null) return;

            var action = _actions[slotId];
            if (action.CanCast(this, target))
            {
                action.Cast(this, target);
            }
        }
        public void UseAction(int slotId, IReadOnlyList<ITargetable> targets)
        {
            if (IsDead || slotId < 0 || slotId >= _actions.Count || targets == null || targets.Count == 0) return;

            var action = _actions[slotId];
            if (action.CanCast(this, targets))
            {
                action.Cast(this, targets);
            }
        }
        public void LearnAction(CBaseAction action)
        {
            if (action == null) return;
            if (_actions.Any(a => a.Id == action.Id)) return;
            _actions.Add(action);
            OnActionsChanged?.Invoke();
        }
        public int ForgetAction(CBaseAction action)
        {
            if (action == null) return 0;
            int removed = _actions.RemoveAll(a => a.Id == action.Id);
            if (removed > 0)
            {
                OnActionsChanged?.Invoke();
            }
            return removed;
        }
        public bool ReplaceAction(int slotId, CBaseAction newAction)
        {
            if (newAction == null || slotId < 0 || slotId >= _actions.Count)
                return false;
            if (_actions.Where((_, index) => index != slotId).Any(a => a.Id == newAction.Id))
                return false;
            _actions[slotId] = newAction;
            OnActionsChanged?.Invoke();
            return true;
        }
        public virtual void Die() { }
        public virtual void OnLevelUpStats() { }
    }
}