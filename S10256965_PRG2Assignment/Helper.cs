//==========================================================
// Student Number : S10256965
// Student Name : Lee Jia Yu
// Partner Name : Yu Yi Lucas
//==========================================================

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace S10256965_PRG2Assignment
{
    public static class Helper
    {
        // Option 6 
        public static string[] ModifyOptions = ["Modify existing ice cream", "Add new ice cream", "Delete existing ice cream"];
        public static string[] ModifyIceCreamOptions = ["Option", "Flavour", "Topping", "Add-on's", "Exit"];

        // Main options 
        public static string[] MainOptions = ["List all customers",
            "List all current orders of gold and ordinary members",
            "Register a new customer",
            "Create a customer's order",
            "Display order details of a customer",
            "Modify order details",
            "Process an order and checkout",
            "Display monthly charged amounts breakdown & total charged amounts for the year",
            "Exit"];

        public static void DisplayMenu(string[] options, string title)
        {
            int idx = 1;
            Console.WriteLine("{0}:", title);
            foreach (string option in options)
            {
                Console.WriteLine("[{0}] {1}", idx, option);
                idx++;
            }
        }
        public static bool IsValidOption(string holdOption, string[] options)
        {
            int option;

            string message = String.Format("'{0}' is an invalid option, ", holdOption);

            if (!Int32.TryParse(holdOption, out option))
            {
                message += "please enter a number.";
            }
            else if (option > options.Length || option < 1)
            {
                message += String.Format("please enter a number within the range 1 and {0}", options.Length);
            }
            else
            {
                return true;
            }

            Console.WriteLine(message);

            return false;
        }
        public static int GetOption(string prompt, string[] options, string title)
        {
            while (true)
            {
                DisplayMenu(options, title);
               
                Console.Write(prompt + ": ");
                string? holdOption = Console.ReadLine();

                if (holdOption != null && IsValidOption(holdOption, options))
                {
                    return Convert.ToInt32(holdOption);
                }
            }
        }
        public static bool TryInitializeCustomers(Dictionary<int, Customer> customerDic, string filePath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // Read and ignore the header line
                    sr.ReadLine();

                    string? line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] data = line.Split(',');

                        if (data.Length != 6)
                        {
                            Console.WriteLine($"Invalid, {data.Length} values passed.");
                            continue;
                        }

                        Customer? customer = TryParseCustomer(data);

                        if (customer == null)
                        {
                            throw new Exception("Failed to parse customer, skipping customer.");
                        }

                        customerDic.Add(customer.MemberId, customer);
                    }
                }
                return true;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return false;
            }
        }
        public static Customer? TryParseCustomer(string[] data)
        {
            /* Format
            Name,MemberId,DOB,MembershipStatus,MembershipPoints,PunchCard
            Amelia,685582,12/03/2000,Gold,150,8
             */

            string name, membershipStatus;
            int memberId, points, punchCard;
            DateTime dob;

            // Try to convert to respective types 
            try
            {
                name = data[0];
                memberId = Convert.ToInt32(data[1]);
                dob = Convert.ToDateTime(data[2]);
                membershipStatus = data[3];
                points = Convert.ToInt32(data[4]);
                punchCard = Convert.ToInt32(data[5]);
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return null;
            }

            // Create PointCard and Customer from values found
            PointCard pointCard = new PointCard(points, punchCard)
            {
                Tier = data[3]
            };

            Customer customer = new Customer(data[0], memberId, dob)
            {
                Rewards = pointCard
            };

            return customer;
        }
        public static bool TryInitalizeOrders(Dictionary<int, Customer> customerDic, string filePath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // Read and ignore the header line
                    sr.ReadLine();

                    string? line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] data = line.Split(',');

                        if (data.Length != 15)
                        {
                            Console.WriteLine($"Invalid, {data.Length} values passed.");
                            return false;
                        }

                        if (!TryParseOrder(data, customerDic))
                        {
                            throw new Exception("Failed to parse order, skipping order.");
                        }
                    }
                }
                return true;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return false;
            }

        }
        public static bool TryParseOrder(string[] data, Dictionary<int, Customer> customerDic)
        {
            /* Format 
            Id,MemberId,TimeReceived,TimeFulfilled,Option,Scoops,Dipped,WaffleFlavour,Flavour1,Flavour2,Flavour3,Topping1,Topping2,Topping3,Topping4
            1,685582,27/10/2023 13:28,27/10/2023 13:51,Waffle,1,,Red Velvet,Ube,,,Mochi,,,
            */

            int id, memberId, scoops;
            string option, waffleFlavour;
            bool isDipped;
            DateTime timeReceived, timeFulfilled;

            List<Flavour> flavours = new List<Flavour>();
            List<Topping> toppings = new List<Topping>();

            try
            {
                id = Convert.ToInt32(data[0]);
                memberId = Convert.ToInt32(data[1]);
                timeReceived = Convert.ToDateTime(data[2]);
                timeFulfilled = Convert.ToDateTime(data[3]);
                option = data[4];
                scoops = Convert.ToInt32(data[5]);

                // Optional fields
                if (data[6] == "") 
                {
                    isDipped = false;
                }
                else if (!bool.TryParse(data[6], out isDipped))
                {
                    throw new Exception("Dipped value is invalid");
                }

                if (data[7] == "" || data[7] == "Original") 
                {
                    waffleFlavour = "";
                }
                else if (IceCreamBuilder.WaffleFlavourNames.Contains(data[7]))
                {
                    waffleFlavour = data[7];
                }
                else
                {
                    throw new Exception("Waffle Flavour value is invalid");
                }

                foreach (string flavour in data[8..11])
                {
                    if (flavour == "") 
                    {
                        break;
                    }
                    else if (flavours.Any(obj => obj.Type == flavour))
                    {
                        flavours.First(obj => obj.Type == flavour).Quantity++;
                    }
                    else
                    {
                        bool premium = IceCreamBuilder.PremiumFlavourNames.Contains(flavour);
                        flavours.Add(new Flavour(flavour, premium, 1));
                    }
                }

                foreach (string topping in data[11..data.Length])
                {
                    if (topping == "") 
                    {
                        break;
                    }
                    else
                    {
                        toppings.Add(new Topping(topping));
                    }
                }
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return false;
            }

            Customer customer = customerDic[memberId];

            IceCream iceCream = new Cup(option, scoops, flavours, toppings);

            if (option == "Cone")
            {
                iceCream = new Cone(option, scoops, flavours, toppings, isDipped);
            }
            else if (option == "Waffle")
            {
                iceCream = new Waffle(option, scoops, flavours, toppings, waffleFlavour);
            }

            // Create a new order if it does not exist
            if (!customer.OrderHistory.Any(obj => obj.Id == id))
            {
                Order order = new Order(id, timeReceived) 
                { 
                    TimeFulfilled = timeFulfilled 
                };
                customer.OrderHistory.Add(order);
            }

            // Add the ice cream to the order
            customer.OrderHistory.First(obj => obj.Id == id).AddIceCream(iceCream);

            return true;
        }
        public static IceCream ModifyIceCream(IceCream iceCream)
        {
            IceCreamBuilder builder = new IceCreamBuilder(iceCream);

            while (true)
            {
                Console.WriteLine("Current ice cream:\n" + iceCream.ToString());

                Helper.DisplayMenu(ModifyIceCreamOptions, "Modify ice cream");

                Console.Write("Enter the modification option: ");
                string? input = Console.ReadLine();

                if (input == null || !Helper.IsValidOption(input, ModifyIceCreamOptions))
                {
                    continue;
                }

                int optionIdx = Convert.ToInt32(input);

                // Change option & select add-on
                if (optionIdx == 1)
                {
                    builder.UpdateOption();
                }
                // Change flavours
                else if (optionIdx == 2)
                {
                    builder.UpdateFlavours();
                }
                // Change toppings
                else if (optionIdx == 3)
                {
                    builder.UpdateToppings();
                }
                // Change add on
                else if (optionIdx == 4)
                {
                    builder.UpdateAddOn();
                }
                // Exit
                else if (optionIdx == 5)
                {
                    return iceCream;
                }

                iceCream = builder.GetIceCream();
            }
        }
        public static Order CreateRandomOrder()
        {
            Random rand = new Random();
            DateTime now = new DateTime();

            Order order = new Order(rand.Next(101), now);

            int noIceCreams = rand.Next(5);

            for (int i = 1; i < noIceCreams; i++)
            {
                IceCream iceCream = CreateRandomIceCream(rand);
                order.AddIceCream(iceCream);
            }
            return order;
        }
        public static void AddRandomOrder(Dictionary<int, Customer> customerDic, Queue<Order> gQ, Queue<Order> rQ)
        {
            Random rand = new Random();

            Customer customer = customerDic.Values.ElementAt(rand.Next(customerDic.Count));

            Queue<Order> q = rQ;
            if (customer.Rewards.Tier == "Gold")
            {
                q = gQ;
            }

            Order order = new Order(q.Count + 1, DateTime.Now);

            int noIceCreams = rand.Next(1, 5);

            for (int i = 0; i < noIceCreams; i++)
            {
                IceCream iceCream = CreateRandomIceCream(rand);
                order.AddIceCream(iceCream);
            }
            Console.WriteLine(order.ToString());

            customer.CurrentOrder = order;
            q.Enqueue(order);
        }
        public static IceCream CreateRandomIceCream(Random rand)
        {
            string[] options = ["Cup", "Cone", "Waffle"];
            string[] flavours = ["Chocolate", "Vanilla", "Strawberry", "Durian", "Ube", "Sea salt"];
            string[] premiumFlavours = ["Durian", "Ube", "Sea salt"];
            string[] toppings = ["Sprinkles", "Mochi", "Sago", "Oreos"];
            string[] waffleFlavours = ["Red velvet", "Charcoal", "Pandan Waffle"];

            // flavour
            List<Flavour> flavoursList = new List<Flavour>();
            int noFlavours = rand.Next(3);

            for (int i = 0; i < noFlavours; i++)
            {
                int flavourIdx = rand.Next(0, 6);

                string flavour = flavours[flavourIdx];

                bool flavourExits = false;
                foreach (Flavour fla in flavoursList)
                {
                    if (fla.Type == flavour)
                    {
                        flavourExits = true;
                        fla.Quantity++;
                    }
                }
                if (!flavourExits)
                {
                    bool premium = premiumFlavours.Contains(flavour);
                    flavoursList.Add(new Flavour(flavour, premium, 1));
                }
            }

            // topping
            List<Topping> toppingList = new List<Topping>();
            int noToppings = rand.Next(4);

            for (int i = 0; i < noToppings; i++)
            {
                int toppingIdx = rand.Next(0, 4);
                toppingList.Add(new Topping(toppings[toppingIdx]));
            }

            // option 
            int optionIdx = rand.Next(3);
            string option = options[optionIdx];

            IceCream iceCream = new Cup(option, flavoursList.Count, flavoursList, toppingList);

            if (optionIdx == 1)
            {
                bool dip = Convert.ToBoolean(rand.Next(0, 2));
                iceCream = new Cone(option, flavoursList.Count, flavoursList, toppingList, dip);
            }
            else
            {
                bool flavouredWaffle = Convert.ToBoolean(rand.Next(0, 2));
                string waffleFlavour = "";
                if (flavouredWaffle)
                {
                    waffleFlavour = waffleFlavours[rand.Next(3)];
                }
                iceCream = new Waffle(option, flavoursList.Count, flavoursList, toppingList, waffleFlavour);
            }

            return iceCream;
        }
    }
}
