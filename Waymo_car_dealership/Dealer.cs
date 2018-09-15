using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Waymo_car_dealership
{
    
    class Dealer
    {
        Int32 Unit_Price = 0;                                                       // Current price cut
        Int32 need = 0;
        static Int32 previous_cut = 0;                                                     // Previous price cut
        ManualResetEvent cut_event = new ManualResetEvent(false);                   // ManualResetEvent to notify DealerFunc that there is a pricecut 
        Int32[] CreditCard = new Int32[] { 34532, 44744, 55643, 60054, 67898 };     // CreditCard details corresponding to 5 threads 
        Int32[] diff = new Int32[] {50,75,100,200,300};                             // Maximum difference between the previous price and the current price to place order
                                                                                    // ID=0 for Dealer1 , ID=1 for Dealer2
                                                                                    // ID=2 for Dealer3 , ID=3 for Dealer4
                                                                                    // ID=4 for Dealer5
        static Random r1 = new Random();                                            
        Int32 ID;

        public Dealer(Int32 id)    // Constuctor to assign unique IDs for dealer objects from which threads are instantiated
        {
            ID = id;
        }
        public void DealerFunc()
        {
            need = r1.Next(1200,3000);                                             // The number of cars needed by each Dealer is selected using random function
            while (Mainapp.price_cut_count < 20)                                   // Mainapp.price_cut_count keeps track of number of price cuts
            { 
                cut_event.WaitOne();                                               // Checking for price-cut event
                if (need > 0 && Math.Abs(Unit_Price - previous_cut) < diff[ID])   // Place an order if number of cars required by a dealer is greater than one and Maximum difference between the previous price and the current price is within limit
                {
                    Mainapp.order_thread_count++;
                    OrderClass dealer_object = new OrderClass();                // Create OrderClass object to place an order after converting to string
                    Int32 amount = r1.Next(1, 50);
                    need = need-amount;
                    dealer_object.setSenderID(ID+1);
                    dealer_object.setAmount(amount);
                    dealer_object.setCreditCard(CreditCard[ID]);
                    dealer_object.setUnitPrice(Unit_Price);
                    String res = Encoder.Encode(dealer_object);                 // Call encoder to convert DataObject to String
                    Monitor.Enter(Mainapp.Buffer);                             // Acquire an exclusive lock on Buffer object
                    try
                    {
                        Mainapp.Buffer.setOneCell(res);                        // Call setOneCell to add string to the cell
                        Console.WriteLine("Dealer " + (ID+1) + " has placed the order-->  " + "Number Of Cars:" + dealer_object.getAmount() + "  UnitPrice:" + dealer_object.getUnitPrice() + "  CreditCard:" + dealer_object.getCreditCard());

                    }
                    catch
                    {
                        Console.WriteLine("Error");
                    }
                    finally { Monitor.Exit(Mainapp.Buffer); }                 // Release an exclusive lock on Buffer object
                    
                }
                cut_event.Reset();                                           // Set the event to false
            }
           
        }

        public void carOnsale(Int32 unit_price)
        {
            previous_cut = Unit_Price;
            Unit_Price = unit_price;                                     // Get the new price of each car
            cut_event.Set();                                             // Notify the DealerFunc() that pricecut event has occurred
        }

    }
}
