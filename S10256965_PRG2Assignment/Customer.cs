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
    internal class Customer
    {
        public string Name { get; set; }
        public int MemberId { get; set; }
        public DateTime Dob { get; set; }
        public Order CurrentOrder { get; set; }
        public List<Order> OrderHistory { get; set; }
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
            // TODO
            return new Order();
        }
        public bool IsBirthday() 
        {
            // TODO
            return false;
        }
        public override string ToString()
        {
            return String.Format("Name: {0}, Member Id: {1}, Date of birth: {2}", 
                                 Name, MemberId, Dob.ToString("dd/mm/yyyy")); 
        }
    }
}
