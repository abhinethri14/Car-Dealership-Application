using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Waymo_car_dealership
{
    public delegate void priceCutEvent(Int32 pr);  // Define a delegate

    // public delegate void order_process(OrderClass oc);  // Delegate for processing the order
    class Plant
    {
        int myId;
        public static Int32 flag = 0;
        string request;
        Int32 Tax;                                  // To calculate the final price for each orde, tax and locationCharge information are required
        Int32 LocationCharge;
        Int32 current_price = 275;                  // Intital price to perform comparision for price cut
        public static Int32 CarPrice = 275;
        Random r = new Random();
        public static event priceCutEvent priceCut;   // Link event to delegate
        public Plant(int id)                          // Constructor to initialize unique ID to threads
        {
            myId = id;
        }

        public void PlantFunc()
        {
            while (Mainapp.price_cut_count < 20)              // Plant thread should termiante after 20 price cuts
            {
                Console.WriteLine();
                while (!changePrice(current_price))           // Loop to check  price cut
                {
                    current_price = PricingModel(CarPrice);   // To update the current price of car

                }
                Console.WriteLine("This is price-cut number: "+ Mainapp.price_cut_count+ "  The unit price of the car after price-cut is " + current_price +" $k");
                Console.WriteLine();
                if (priceCut != null)                        // There is at least a subscriber 
                    priceCut(current_price);                 // Emit event to subscribers
                Thread.Sleep(5000);                          // Sleep for few seconds so that dealers can place orders
                for (int i = 0; i < Mainapp.order_thread_count; i++)
                {
                    Thread order_serve = new Thread(() => OrderProcessingFunc());  
                    order_serve.Start();                     // start Order Processing thread to process order
                }
                Thread.Sleep(5000);                          // sleep for few seconds so that orders placed by Dealer will be processed by orderProcessingFunc
                Mainapp.order_thread_count = 0;
            }
        }





        public void OrderProcessingFunc()
        {
            // request = "-1";
            try
            {
                //  while (request.Equals("-1"))                  // Take the order form the queue
                Monitor.Enter(Mainapp.Buffer);                    // Acquire lock on Buffer object
                request = Mainapp.Buffer.getOneCell();            // Get the order in form of the string
            }
            finally { Monitor.Exit(Mainapp.Buffer); }             // Release lock on Buffer object
            OrderClass order = Decoder.Decode(request);           // Convert string to object of type OrderClass

            Tax = r.Next(1, 10);
            LocationCharge = r.Next(10, 150);
            if (order.getCreditCard() >= 30000 && order.getCreditCard() <= 70000)        // Check whether credit card id valid
            {
                Thread.Sleep(r.Next(1, 3000));
                Console.WriteLine();
                Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("Order from the Dealer " + order.getSenderID() + " has been processed");
                Int32 price = order.getUnitPrice() * order.getAmount() ; // Calculate the final amount
                float final_amount = price + (float)Tax / 100 * price + LocationCharge;
                Console.WriteLine("Total number of cars:" + order.getAmount() + "  Unit Price:" + order.getUnitPrice()+" $k"+ "  Tax: " + (float)Tax/100*price+ " LocationCharge: "+ LocationCharge+" Total amount:" + final_amount + " $k");  // Print the final amount and order detailss
                Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------");
                
            }
            else
                Console.WriteLine("Credit card details are incorrect");

        }



        public static bool changePrice(Int32 price)
        {
            if (price < CarPrice)                // Check whether there is drop in the price
            {
                Mainapp.price_cut_count++;
                CarPrice = price;
                flag = 1;
                return true;

            }
            if(flag!=1)
            Console.WriteLine("There is no price-cut for this unit price: " + price+" $k");
            flag = 0;
            CarPrice = price;                   // Update the car price
            return false;
        }
        public Int32 PricingModel(Int32 price)
        {
            return r.Next(50, 500);           // Random function to calculate car price
        }
    }
}
