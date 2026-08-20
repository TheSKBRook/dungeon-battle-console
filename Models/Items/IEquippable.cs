using ConsoleApp1.Models.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models.Items
{
    internal interface IEquippable
    {
        void Equip(Hero hero);
    }
}
