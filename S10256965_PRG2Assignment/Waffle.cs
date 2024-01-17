//==========================================================
// Student Number : S10256965
// Student Name : Lee Jia Yu
// Partner Name : Yu Yi Lucas
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10256965_PRG2Assignment
{
    public class Waffle : IceCream
    {
        public string WaffleFlavour { get; set; }
        public Waffle() { }
        public Waffle(string option, int scoop, List<Flavour> flavours, List<Topping> toppings, string waffleFlavour) 
                      : base(option, scoop, flavours, toppings)
        {
            WaffleFlavour = waffleFlavour;
        }
        public override double CalculatePrice() 
        {
            // TODO
            return 0;
        }
        public override string ToString()
        {
            return base.ToString() + "Add-on: " + WaffleFlavour;
        }

    }
}
