using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10256965_PRG2Assignment
{
    public class IceCreamBuilder
    {
        private static readonly string[] OptionNames = ["Cup", "Cone", "Waffle"];
        private static readonly string[] FlavourNames = ["Chocolate", "Vanilla", "Strawberry", "Durian", "Ube", "Sea salt"];
        private static readonly string[] PremiumFlavourNames = ["Durian", "Ube", "Sea salt"];
        private static readonly string[] ToppingNames = ["Sprinkles", "Mochi", "Sago", "Oreos"];
        private static readonly string[] WaffleFlavourNames = ["Red velvet", "Charcoal", "Pandan Waffle"];

        private static readonly int maxNoFlavours = 3;
        private static readonly int maxNoToppings = 4;

        public static IceCream UpdateOption(IceCream iceCream)
        {
            int optionIdx = Helper.GetOption("Enter how you would like your ice cream to be served",
                OptionNames, "Serving options");
            string option = OptionNames[optionIdx - 1];

            if (optionIdx == 1)
            {
                iceCream = new Cup(option, iceCream.Scoop, iceCream.Flavours, iceCream.Toppings);
            }
            else if (optionIdx == 2)
            {
                bool isDipped = GetConeAddOn();
                iceCream = new Cone(option, iceCream.Scoop, iceCream.Flavours, iceCream.Toppings, isDipped);
            }
            else            
            {
                string? waffleFlavour = GetWaffleAddOn();
                iceCream = new Waffle(option, iceCream.Scoop, iceCream.Flavours, iceCream.Toppings, waffleFlavour);
            }
            return iceCream;
        }
        public static IceCream UpdateFlavours(IceCream iceCream)
        {
            string menuTitle = "Flavour options";
            string prompt = "You can choose a maximum of " + maxNoFlavours + ".\nEnter 's' if you would like to skip.";

            int noFlavours = 0;
            bool skip = false;

            while (!skip && maxNoFlavours != noFlavours)
            {
                Helper.DisplayMenu(FlavourNames, menuTitle); 
                Console.WriteLine(prompt);

                for (; noFlavours < maxNoFlavours; noFlavours++)
                {
                    Console.Write("Enter flavour ({0} left): ", maxNoFlavours - noFlavours);
                    string? input = Console.ReadLine();

                    if (input == "s")
                    {
                        Console.WriteLine("Skipping...");
                        skip = true;
                        break;
                    }
                    else if (input == null || !Helper.IsValidOption(input, FlavourNames))
                    {
                        break;
                    }

                    string flavour = FlavourNames[Convert.ToInt32(input) - 1];

                    bool flavourExits = false;

                    foreach (Flavour flav in iceCream.Flavours)
                    {
                        if (flav.Type == flavour)
                        {
                            flavourExits = true;
                            flav.Quantity++;
                        }
                    }
                    if (!flavourExits)
                    {
                        bool premium = PremiumFlavourNames.Contains(flavour);
                        iceCream.Flavours.Add(new Flavour(flavour, premium, 1));
                    }
                }
            }
            return iceCream;
        }
        public static IceCream UpdateToppings(IceCream iceCream)
        {
            string menuTitle = "Topping options";
            string prompt = "You can choose a maximum of " + maxNoToppings + ".\nEnter 's' if you would like to skip.";

            int noToppings = 0;
            bool skip = false;

            while (!skip && maxNoToppings != noToppings)
            {
                Helper.DisplayMenu(ToppingNames, menuTitle); 
                Console.WriteLine(prompt);

                for (; noToppings < maxNoToppings; noToppings++)
                {
                    Console.Write("Enter topping ({0} left): ", maxNoToppings - noToppings);
                    string? input = Console.ReadLine();

                    if (input == "s")
                    {
                        Console.WriteLine("Skipping...");
                        skip = true;
                        break;
                    }
                    else if (input == null || !Helper.IsValidOption(input, ToppingNames))
                    {
                        break;
                    }

                    string topping = ToppingNames[Convert.ToInt32(input) - 1];

                    iceCream.Toppings.Add(new Topping(topping));
                }
            }
            return iceCream;
        }
        public static bool GetConeAddOn()
        {
            while (true)
            {
                Console.Write("Would you like a chocolate-dipped cone (Y/N): ");
                string? input = Console.ReadLine();

                if (input == "Y" || input == "N")
                {
                    return input == "Y";
                }
                else
                {
                    Console.WriteLine("{} is an invalid option, enter 'Y' or 'N'.", input);
                }
            }
        }
        public static string? GetWaffleAddOn()
        {
            while (true)
            {
                Console.Write("Would you like a flavoured waffle (Y/N): ");
                string? input = Console.ReadLine();

                if (input == "Y")
                {
                    int waffleFlavourIdx = Helper.GetOption("Enter which flavour you would like",
                        WaffleFlavourNames, "Flavoured waffle options");

                    return WaffleFlavourNames[waffleFlavourIdx - 1];
                }
                else if (input == "N")
                {
                    return null;
                }
                else
                {
                    Console.WriteLine("{0} is an invalid option, enter 'Y' or 'N'.", input);
                }
            }
        }

    }
}
