//==========================================================
// Student Number : S10256965
// Student Name : Lee Jia Yu
// Partner Name : Yu Yi Lucas
//==========================================================

using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10256965_PRG2Assignment
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime TimeReceived { get; set; }
        public DateTime? TimeFulfilled { get; set; }
        public List<IceCream> IceCreamList { get; set; }
        public Order() { }
        public Order(int id, DateTime timereceived)
        {
            Id = id;
            TimeReceived = timereceived;
            IceCreamList = new List<IceCream>();
        }
        public void ModifyIceCream(int id) 
        {
            IceCream iceCream = IceCreamList[id];

            while (true)
            {
                Console.WriteLine("Current ice cream:\n" + iceCream.ToString());

                Helper.DisplayMenu(Helper.ModifyIceCream, "Modify ice cream");

                Console.WriteLine("Enter 'q' when you are done modifying the ice cream.");

                Console.Write("Enter the modification option: ");
                string holdModifyOption = Console.ReadLine();

                if (holdModifyOption == "q")
                {
                    break;
                }
                if (holdModifyOption == null || !Helper.IsValidOption(holdModifyOption, Helper.ModifyIceCream))
                {
                    continue;
                }

                int modifyOption = Convert.ToInt32(holdModifyOption);

                // Change option & select add-on
                if (modifyOption == 1)
                {
                    string changeOption = Helper.CreateOption();
                    if (changeOption == iceCream.Option)
                    {
                        Console.WriteLine("No change made.");
                        continue;
                    }
                    else if (changeOption == "Cup")
                    {
                        iceCream = Helper.CreateIceCreamCup(iceCream.Flavours, iceCream.Toppings);
                    }
                    else if (changeOption == "Cone")
                    {
                        iceCream = Helper.CreateIceCreamCone(iceCream.Flavours, iceCream.Toppings);
                    }
                    else 
                    {
                        iceCream = Helper.CreateIceCreamWaffle(iceCream.Flavours, iceCream.Toppings);
                    }
                }
                // Change flavours
                else if (modifyOption == 2)
                {
                    iceCream.Flavours = new List<Flavour>();
                    Helper.CreateFlavours(iceCream.Flavours);
                }
                // Change toppings
                else if (modifyOption == 3)
                {
                    iceCream.Toppings = new List<Topping>();
                    Helper.CreateToppings(iceCream.Toppings);
                }
                // Change add-on for Cone
                else if (iceCream is Cone)
                {
                    iceCream = Helper.CreateIceCreamCone(iceCream.Flavours, iceCream.Toppings);
                }
                // Change add-on for Waffle
                else
                {
                    iceCream = Helper.CreateIceCreamWaffle(iceCream.Flavours, iceCream.Toppings);
                }
            }
        }
        public void AddIceCream(IceCream iceCream) 
        { 
            IceCreamList.Add(iceCream);
        }
        public void DeleteIceCream(int id) 
        {
            IceCreamList.RemoveAt(id);
        }
        public double CalculateTotal() 
        {
            double total = 0;
            foreach (IceCream ic in IceCreamList)
            {
                total += ic.CalculatePrice();
            }
            return total;
        }
        public override string ToString()
        {
            string icecreams = "";
            int num = 1;

            foreach (IceCream icecream in IceCreamList)
            {
                icecreams += String.Format("\n\t{0}. {1}", num, icecream.ToString());
                num++;
            }
            return String.Format("Id: {0}, Time Recived: {1}, Time fulfilled: {2}, Ice Creams: {3}", 
                                 Id, TimeReceived.ToString(), TimeFulfilled.ToString(), icecreams);
        }

    }
}
