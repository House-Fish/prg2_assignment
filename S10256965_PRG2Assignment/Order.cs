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
        public List<IceCream> IceCreamList = new List<IceCream>();
        public Order() { }
        public Order(int id, DateTime timereceived)
        {
            Id = id;
            TimeReceived = timereceived;
        }
        public void ModifyIceCream(int id) 
        {
            IceCreamList[id] = Helper.ModifyIceCream(IceCreamList[id]);
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
