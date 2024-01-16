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
            Queue<Order> goldQueue = new Queue<Order>();
            Queue<Order> regularQueue = new Queue<Order>();

            Init(customerDic);

            // 1) List all customers
            // DisplayAllCustomerDetails(customerDic);

            // 2) List all current orders of gold and ordinary members
            // DisplayGoldAndOrdinaryOrders(customerDic);

            // 3) Register a new customer
            // AddNewCustomer(customerDic);

            // 5) Display order details of a customer
            // DisplayCustomersOrders(customerDic);
            Console.WriteLine(CreateIceCream().ToString());
        }
        static void DisplayAllCustomerDetails(Dictionary<string, Customer> customerDic)
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
        static void DisplayCurrentOrders(Queue<Order> regularQueue, Queue<Order> goldQueue)
        {
            int idx = 1;
            Console.WriteLine("Regular queue: ");
            foreach(Order order in regularQueue)
            {
                Console.WriteLine("[{0}] {1}", idx, order.ToString());
                idx++;
            }

            idx = 1;
            Console.WriteLine("Gold queue: ");
            foreach(Order order in goldQueue)
            {
                Console.WriteLine("[{0}] {1}", idx, order.ToString());
                idx++;
            }
        }
        static void DisplayMenu(string[] options, string title)
        {
            int idx = 1;
            Console.WriteLine("{0}:", title);
            foreach (string option in options)
            {
                Console.WriteLine("[{0}] {1}", idx, option);
                idx++;
            }
        }
        static IceCream CreateIceCream()
        {
            string[] options = ["Cup", "Cone", "Waffle"];
            string[] flavours = ["Chocolate", "Vanilla", "Strawberry", "Durian", "Ube", "Sea salt"];
            string[] premiumFlavours = ["Durian", "Ube", "Sea salt"];
            string[] toppings = ["Sprinkles", "Mochi", "Sago", "Oreos"];
            string[] waffleFlavours = ["Red velvet", "Charcoal", "Pandan Waffle"];

            // Get how the ice cream is served (Cup, Cone, or Waffle)
            string servingOption;
            while (true)
            {
                DisplayMenu(options, "Serving options");
               
                Console.Write("Enter how you would like your ice cream served: ");
                string? option = Console.ReadLine();
                
                if (options.Contains(option))
                {
                    servingOption = option;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option, please enter an option in the list.");
                }
            }

            // Get the preferred flavours (Vanilla, Chocolate, Strawberry, Durian, Ube, or Sea salt)
            List<Flavour> flavoursList = new List<Flavour>();
            const int maxNoFlavours = 3;
            int noFlavours = 0;
            bool skip = false;

            while (!skip)
            {
                DisplayMenu(flavours, "Flavour options");
                Console.WriteLine("You can choose a maximum of {0} flavours, enter 'c' if you would like to " +
                                  "continue to the toppings.", maxNoFlavours);

                for (; noFlavours < maxNoFlavours; noFlavours++)
                {
                    Console.Write("Enter flavour ({0} left): ", maxNoFlavours - noFlavours);
                    string? flavour = Console.ReadLine();

                    if (flavour == "c")
                    {
                        Console.WriteLine("Continuing...");
                        skip = true;
                        break;
                    }
                    else if (!flavours.Contains(flavour))
                    {
                        Console.WriteLine("Invalid flavour, please enter it again.");
                        break;
                    }

                    if (flavoursList.Count > 0)
                    {
                        foreach (Flavour fla in flavoursList)
                        {
                            if (fla.Type == flavour) { fla.Quantity++; }
                        }
                    }
                    bool premium = premiumFlavours.Contains(flavour) ? true : false;
                    flavoursList.Add(new Flavour(flavour, premium, 1));
                }
                if (skip || maxNoFlavours == noFlavours) { break; }
            }

            // Get the prefferd toppings (Sprinkles, Mochi, Sago, Oreos)
            List<Topping> toppingsList = new List<Topping>();
            const int maxNoToppings = 4;
            int noToppings = 0;
            skip = false;

            while (!skip)
            {
                DisplayMenu(toppings, "Topping options");
                Console.WriteLine("You can choose a maximum of {0} toppings, enter 'c' if you would like to " +
                                  "continue to the extras.", maxNoToppings);

                for (; noToppings < maxNoToppings; noToppings++)
                {
                    Console.Write("Enter flavour ({0} left): ", maxNoToppings - noToppings);
                    string? topping = Console.ReadLine();

                    if (topping == "c")
                    {
                        Console.WriteLine("Continuing...");
                        skip = true;
                        break;
                    }
                    else if (!toppings.Contains(topping))
                    {
                        Console.WriteLine("Invalid topping, please enter it again.");
                        break;
                    }

                    toppingsList.Add(new Topping(topping));
                }
                if (skip || maxNoFlavours == noFlavours) { break; }
            }

            // Create objects and add add-ons
            IceCream iceCream = new Cup("Cup", flavoursList.Count, flavoursList, toppingsList);
            if (servingOption == "Cone")
            {
                bool dip;

                while (true)
                {
                    Console.Write("Would you like a chocolate-dipped cone (Y/N): ");
                    string dipped = Console.ReadLine();

                    if (dipped == "Y")
                    {
                        dip = true;
                        break;
                    }
                    else if (dipped == "N")
                    {
                        dip = false;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid option, enter 'Y' or 'N'.");
                    }
                }
                iceCream = new Cone("Cone", flavoursList.Count, flavoursList, toppingsList,
                                             dip);
            }
            else
            {
                string waffleFlavour = "";
                while (true)
                {
                    Console.Write("Would you like a flavoured waffle (Y/N): ");
                    string wantFlavourWaffle = Console.ReadLine();

                    if (wantFlavourWaffle == "Y")
                    {
                        while (true)
                        {
                            DisplayMenu(waffleFlavours, "Waffle flavour options");

                            Console.Write("Enter flavour: ");
                            string waffleFlav = Console.ReadLine();

                            if (waffleFlavours.Contains(waffleFlav))
                            {
                                waffleFlavour = waffleFlav;
                                break;
                            }
                            Console.WriteLine("Invalid topping, please enter it again.");
                        }
                        break;
                    }
                    else if (wantFlavourWaffle == "N")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid option, enter 'Y' or 'N'.");
                    }
                }
                iceCream = new Waffle("Waffle", toppingsList.Count, flavoursList, toppingsList,
                                                waffleFlavour);
            }
            return iceCream;
        }
        static void DisplayCustomersOrders(Dictionary<string, Customer> customerDic)
        {
            DisplayAllCustomers(customerDic);

            Console.Write("Enter a customer: ");
            int option = Convert.ToInt32(Console.ReadLine()); // Error Validation

            Customer customer = customerDic.ElementAt(option).Value;

            Console.WriteLine("Current order: ");
            Console.WriteLine(customer.CurrentOrder.ToString());

            Console.WriteLine("Past orders: ");
            foreach (Order order in customer.OrderHistory)
            {
                Console.WriteLine(order.ToString());
            }
        }
        static void DisplayAllCustomers(Dictionary<string, Customer> customerDic)
        {
            int idx = 1;

            Console.WriteLine("All the customers: ");
            foreach (KeyValuePair<string, Customer> pair in customerDic)
            {
                Customer customer = pair.Value;
                Console.WriteLine("[{0}] {1}", idx, customer.Name);
                idx++;
            }
        }
    }
}
