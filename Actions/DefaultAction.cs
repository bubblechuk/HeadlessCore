using HeadlessCore.Effects;

namespace HeadlessCore.Actions
{
    public class DefaultAction : CBaseAction
    {
        public DefaultAction(
            string id,
            string name,
            string description,
            int cost,
            TargetScope scope,
            DamageType actionType,
            int baseValue = 0,
            float statScaling = 0f,
            IEnumerable<CStatusEffect>? effects = null)
            : base(id, name, description, cost, scope, actionType, baseValue, statScaling, effects)
        {
        }
    }
}