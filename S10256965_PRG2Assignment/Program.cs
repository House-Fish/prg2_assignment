//==========================================================
// Student Number : S10256965
// Student Name : Lee Jia Yu
// Partner Name : Yu Yi Lucas
//==========================================================

using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Dynamic;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;

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

            while (true)
            {
                int option = Helper.GetOption("Enter the option", Helper.MainOptions, "Enter option");

                Console.WriteLine($"\n{option}) {Helper.MainOptions[option-1]}:");

                if (option == 1)
                {
                    DisplayAllCustomerDetails(customerDic);
                }
                else if (option == 2)
                {
                    DisplayCurrentOrders(regularQueue, goldQueue);
                }
                else if (option == 3)
                {
                    AddNewCustomer(customerDic);
                }
                else if (option == 4)
                {
                    CreateCustomerOrder(customerDic, regularQueue, goldQueue);
                }
                else if (option == 5)
                {
                    DisplayCustomersOrders(customerDic);
                }
                else if (option == 6)
                {
                    ModifyOrder(customerDic);
                }
                else if (option == 7)
                {
                    ProcessOrder(customerDic, regularQueue, goldQueue);
                }
                else if (option == 8)
                {
                    MonthlyBreakdown(customerDic);
                }
                else if (option == 9)
                {
                    Console.WriteLine("See you again, cowboy!");
                    break;
                }
                else
                {
                    Console.WriteLine("Error, option has not been processed.");
                }

                // Spacer
                Console.ReadLine();
            }
        }
        static void Init(Dictionary<int, Customer> customerDic)
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

            // Spacer
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
            string name;
            int id;
            DateTime dob;

            while (true)
            {
                Console.Write("Enter the name of the new customer: ");
                string? input = Console.ReadLine();

                if (input == null)
                {
                    Console.WriteLine("Invalid, name should not be null.");
                }
                else if (customerDic.Select(obj => obj.Value.Name).Contains(input))
                {
                    Console.WriteLine("Invalid, name is already in the system.");
                    Console.WriteLine("Try to enter your full name or an alias.");
                }
                else
                {
                    name = input;
                    break;
                }
            }

            while (true)
            {
                Console.Write("Enter the ID of the new customer: ");
                string? input = Console.ReadLine();

                if (input == null)
                {
                    Console.WriteLine("Invalid, id should not be null.");
                }
                else if (input.Length != 6)
                {
                    Console.WriteLine("Invalid, id should be 6 digits.");
                }
                else if (!int.TryParse(input, out id))
                {
                    Console.WriteLine("Invalid, id should be an integer.");
                }
                else if (customerDic.ContainsKey(id))
                {
                    Console.WriteLine("Invalid, id is already used in the system.");
                }
                else
                {
                    break;
                }
            }

            while (true)
            {
                Console.Write("Enter the Date of Birth of the new customer: ");
                string? input = Console.ReadLine();

                if (input == null)
                {
                    Console.WriteLine("Invalid, date of birth should not be null.");
                }
                else if (!DateTime.TryParseExact(input, "dd/MM/yyyy", null, DateTimeStyles.None, out dob))
                {
                    Console.WriteLine("Invalid, date of birth should be a valid date.");
                }
                else
                {
                    break;
                }
            }
            Customer customer = new Customer(name, Convert.ToInt32(id), dob);

            PointCard pointCard = new PointCard(0, 0);

            customer.Rewards = pointCard;

            customerDic.Add(id, customer);

            string text = $"{name},{id},{dob.ToString("dd/MM/yyyy")},Ordinary,0,0";
            File.AppendAllText("customers.csv", Environment.NewLine + text);

            Console.WriteLine("New customer added successfully.");
        }
        // 4) Create a customer's order
        static void CreateCustomerOrder(Dictionary<int, Customer> customerDic, Queue<Order> regularQueue, Queue<Order> goldQueue)
        {
            // displays the list of customers
            // finds the cutomer via their id in the list displayed
            // option is the value corresponding to the index in the customer dic - 1
            List<string> customerNames = customerDic.Select(kvp => kvp.Value.Name).ToList();
            int option = Helper.GetOption("Enter the customer you would like", customerNames.ToArray(),
                                          "Customer names");
  
            Queue<Order> orders = regularQueue;
            Customer customer = customerDic.Values.ElementAt(option - 1);

            Order order = customer.MakeOrder();

            if (customer.Rewards.Tier == "Gold")
            {
                orders = goldQueue;
            }

            orders.Enqueue(order);
            customer.CurrentOrder = order;

            Console.WriteLine("Your order is successful");
        }

        // 5) Display order details of a customer 
        static void DisplayCustomersOrders(Dictionary<int, Customer> customerDic)
        {
            // Get just the names of customers 
            string[] customerNames = customerDic.Select(kvp => kvp.Value.Name).ToArray();
            int option = Helper.GetOption("Enter the customer you would like", customerNames,
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
            // Get just the names of customers 
            string[] customerNames = customerDic.Select(kvp => kvp.Value.Name).ToArray();
            int customerIdx = Helper.GetOption("Enter the customer you would like", customerNames, 
                "Customer names");

            Customer customer = customerDic.ElementAt(customerIdx - 1).Value;

            // Get customers current order
            Order? currentOrder = customer.CurrentOrder;

            if (currentOrder == null)
            {
                Console.WriteLine("Invalid option, {0} does not have any current orders.", customer.Name);
                return;
            }

            // List all the ice cream objects contained in the order
            Console.WriteLine("Current order:\n" + currentOrder.ToString());

            // Prompt the user on their action
            int optionIdx = Helper.GetOption("Enter the action you would like to take", Helper.ModifyOptions, 
                "Possible actions");

            List<IceCream> iceCreamList = currentOrder.IceCreamList;

            // Modify ice cream
            if (optionIdx == 1)
            {
                string[] iceCreamDetails = iceCreamList.Select(icecream => icecream.ToString()).ToArray();
                int iceCreamIdx = Helper.GetOption("Enter the ice cream you would like to modify", 
                                                    iceCreamDetails, "Ice creams");

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
                if (iceCreamList.Count < 2)
                {
                    Console.WriteLine("Invalid option, you have to have a minimum of one ice cream");
                    return;
                }

                string[] iceCreamDetails = iceCreamList.Select(icecream => icecream.ToString()).ToArray();
                int iceCreamIdx = Helper.GetOption("Enter the ice cream you would like to delete",
                                                    iceCreamDetails, "Ice creams");

                currentOrder.DeleteIceCream(iceCreamIdx - 1);
            }

            Console.WriteLine("Updated order:\n" + currentOrder.ToString());
        }
        // 7) Process an order and checkout
        static void ProcessOrder(Dictionary<int, Customer> customerDic, Queue<Order> regularQueue, Queue<Order> goldQueue)
        {
            double totalAmount;

            // Process regular queue if gold queue is empty
            Queue<Order> queue = goldQueue.Count == 0 ? regularQueue : goldQueue;

            // Return if the queues are empty
            if (queue.Count == 0)
            {
                Console.WriteLine("There are no customers in the queue.");
                return;
            }
            
            Order order = queue.Dequeue();

            // Display all the ice creams in the order
            int idx = 1;
            Console.WriteLine("Items in the order: ");
            foreach (IceCream iceCream in order.IceCreamList)
            {
                Console.WriteLine("[" + idx + "] " + iceCream.ToString());
                idx++;
            }

            // Display the total bill amount
            totalAmount = order.CalculateTotal();
            Console.WriteLine("Total bill: " + totalAmount.ToString("0.00"));

            // Get the customer object which has the order id
            Customer? customer = customerDic.Values
                .FirstOrDefault(obj => obj?.CurrentOrder?.Id == order.Id);

            if (customer == null)
            {
                Console.WriteLine("Order is not linked to a customer.");
                return;
            }

            // Display the customers details
            Console.WriteLine("\nCustomer details:");
            Console.WriteLine("Name: " + customer.Name);
            Console.WriteLine("Membership status: " + customer.Rewards.Tier);
            Console.WriteLine("Points: " + customer.Rewards.Points);
            Console.WriteLine();

            // Deduct most expensive ice cream if today is birthday
            if (customer.IsBirthday())
            {
                IceCream? mostExpensiveIceCream = order.IceCreamList.Max();
                if (mostExpensiveIceCream == null)
                {
                    Console.WriteLine("Unable to locate most expensive ice cream.");
                }
                else
                {
                    totalAmount -= mostExpensiveIceCream.CalculatePrice();
                    Console.WriteLine("Happy Birthday! Discounting the most expensive ice cream.");
                }
            }

            // Deduct first ice cream if punch card full
            if (customer.Rewards.PunchCard == 10)
            {
                IceCream firstIceCream = order.IceCreamList[0];
                totalAmount -= firstIceCream.CalculatePrice();
                customer.Rewards.PunchCard = 0;
                Console.WriteLine("You have filled up your punch card! Discounting your first ice cream.");
            }

            // Redeem points if Silver or Gold member
            if (customer.Rewards.Tier == "Silver" || customer.Rewards.Tier == "Gold")
            {
                int noPoints;
                while (true)
                {
                    Console.Write("Enter how many points you would like to use: ");
                    if (!Int32.TryParse(Console.ReadLine(), out noPoints))
                    {
                        Console.WriteLine("Invalid, points should be a number");
                    }
                    else if (noPoints < 0 || noPoints > customer.Rewards.Points) 
                    {
                        Console.WriteLine("Invalid, the value should be between 0 and " +
                            $"{customer.Rewards.Points} points.");
                    }
                    else
                    {
                        break;
                    }
                }

                double discount = noPoints * 0.02;
                totalAmount -= discount;

                Console.WriteLine($"Discounted ${discount.ToString("0.00")}.");
            }

            Console.WriteLine("Final total bill: " + totalAmount.ToString("0.00"));

            Console.Write("Enter any key to make payment: ");
            Console.ReadLine();

            // Update punch card
            int punchCard = customer.Rewards.PunchCard;
            int noIceCreams = order.IceCreamList.Count;

            customer.Rewards.PunchCard = Math.Min(punchCard + noIceCreams, 10);

            // Earn points
            customer.Rewards.AddPoints(Convert.ToInt32(Math.Floor(totalAmount * 0.72)));

            // Update member status
            if (customer.Rewards.Points >= 100)
            {
                customer.Rewards.Tier = "Gold";
            }
            else if (customer.Rewards.Points >= 50)
            {
                customer.Rewards.Tier = "Silver";
            }

            order.TimeFulfilled = DateTime.Now;

            customer.CurrentOrder = null;
            customer.OrderHistory.Add(order);
        }
        // 8) Dsiplay monthly charged amounts and total charged amounts for the year
        static void MonthlyBreakdown(Dictionary<int, Customer> customerDic)
        {
            Console.Write("Enter the year: ");
            DateTime input = new DateTime(Convert.ToInt32(Console.ReadLine()), 1, 1);

            Dictionary<int, double> incomeDic = new Dictionary<int, double>();
            for (int i = 1; i < 13; i++)
            {
                incomeDic.Add(i, 0);
            }

            // find the year and month the order was finished \
            // two loops
            // first loop goes through all the customers and accesses the OrderHistory from the customerDic 
            // second loop selects the orders based off of the year the user input
            // append to incomeDic by month = key, order cost add to value by the key
            foreach( Customer customer in customerDic.Values ) {

                List<Order> orderList = customer.OrderHistory;
                foreach ( Order order in orderList )
                {
                    DateTime timeFulfilled = (DateTime)(order.TimeFulfilled);

                    if (timeFulfilled.Year == input.Year)
                    {
                        incomeDic[timeFulfilled.Month] += order.CalculateTotal();
                    }
                }
            }

            foreach (KeyValuePair<int, double> kvp in incomeDic ) 
            { 
                Console.WriteLine(kvp.Key.ToString() + ": " + kvp.Value.ToString());
            }

        }
    }
}
