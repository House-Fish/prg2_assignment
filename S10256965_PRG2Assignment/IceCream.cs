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
    public abstract class IceCream : IComparable<IceCream>
    {
        public string Option = "Cup"; 
        public int Scoop { get; set; } 
        public List<Flavour> Flavours = new List<Flavour>(); 
        public List<Topping> Toppings = new List<Topping>();
        public IceCream() { }
        public IceCream(string option, int scoop, List<Flavour> flavours, List<Topping> toppings) 
        { 
            Option = option;
            Scoop = scoop;
            Flavours = flavours;
            Toppings = toppings;
        }
        public abstract double CalculatePrice();
        public int CompareTo(IceCream oIceCream)
        {
            return CalculatePrice().CompareTo(oIceCream.CalculatePrice());
        }
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
