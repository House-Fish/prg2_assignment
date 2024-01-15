//==========================================================
// Student Number : S10256965
// Student Name : Lee Jia Yu
// Partner Name : Yu Yi Lucas
//==========================================================
using System.Security.Cryptography;

namespace S10256965_PRG2Assignment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, Customer> customerDic = new Dictionary<string, Customer>();
            Init(customerDic);

            // 1) List all customers
            DisplayAll(customerDic);

            // 2) List all current orders of gold and ordinary members
            DisplayGoldAndOrdinaryOrders(customerDic);

            // 3) Register a new customer
            AddNewCustomer(customerDic);

        }
        static void DisplayAll(Dictionary<string, Customer> customerDic)
        {
            foreach (KeyValuePair<string, Customer> kvp in customerDic)
            {
                Console.WriteLine(kvp.Value.ToString());
            }
        }
        static void Init(Dictionary <string, Customer > customerDic)
        {
            using (StreamReader sr = new StreamReader("customers.csv"))
            {
                string line = sr.ReadLine();

                while ((line = sr.ReadLine()) != null)
                {
                    string[] data = line.Split(',');
                    //Amy,685582,12/03/2000
                    Customer obj = new Customer(data[0], Convert.ToInt32(data[1]), DateTime.Parse(data[2]));
                    customerDic.Add(data[1], obj);
                }
            }

        }
        static void AddNewCustomer(Dictionary<string, Customer> customerDic)
        {
            Console.Write("Enter the name of the new customer: ");
            string name = Console.ReadLine();

            Console.Write("Enter the ID of the new customer: ");
            string id = Console.ReadLine();

            Console.Write("Enter the Date of Birth of the new customer: ");
            DateTime dob = DateTime.Parse(Console.ReadLine());

            Customer customer = new Customer(name, Convert.ToInt32(id), dob);

            PointCard pointCard = new PointCard(0, 0);

            customer.Rewards = pointCard;

            customerDic.Add(id, customer);

            File.AppendAllText("customers.csv", Environment.NewLine + name + "," + id + "," + dob.ToString("dd/mm/yyyy"));

            Console.WriteLine("New customer added successfully.");
        }
        static void DisplayGoldAndOrdinaryOrders(Dictionary<string, Customer> customerDic)
        {
            foreach (KeyValuePair<string, Customer> pair in customerDic)
            {
                Customer customer = pair.Value;
                if (customer.Rewards.Tier == "Gold" ||  customer.Rewards.Tier == "Ordinary")
                {
                    Console.WriteLine(customer.CurrentOrder.ToString());
                }
            }
        }
    }
}
