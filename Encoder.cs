using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waymo_car_dealership
{
    public class Encoder
    {
        // Encode function to convert object of type OrderClass to String by adding space as delimiter
        public static String Encode(OrderClass e)
        {
            String s = Convert.ToString(e.getSenderID()) + " " + Convert.ToString(e.getAmount()) + " " + Convert.ToString(e.getCreditCard()) + " " + Convert.ToString(e.getUnitPrice()); ; 
            return s;
        }
    }
}
