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
    public abstract class IceCream
    {
        public string Option { get; set; } // Cup, Cone or Waffle
        public int Scoop { get; set; } // Single, Double or Triple
        public List<Flavour> Flavours = new List<Flavour>(); // 3 from Regular or Premium Flavour
        public List<Topping> Toppings = new List<Topping>();// 4 from Toppings
        public IceCream() { }
        public IceCream(string option, int scoop, List<Flavour> flavours, List<Topping> toppings) 
        { 
            Option = option;
            Scoop = scoop;
            Flavours = flavours;
            Toppings = toppings;
        }
        public abstract double CalculatePrice();
        public override string ToString()
        {
            string message = "Flavours: ";
            foreach(Flavour flavour in Flavours)
            {
                message += flavour.ToString() + " ";
            }
            message += "Toppings: ";
            foreach(Topping topping in Toppings)
            {
                message += topping.ToString() + " ";
            }
            return Option + ": " + message;
        }
    }
}
