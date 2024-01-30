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
        public string? WaffleFlavour { get; set; }
        public Waffle() { }
        public Waffle(string option, int scoop, List<Flavour> flavours, List<Topping> toppings, string? waffleFlavour) 
                      : base(option, scoop, flavours, toppings)
        {
            WaffleFlavour = waffleFlavour;
        }
        public override double CalculatePrice() 
        {
            // Calculate the base price based on the number of scoops
            double price; 
            if (Scoop == 0)
            {
                price = 2;
            }
            else if (Scoop > 1)
            {
                price = 6.50 + Scoop;
            }
            else
            {
                price = 7.00;
            }

            // Add on the premium flavours cost
            foreach (Flavour f in Flavours)
            {
                if (f.Premium) { price += f.Quantity * 2; }
            }

            // Add on the price of the flavoured waffle
            price = WaffleFlavour != null ? price += 3 : price;

            // Add the cost of the toppings
            return price + Toppings.Count;        
        }
        public override string ToString()
        {
            return base.ToString() + "Add-on: " + WaffleFlavour;
        }

    }
}
