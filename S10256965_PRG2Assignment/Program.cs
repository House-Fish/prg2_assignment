﻿//==========================================================
// Student Number : S10256965
// Student Name : Lee Jia Yu
// Partner Name : Yu Yi Lucas
//==========================================================

using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Dynamic;
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

            // 4) Create a customer's order
            // CreateCustomerOrder(customerDic, regularQueue, goldQueue);

            // 5) Display order details of a customer
            // DisplayCustomersOrders(customerDic);

            // 6) Modify order details
            // Test Case 
            /*          
            Customer customer = customerDic.ElementAt(0).Value;
            customer.CurrentOrder = Helper.CreateRandomOrder();
            */
            // ModifyOrder(customerDic);

            // 7) Process an order and checkout
            // Test cases
            /*
            while (true)
            {
                Helper.AddRandomOrder(customerDic, goldQueue, regularQueue);
                ProcessOrder(customerDic, regularQueue, goldQueue);
            }
            */
            // ProcessOrder(customerDic, regularQueue, goldQueue);
            MonthlyBreakdown(customerDic);
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
            // handle customers that already exist
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

            File.AppendAllText("customers.csv", Environment.NewLine + name + "," + id + "," + dob.ToString("dd/mm/yyyy") + ", Ordinary, 0, 0");

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
            if (customer.Rewards.Tier == "Gold")
            {
                orders = goldQueue;
            }
            // id in this case is the position in the queue

            Order order = new Order(orders.Count + 1, DateTime.Now);
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
            orders.Enqueue(order);
            Console.WriteLine("Your order is successfull");

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
        // 7) Process an order and checkout
        static void ProcessOrder(Dictionary<int, Customer> customerDic, Queue<Order> regularQueue, Queue<Order> goldQueue)
        {
            double totalAmount;

            // Only go through regular queue if the gold queue is empty
            Queue<Order> queue = goldQueue.Count == 0 ? regularQueue : goldQueue;

            Order order = queue.Dequeue();

            int idx = 1;
            Console.WriteLine("Items in the order: ");
            foreach (IceCream iceCream in order.IceCreamList)
            {
                Console.WriteLine("[" + idx + "] " + iceCream.ToString());
                idx++;
            }

            totalAmount = order.CalculateTotal();
            Console.WriteLine("Total bill: " + totalAmount.ToString("0.00"));

            Customer? customer = customerDic.Values
                .FirstOrDefault(obj => obj?.CurrentOrder?.Id == order.Id);

            if (customer == null)
            {
                Console.WriteLine("Order is not linked to a customer.");
                return;
            }

            Console.WriteLine("Customer details:");
            Console.WriteLine("Name: " + customer.Name);
            Console.WriteLine("Membership status: " + customer.Rewards.Tier);
            Console.WriteLine("Points: " + customer.Rewards.Points);

            if (customer.IsBirthday())
            {
                IceCream expensiveIceCream = order.IceCreamList.Max();
                totalAmount -= expensiveIceCream.CalculatePrice();
                Console.WriteLine("Happy Birthday! Discounting the most expensive ice cream.");
            }

            if (customer.Rewards.PunchCard == 10)
            {
                IceCream firstIceCream = order.IceCreamList[0];
                totalAmount -= firstIceCream.CalculatePrice();
                customer.Rewards.PunchCard = 0;
                Console.WriteLine("You have filled up your punch card! Discounting your first ice cream.");
            }

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

            int punchCard = customer.Rewards.PunchCard;
            int addition = order.IceCreamList.Count;
            customer.Rewards.PunchCard = Math.Min(punchCard + addition, 10);

            customer.Rewards.AddPoints((int)Math.Floor(totalAmount * 0.72));

            if (customer.Rewards.Points >= 100)
            {
                customer.Rewards.Tier = "Gold";
            }
            else if (customer.Rewards.Points >= 50)
            {
                customer.Rewards.Tier = "Silver";
            }

            order.TimeFulfilled = DateTime.Now;

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
