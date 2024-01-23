using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10256965_PRG2Assignment
{
    public static class Helper
    {
        public static string[] ModifyOptions = ["Modify existing ice cream", "Add new ice cream", "Delete existing ice cream"];
        public static string[] ModifyIceCreamOptions = ["Option", "Flavour", "Topping", "Add-on's", "Exit"];
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
        public static bool TryParseCustomer(string[] data, out Customer? customer)
        {
            customer = null;

            if (!int.TryParse(data[1], out int memberId))
            {
                Console.WriteLine($"Invalid, unable to parse member id of {data[0]}");
                return false;
            }

            if (!DateTime.TryParse(data[2], out DateTime dob))
            {
                Console.WriteLine($"Invalid, unable to parse date of birth of {data[0]}");
                return false;
            }

            if (!int.TryParse(data[4], out int points))
            {
                Console.WriteLine($"Invalid, unable to parse points of {data[0]}");
                return false;
            }

            if (!int.TryParse(data[5], out int punchCard))
            {
                Console.WriteLine($"Invalid, unable to parse punch card of {data[0]}");
                return false;
            }

            PointCard pointCard = new PointCard(points, punchCard)
            {
                Tier = data[3]
            };

            customer = new Customer(data[0], memberId, dob)
            {
                Rewards = pointCard
            };

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

            for (int i = 0; i < noIceCreams; i++)
            {
                IceCream iceCream = CreateRandomIceCream(rand);
                order.AddIceCream(iceCream);
            }
            return order;
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
