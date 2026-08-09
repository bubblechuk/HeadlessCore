using HeadlessCore.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessCore.Items
{
    public interface IConsumable
    {
        bool Use(CBaseEntity target);
    }
}
