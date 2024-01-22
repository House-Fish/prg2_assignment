using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10256965_PRG2Assignment
{
    public class IceCreamDirector
    {
        private IceCream _iceCream { get; set; }

        public IceCreamDirector()
        { 
            _iceCream = new Cup();
        }
        public IceCreamDirector(IceCream iceCream)
        {
            _iceCream = iceCream;
        }
        public void Build()
        {
            _iceCream = IceCreamBuilder.UpdateOption(_iceCream);
            _iceCream = IceCreamBuilder.UpdateFlavours(_iceCream);
            _iceCream = IceCreamBuilder.UpdateToppings(_iceCream);
        }
        public void ModifyOption()
        {
            _iceCream = IceCreamBuilder.UpdateOption(_iceCream);
        }
        public void ModifyFlavours()
        {
            _iceCream = IceCreamBuilder.UpdateFlavours(_iceCream);
        }
        public void ModifyToppings()
        {
            _iceCream = IceCreamBuilder.UpdateToppings(_iceCream);
        }
        public void ModifyAddOn()
        {
            if (_iceCream is Cone)
            {
                bool isDipped = IceCreamBuilder.GetConeAddOn();
                _iceCream = new Cone(_iceCream.Option, _iceCream.Scoop, _iceCream.Flavours, _iceCream.Toppings,
                    isDipped);
            }
            else if ( _iceCream is Waffle) 
            { 
                string? waffleFlavour = IceCreamBuilder.GetWaffleAddOn();
                _iceCream = new Waffle(_iceCream.Option, _iceCream.Scoop, _iceCream.Flavours, _iceCream.Toppings,
                    waffleFlavour);
            }
        }
        public IceCream GetIceCream()
        {
            return _iceCream;
        }
    }
}
