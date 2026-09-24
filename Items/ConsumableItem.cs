using HeadlessCore.Actions;
using HeadlessCore.Factories;
using System.Reflection.Metadata;

namespace HeadlessCore.Items
{
    public class ConsumableItem : CBaseItem, IConsumable
    {
        public IAction BoundAction { get; }
        public ConsumableItem(
            string id, 
            string action,
            int maxStackSize = 1, 
            int currentStack = 1
            ) : base(id, maxStackSize, currentStack)
        {
            BoundAction = ActionFactory.Create(action);
        }
        public bool Use(ITargetable caster, ITargetable target) => Use(caster, new[] { target });
        public bool Use(ITargetable caster, IReadOnlyList<ITargetable> targets)
        {
            if (CurrentStack <= 0) return false;

            if (BoundAction.CanCast(caster, targets, ignoreCost: true))
            {
                BoundAction.Cast(caster, targets, ignoreCost: true);
                CurrentStack--;
                return true;
            }

            return false;
        }
    }
}
