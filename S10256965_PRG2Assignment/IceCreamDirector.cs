using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10256965_PRG2Assignment
{
    public class IceCreamDirector
    {
        private IceCream IceCream { get; set; }

        public IceCreamDirector()
        { 
            IceCream = new Cup();
        }
        public IceCreamDirector(IceCream iceCream)
        {
            IceCream = iceCream;
        }
        public void Build()
        {
            IceCream = IceCreamBuilder.UpdateOption(IceCream);
            IceCream = IceCreamBuilder.UpdateFlavours(IceCream);
            IceCream = IceCreamBuilder.UpdateToppings(IceCream);
        }
        public void ModifyOption()
        {
            IceCream = IceCreamBuilder.UpdateOption(IceCream);
        }
        public void ModifyFlavours()
        {
            IceCream = IceCreamBuilder.UpdateFlavours(IceCream);
        }
        public void ModifyToppings()
        {
            IceCream = IceCreamBuilder.UpdateToppings(IceCream);
        }
        public void ModifyAddOn()
        {
            if (IceCream is Cone)
            {
                bool isDipped = IceCreamBuilder.GetConeAddOn();
                IceCream = new Cone(IceCream.Option, IceCream.Scoop, IceCream.Flavours, IceCream.Toppings,
                    isDipped);
            }
            else if ( IceCream is Waffle) 
            { 
                string? waffleFlavour = IceCreamBuilder.GetWaffleAddOn();
                IceCream = new Waffle(IceCream.Option, IceCream.Scoop, IceCream.Flavours, IceCream.Toppings,
                    waffleFlavour);
            }
        }
        public IceCream GetIceCream()
        {
            return IceCream;
        }
    }
}
