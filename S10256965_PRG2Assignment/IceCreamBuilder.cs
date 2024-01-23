using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10256965_PRG2Assignment
{
    public class IceCreamBuilder
    {
        private IceCream HoldIceCream { get; set; }
        private readonly string[] OptionNames = ["Cup", "Cone", "Waffle"];
        private readonly string[] FlavourNames = ["Chocolate", "Vanilla", "Strawberry", "Durian", "Ube", "Sea salt"];
        private readonly string[] PremiumFlavourNames = ["Durian", "Ube", "Sea salt"];
        private readonly string[] ToppingNames = ["Sprinkles", "Mochi", "Sago", "Oreos"];
        private readonly string[] WaffleFlavourNames = ["Red velvet", "Charcoal", "Pandan Waffle"];

        private readonly int maxNoFlavours = 3;
        private readonly int maxNoToppings = 4;

        public IceCreamBuilder() 
        {
            HoldIceCream = new Cup();
            UpdateOption();
            UpdateFlavours();
            UpdateToppings();
        }
        public IceCreamBuilder(IceCream iceCream)
        {
            HoldIceCream = iceCream;
        }
        public void UpdateOption()
        {
            int optionIdx = Helper.GetOption("Enter how you would like your ice cream to be served",
                OptionNames, "Serving options");
            string option = OptionNames[optionIdx - 1];

            if (optionIdx == 1)
            {
                 HoldIceCream = new Cup(option, HoldIceCream.Scoop, HoldIceCream.Flavours, HoldIceCream.Toppings);
            }
            else if (optionIdx == 2)
            {
                bool isDipped = GetConeAddOn();
                HoldIceCream = new Cone(option, HoldIceCream.Scoop, HoldIceCream.Flavours, HoldIceCream.Toppings, isDipped);
            }
            else            
            {
                string? waffleFlavour = GetWaffleAddOn();
                HoldIceCream = new Waffle(option, HoldIceCream.Scoop, HoldIceCream.Flavours, HoldIceCream.Toppings, waffleFlavour);
            }
        }
        public void UpdateFlavours()
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

                    foreach (Flavour flav in HoldIceCream.Flavours)
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
                        HoldIceCream.Flavours.Add(new Flavour(flavour, premium, 1));
                    }
                }
            }
        }
        public void UpdateToppings()
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

                    HoldIceCream.Toppings.Add(new Topping(topping));
                }
            }
        }
        public void UpdateAddOn()
        {
            if (HoldIceCream is Cone)
            {
                bool isDipped = GetConeAddOn();
                HoldIceCream = new Cone(HoldIceCream.Option, HoldIceCream.Scoop, HoldIceCream.Flavours, HoldIceCream.Toppings,
                    isDipped);
            }
            else if (HoldIceCream is Waffle) 
            { 
                string? waffleFlavour = GetWaffleAddOn();
                HoldIceCream = new Waffle(HoldIceCream.Option, HoldIceCream.Scoop, HoldIceCream.Flavours, HoldIceCream.Toppings,
                    waffleFlavour);
            }
        }
        public bool GetConeAddOn()
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
        public string? GetWaffleAddOn()
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
        public IceCream GetIceCream()
        {
            return HoldIceCream;
        }
    }
}
