using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waymo_car_dealership
{
    public class OrderClass
    {
        private Int32 senderId;
        private Int32 amount;
        private Int32 unitprice;
        private Int32 cardNo;

        //Following methods assign values to the private members of the class OrderClass
        public void setSenderID(Int32 ID)
        {
            senderId = ID;
        }

        public void setAmount(Int32 am)
        {
            amount = am;
        }
        public void setUnitPrice(Int32 up)
        {
            unitprice = up;
        }
        public void setCreditCard(Int32 cc)
        {
            cardNo = cc;
        }

        //Following methods fetch values for the private members of the class OrderClass
        public Int32 getSenderID()
        {
            return this.senderId;
        }

        public Int32 getAmount()
        {
            return this.amount;
        }
        public Int32 getUnitPrice()
        {
            return this.unitprice;
        }
        public Int32 getCreditCard()
        {
            return this.cardNo;
        }
    }
}
