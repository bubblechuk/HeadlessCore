using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessCore
{
    public struct Stats
    {
        private const int minLvl = 0;
        private const int maxLvl = 99;

        public int Caliber = 0;
        public int Spirituality = 0;
        public int Tinkering = 0;
        public int Resonance = 0;
        public int Willpower = 0;
        public Stats(int caliber, int spirituality, int tinkering, int resonance, int willpower) {
            Caliber = Math.Clamp(caliber, minLvl, maxLvl);
            Spirituality = Math.Clamp(spirituality, minLvl, maxLvl);
            Tinkering = Math.Clamp(tinkering, minLvl, maxLvl);
            Resonance = Math.Clamp(resonance, minLvl, maxLvl);
            Willpower = Math.Clamp(willpower, minLvl, maxLvl);
        }
        public static Stats operator+(Stats a, Stats b)
        {
            return new Stats(a.Caliber + b.Caliber, 
                             a.Spirituality + b.Spirituality,
                             a.Tinkering + b.Tinkering,
                             a.Resonance + b.Resonance, 
                             a.Willpower + b.Willpower);
        }
        public static Stats operator -(Stats a, Stats b)
        {
            return new Stats(a.Caliber - b.Caliber,
                             a.Spirituality - b.Spirituality,
                             a.Tinkering - b.Tinkering,
                             a.Resonance - b.Resonance,
                             a.Willpower - b.Willpower);
        }
    }
}
