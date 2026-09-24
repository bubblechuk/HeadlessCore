using HeadlessCore.Characters;
using HeadlessCore.Effects;

namespace HeadlessCore.Actions
{
    public enum TargetScope
    {
        SingleTarget,
        Self,
        AllyTeam,
        EnemyTeam,
        All,
        RandomTargets
    }
    public enum DamageType
    {
        Physical
    }
    public abstract class CBaseAction : IAction
    {
        public string Id { get; }
        //public string Name { get; }
        //public string Description { get; }
        public int Cost { get; }
        public TargetScope Scope { get; }
        public DamageType ActionType { get; }
        public int BaseValue { get; }
        public float StatScaling { get; }
        public IReadOnlyList<CStatusEffect> Effects => _effects;
        protected readonly List<CStatusEffect> _effects;
        protected CBaseAction(
            string id,
            int cost,
            TargetScope scope,
            DamageType actionType,
            int baseValue = 0,
            float statScaling = 0f,
            IEnumerable<CStatusEffect>? effects = null)
        {
            Id = id;
            Cost = cost;
            Scope = scope;
            ActionType = actionType;
            BaseValue = baseValue;
            StatScaling = statScaling;
            _effects = effects?.ToList() ?? new List<CStatusEffect>();
        }

        public virtual bool CanCast(ITargetable caster, ITargetable target, bool ignoreCost = false)
            => CanCast(caster, new[] { target }, ignoreCost);
        public virtual bool CanCast(ITargetable caster, IReadOnlyList<ITargetable> targets, bool ignoreCost = false)
        {
            if (caster is not CBaseEntity entity) return false;
            if (entity.IsDead) return false;
            if (!ignoreCost && !entity.HasEnoughSP(Cost)) return false;
            if (targets == null || targets.Count == 0) return false;

            return ValidateTargetScope(targets);
        }
        public virtual void Cast(ITargetable caster, ITargetable target, bool ignoreCost = false)
            => Cast(caster, new[] { target }, ignoreCost);
        public virtual void Cast(ITargetable caster, IReadOnlyList<ITargetable> targets, bool ignoreCost = false)
        {
            if (!CanCast(caster, targets, ignoreCost)) return;

            if (!ignoreCost && caster is CBaseEntity entityCaster)
            {
                entityCaster.ConsumeSP(Cost);
            }
            foreach (var target in targets)
            {
                if (target == null) continue;
                ExecuteOnTarget(caster, target);
            }
        }
        protected virtual bool ValidateTargetScope(IReadOnlyList<ITargetable> targets)
        {
            return Scope switch
            {
                TargetScope.SingleTarget or TargetScope.Self => targets.Count == 1,
                _ => true
            };
        }
        protected virtual void ExecuteOnTarget(ITargetable caster, ITargetable target)
        {
            ApplyInstantImpact(caster, target);

            if (target is CBaseEntity entityTarget)
            {
                foreach (var effect in _effects)
                {
                    entityTarget.ApplyStatus(effect);
                }
            }
        }
        protected virtual void ApplyInstantImpact(ITargetable caster, ITargetable target)
        {
            if (BaseValue == 0 && StatScaling == 0f) return;

            int power = BaseValue;
            if (caster is CBaseEntity entityCaster)
            {
                power += (int)(entityCaster.TotalStats.Caliber * StatScaling);
            }

            switch (ActionType)
            {
                case DamageType.Physical:
                    target.TakeDamage(power);
                    break;
            }
        }
    }
}
