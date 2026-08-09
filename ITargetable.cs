using HeadlessCore.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessCore
{
    public interface ITargetable
    {
        Stats TotalStats { get; }
        void TakeDamage(int amount);
        void Heal(int amount);
        void RestoreSP(int amount);
        bool ConsumeSP(int amount);
    }
}
