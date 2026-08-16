using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessCore.Effects
{
    internal class StatModifierEffect : CStatusEffect
    {
        private readonly Stats _statModifier;
        public StatModifierEffect(string id, Stats statModifier, int turns)
        {
            Id = id;
            _statModifier = statModifier;
            RemainingTurns = turns;
        }
        public override Stats GetStatModifier() => _statModifier;
        public override void OnTurnTick(ITargetable target)
        {
            if (IsPermanent) return;
            RemainingTurns--;
            if (RemainingTurns == 0)
            {
                IsExpired = true;
            }
        }
    }
}
