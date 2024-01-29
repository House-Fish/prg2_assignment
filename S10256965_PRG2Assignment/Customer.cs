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
    public class Customer
    {
        public string Name { get; set; }
        public int MemberId { get; set; }
        public DateTime Dob { get; set; }
        public Order? CurrentOrder { get; set; }
        public List<Order> OrderHistory = new List<Order>();
        public PointCard Rewards { get; set; }
        public Customer() { }
        public Customer(string name, int memberId, DateTime dob)
        {
            Name = name;
            MemberId = memberId;
            Dob = dob;
        }
        public Order MakeOrder() 
        {
            DateTime now = DateTime.Now;
            int id = now.Year * 10000 + now.Day * 1000 + now.Second * 100 + now.Microsecond;

            Order order = new Order(id, now);

            while (true)
            {
                IceCreamBuilder builder = new IceCreamBuilder();
                order.AddIceCream(builder.GetIceCream());
                Console.WriteLine("Would you like to add another ice cream to your order? [Y/N]");
                string input = Console.ReadLine();
                if (input == "N"){
                    break;
                }
            }

            return order;
        }
        public bool IsBirthday() 
        {
            return DateTime.Today.Month == Dob.Month && DateTime.Today.Day == Dob.Day;
        }
        public override string ToString()
        {
            return String.Format("Name: {0}, Member Id: {1}, Date of birth: {2}", 
                                 Name, MemberId, Dob.ToString("dd/mm/yyyy")); 
        }
    }
}
