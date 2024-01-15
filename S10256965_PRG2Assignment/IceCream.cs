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
    internal abstract class IceCream
    {
        public string Option { get; set; }
        public int Scoop { get; set; }
        public List<Flavour> Flavours { get; set; }
        public List<Topping> Toppings { get; set; }
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
            return "Ice Cream";
        }
    }
}
