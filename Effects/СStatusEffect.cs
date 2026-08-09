using HeadlessCore.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessCore.Effects
{
    public abstract class CStatusEffect
    {
        public string Id { get; }
        public int RemainingTurns { get; protected set; }
        public virtual Stats GetStatModifier() => Stats.Zero;

        public virtual void OnApply(ITargetable target) { }
        public virtual void OnRemove(ITargetable target) { }
        public virtual void OnTurnTick(ITargetable target) { }
    }
}
