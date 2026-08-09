using HeadlessCore.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessCore
{
    public interface ITarget
    {
        void ApplyEffect(IEffect effect);
    }
}
