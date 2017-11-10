using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waymo_car_dealership
{
    public class Decoder
    {
        // Encode function to convert String to object of type OrderClass by splitting using space whcih is the delimiter used in Encoder class
        public static OrderClass Decode(String d1)
        {
            String[] s = d1.Split(' ');                           
            OrderClass object1 = new OrderClass();
            object1.setSenderID(Int32.Parse(s[0]));
            object1.setAmount(Int32.Parse(s[1]));
            object1.setCreditCard(Int32.Parse(s[2]));
            object1.setUnitPrice(Int32.Parse(s[3]));
            return object1;
        }
    }
}
