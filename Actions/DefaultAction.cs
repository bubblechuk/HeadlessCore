using HeadlessCore.Effects;

namespace HeadlessCore.Actions
{
    public class DefaultAction : CBaseAction
    {
        public DefaultAction(
            string id,
            int cost,
            TargetScope scope,
            DamageType actionType,
            int baseValue = 0,
            float statScaling = 0f,
            IEnumerable<CStatusEffect>? effects = null)
            : base(id, cost, scope, actionType, baseValue, statScaling, effects)
        {
        }
    }
}