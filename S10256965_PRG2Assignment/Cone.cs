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
    internal class Cone : IceCream
    {
        public bool Dipped { get; set; }
        public Cone() { }
        public Cone(string option, int scoop, List<Flavour> flavours, List<Topping> toppings, bool dipped)
                    : base(option, scoop, flavours, toppings)
        {
            Dipped = dipped;
        }
        public override double CalculatePrice() 
        {
            // TODO
            return 0;
        }
        public override string ToString()
        {
            return "cup";
        }
    }
}
