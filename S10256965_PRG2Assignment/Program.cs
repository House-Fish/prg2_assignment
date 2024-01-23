//==========================================================
// Student Number : S10256965
// Student Name : Lee Jia Yu
// Partner Name : Yu Yi Lucas
//==========================================================
using System.ComponentModel.Design;
using System.Dynamic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace S10256965_PRG2Assignment
{
    public class Program
    {
        static void Main()
        {
            Dictionary<int, Customer> customerDic = new Dictionary<int, Customer>();
            Queue<Order> regularQueue = new Queue<Order>();
            Queue<Order> goldQueue = new Queue<Order>();

            Init(customerDic);

            // Init random 
            Random random = new Random();

            // 1) List all customers
            // DisplayAllCustomerDetails(customerDic);

            // 2) List all current orders of gold and ordinary members
            // Test Case 
            /*
            int noRandom = random.Next(10);
            for (int i = 0; i < noRandom; i++)
            {
                regularQueue.Enqueue(Helper.CreateRandomOrder());
            }
            noRandom = random.Next(10);
            for (int i = 0; i < noRandom; i++)
            {
                goldQueue.Enqueue(Helper.CreateRandomOrder());
            }
            */
            // DisplayCurrentOrders(regularQueue, goldQueue);

            // 3) Register a new customer
            // AddNewCustomer(customerDic);

            // 5) Display order details of a customer
            DisplayCustomersOrders(customerDic);

            // 6) Modify order details
            // Test Case 
            /*          
            Customer customer = customerDic.ElementAt(0).Value;
            customer.CurrentOrder = Helper.CreateRandomOrder();
            */
            // ModifyOrder(customerDic);

        }
        static void Init(Dictionary <int, Customer > customerDic)
        {
            string customerFilePath = "customers.csv";
            string orderFilePath = "orders.csv";

            Helper.TryInitializeCustomers(customerDic, customerFilePath);
            Helper.TryInitalizeOrders(customerDic, orderFilePath);
        }
        // 1) List all customers
        static void DisplayAllCustomerDetails(Dictionary<int, Customer> customerDic)
        {
            foreach (KeyValuePair<int, Customer> kvp in customerDic)
            {
                Console.WriteLine(kvp.Value.ToString());
            }
        }
        // 2) List all current orders
        static void DisplayCurrentOrders(Queue<Order> regularQueue, Queue<Order> goldQueue)
        {
            int idx = 1;
            if (regularQueue.Count == 0) 
            {
                Console.WriteLine("Regular queue is empty.");
            }
            else
            {
                Console.WriteLine("Regular queue: ");
                foreach (Order order in regularQueue)
                {
                    Console.WriteLine("[{0}] {1}", idx, order.ToString());
                    idx++;
                }
            }

            Console.WriteLine();

            idx = 1;
            if (goldQueue.Count == 0)
            {
                Console.WriteLine("Gold queue is empty.");
            }
            else
            {
                Console.WriteLine("Gold queue: ");
                foreach (Order order in goldQueue)
                {
                    Console.WriteLine("[{0}] {1}", idx, order.ToString());
                    idx++;
                }
            }
        }
        // 3) Register a new customer
        static void AddNewCustomer(Dictionary<int, Customer> customerDic)
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

            Int32.TryParse(id, out int memberId);
            customerDic.Add(memberId, customer);

            File.AppendAllText("customers.csv", Environment.NewLine + name + "," + id + "," + dob.ToString("dd/mm/yyyy"));

            Console.WriteLine("New customer added successfully.");
        }
        // 5) Display order details of a customer 
        static void DisplayCustomersOrders(Dictionary<int, Customer> customerDic)
        {
            List<string> customerNames = customerDic.Select(kvp => kvp.Value.Name).ToList();
            int option = Helper.GetOption("Enter the customer you would like", customerNames.ToArray(),
                                          "Customer names");

            Customer customer = customerDic.ElementAt(option - 1).Value;

            Console.WriteLine();

            // Current orders 
            if (customer.CurrentOrder != null)
            {
                Console.WriteLine("Current order: \n" + customer.CurrentOrder.ToString());
            }
            else
            {
                Console.WriteLine("There is no current order.");
            }

            Console.WriteLine();

            // Past orders
            int idx = 1;

            if (customer.OrderHistory != null)
            {
                Console.WriteLine("Past orders: ");
                foreach (Order order in customer.OrderHistory)
                {
                    Console.WriteLine("[{0}] {1}", idx, order.ToString());
                    idx++;
                }
            }
            else
            {
                Console.WriteLine("There are no past orders.");
            }
        }
        // 6) Modify order details 
        static void ModifyOrder(Dictionary<int, Customer> customerDic)
        {
            // List & prompt user to select customer
            List<string> customerNames = customerDic.Select(kvp => kvp.Value.Name).ToList();
            int customerIdx = Helper.GetOption("Enter the customer you would like", customerNames.ToArray(), 
                "Customer names");

            Customer customer = customerDic.ElementAt(customerIdx - 1).Value;

            // Get customers current order
            Order currentOrder = customer.CurrentOrder;

            // List all the ice creams in the current order
            if (currentOrder == null)
            {
                Console.WriteLine("Invalid option, {0} does not have any current orders.", customer.Name);
                return;
            }

            Console.WriteLine("Current order:\n" + currentOrder.ToString());

            // List & prompt user to select modify option
            int optionIdx = Helper.GetOption("Enter the action you would like to take", Helper.ModifyOptions, 
                "Possible actions");

            List<IceCream> iceCreams = currentOrder.IceCreamList;

            // Modify ice cream
            if (optionIdx == 1)
            {
                List<string> iceCreamDetails = iceCreams.Select(icecream => icecream.ToString()).ToList();
                int iceCreamIdx = Helper.GetOption("Enter the ice cream you would like to modify", 
                                                    iceCreamDetails.ToArray(), "Ice creams");

                currentOrder.ModifyIceCream(iceCreamIdx - 1);
            }
            // Add new ice cream
            else if (optionIdx == 2)
            {
                IceCreamBuilder builder = new IceCreamBuilder();

                currentOrder.AddIceCream(builder.GetIceCream());
            }
            // Delete ice cream
            else
            {
                if (iceCreams.Count < 2)
                {
                    Console.WriteLine("Invalid option, you have to have a minimum of one ice cream");
                    return;
                }

                List<string> iceCreamDetails = iceCreams.Select(icecream => icecream.ToString()).ToList();
                int iceCreamIdx = Helper.GetOption("Enter the ice cream you would like to modify",
                                                    iceCreamDetails.ToArray(), "Ice creams");

                currentOrder.DeleteIceCream(iceCreamIdx - 1);
            }

            Console.WriteLine("Updated order:\n" + currentOrder.ToString());
        }
    }
}
