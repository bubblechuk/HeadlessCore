using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessCore.Effects
{
    public enum StatType
    {
        HP,
        SP,
        XP,
        Caliber,
        Spirituality,
        Tinkering,
        Resonance,
        Willpower
}
    public enum EffectType
    {
        FlatAmount,
        PercentAmount,
        ContiniousAmount
    }
    public interface IEffect
    {
        StatType TargetStat { get; }
        EffectType Type { get; }
        public int Value { get; }
        public int DurationTurns { get; }
    }
}
