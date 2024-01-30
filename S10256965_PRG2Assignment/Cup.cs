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
    public class Cup : IceCream
    {
        public Cup() { }
        public Cup(string option, int scoop, List<Flavour> flavours, List<Topping> toppings) 
                   : base(option, scoop, flavours, toppings) { }
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
                price = 3.50 + Scoop;
            }
            else
            {
                price = 4.00;
            }

            // Add on the premium flavours cost
            foreach (Flavour f in Flavours)
            {
                if (f.Premium) { price += f.Quantity * 2; }
            }

            // Add the cost of the toppings
            return price + Toppings.Count;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
