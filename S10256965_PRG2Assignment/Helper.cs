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
        public static string[] Options = ["Cup", "Cone", "Waffle"];
        public static string[] Flavours = ["Chocolate", "Vanilla", "Strawberry", "Durian", "Ube", "Sea salt"];
        public static string[] PremiumFlavours = ["Durian", "Ube", "Sea salt"];
        public static string[] Toppings = ["Sprinkles", "Mochi", "Sago", "Oreos"];
        public static string[] WaffleFlavours = ["Red velvet", "Charcoal", "Pandan Waffle"];
        public static string[] ModifyOptions = ["Modify existing ice cream", "Add new ice cream", "Delete existing ice cream"];
        public static string[] ModifyIceCream = ["Option", "Flavour", "Topping", "Add-on's"];

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
        public static bool TryParseCustomer(string[] data, out Customer customer)
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

            PointCard pointCard = new PointCard(points, punchCard);
            pointCard.Tier = data[3];

            customer = new Customer(data[0], memberId, dob);
            customer.Rewards = pointCard;

            return true;
        }
        public static string CreateOption()
        {
            int holdOption = GetOption("Enter how you would like your ice cream to be served",
                                        Options, "Serving options");
            return Options[holdOption - 1];
        }
        public static void CreateFlavours(List<Flavour> flavoursList)
        {
            int maxNoFlavours = 3;
            int noFlavours = 0;
            bool skip = false;

            while (!skip)
            {
                DisplayMenu(Flavours, "Flavour options");
                Console.WriteLine("You can choose a maximum of {0} flavours, enter 'c' if you would like to " +
                                  "continue to the toppings.", maxNoFlavours);

                for (; noFlavours < maxNoFlavours; noFlavours++)
                {
                    Console.Write("Enter flavour ({0} left): ", maxNoFlavours - noFlavours);
                    string? holdFlavour = Console.ReadLine();

                    if (holdFlavour == "c")
                    {
                        Console.WriteLine("Continuing...");
                        skip = true;
                        break;
                    }
                    else if (holdFlavour != null && IsValidOption(holdFlavour, Flavours))
                    {
                        string flavour = Flavours[Convert.ToInt32(holdFlavour) - 1];

                        bool flavourExits = false;
                        foreach (Flavour flav in flavoursList)
                        {
                            if (flav.Type == flavour)
                            {
                                flavourExits = true;
                                flav.Quantity++;
                            }
                        }
                        if (!flavourExits)
                        {
                            bool premium = PremiumFlavours.Contains(flavour) ? true : false;
                            flavoursList.Add(new Flavour(flavour, premium, 1));
                        }

                        continue;
                    }
                    noFlavours--;
                }
                if (skip || maxNoFlavours == noFlavours) { break; }
            }
        }
        public static void CreateToppings(List<Topping> toppingsList)
        {
            const int maxNoToppings = 4;
            int noToppings = 0;
            bool skip = false;

            while (!skip)
            {
                DisplayMenu(Toppings, "Toppings options");
                Console.WriteLine("You can choose a maximum of {0} toppings, enter 'c' if you would like to " +
                                  "continue to the add-on's.", maxNoToppings);

                for (; noToppings < maxNoToppings; noToppings++)
                {
                    Console.Write("Enter flavour ({0} left): ", maxNoToppings - noToppings);
                    string? holdTopping = Console.ReadLine();

                    if (holdTopping == "c")
                    {
                        Console.WriteLine("Continuing...");
                        skip = true;
                        break;
                    }
                    else if (holdTopping != null && IsValidOption(holdTopping, Toppings))
                    {
                        string topping = Toppings[Convert.ToInt32(holdTopping) - 1];
                        toppingsList.Add(new Topping(topping));
                        continue;
                    }
                    noToppings--;
                }
                if (skip || maxNoToppings == noToppings) { break; }
            }
        }
        public static IceCream CreateIceCreamCone(List<Flavour> flavoursList, List<Topping> toppingsList)
        {
            bool dipped;
            while (true)
            {
                Console.Write("Would you like a chocolate-dipped cone (Y/N): ");
                string wantDipped = Console.ReadLine();

                if (wantDipped == "Y" || wantDipped == "N")
                {
                    dipped = wantDipped == "Y" ? true : false;
                    break;
                }
                else
                {
                    Console.WriteLine("{} is an invalid option, enter 'Y' or 'N'.", wantDipped);
                }
            }
            return new Cone("Cone", flavoursList.Count, flavoursList, toppingsList, dipped);
        }
        public static IceCream CreateIceCreamCup(List<Flavour> flavoursList, List<Topping> toppingsList)
        {
            return new Cup("Cup", flavoursList.Count, flavoursList, toppingsList);
        }
        public static IceCream CreateIceCreamWaffle(List<Flavour> flavoursList, List<Topping> toppingsList)
        {
            string waffleFlavour = "";
            while (true)
            {
                Console.Write("Would you like a flavoured waffle (Y/N): ");
                string wantFlavourWaffle = Console.ReadLine();

                if (wantFlavourWaffle == "Y")
                {
                    int holdWaffleFlavour = GetOption("Enter which flavour you would like",
                                                      WaffleFlavours, "Flavoured waffle options");
                    waffleFlavour = WaffleFlavours[holdWaffleFlavour - 1];
                    break;
                }
                else if (wantFlavourWaffle == "N")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("{0} is an invalid option, enter 'Y' or 'N'.", wantFlavourWaffle);
                }
            }
            return new Waffle("Waffle", toppingsList.Count, flavoursList, toppingsList, waffleFlavour);
        }
        public static IceCream BuildIceCream()
        {
            // Get the option (Cup, Cone, or Waffle)
            string option = CreateOption();

            // Get the preferred flavours (Vanilla, Chocolate, Strawberry, Durian, Ube, or Sea salt)
            List<Flavour> flavoursList = new List<Flavour>();
            CreateFlavours(flavoursList);

            // Get the prefferd toppings (Sprinkles, Mochi, Sago, Oreos)
            List<Topping> toppingsList = new List<Topping>();
            CreateToppings(toppingsList);

            // Create ice cream objects and add add-ons
            if (option == "Cup")
            {
                return CreateIceCreamCup(flavoursList, toppingsList);
            }
            else if (option == "Cone")
            {
                return CreateIceCreamCone(flavoursList, toppingsList);
            }
            else
            {
                return CreateIceCreamWaffle(flavoursList, toppingsList);
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
                    bool premium = premiumFlavours.Contains(flavour) ? true : false;
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
