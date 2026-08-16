using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessCore.Effects
{
    internal class PoisonEffect : CStatusEffect
    {
        public int HpDrain { get; }
        public int SpDrain { get; }
        public int XpDrain { get; }
        public PoisonEffect(string id, int turns, int hpDrain, int spDrain, int xpDrain)
        {
            Id = id;
            RemainingTurns = turns;
            HpDrain = hpDrain;
            SpDrain = spDrain;
            XpDrain = xpDrain;
        }
        public override void OnTurnTick(ITargetable target)
        {
            if (HpDrain > 0) target.TakeDamage(HpDrain);
            if (SpDrain > 0) target.ConsumeSP(SpDrain);
            if (IsPermanent) return;
            else 
            {
                RemainingTurns--;
                if (RemainingTurns == 0)
                {
                    IsExpired = true;
                    return;
                }
            }
        }
    }
}
