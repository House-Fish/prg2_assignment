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
    internal class Order
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
        }
        public void ModifyIceCream(int id) { }
        public void AddIceCream(IceCream iceCream) { }
        public void DeleteIceCream(int id) { }
        public double CalculateTotal() 
        {
            // TODO
            return 0;
        }
        public override string ToString()
        {
            string icecreams = "";
            int num = 1;

            foreach (IceCream icecream in IceCreamList)
            {
                icecreams += String.Format("\n{0}. {1}", num, icecream.ToString());
                num++;
            }
            return String.Format("Id: {0}, Time Recived: {1}, Ice Creams: {2}", 
                                 Id, TimeReceived.ToString(), icecreams);
        }

    }
}
