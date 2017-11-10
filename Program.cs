using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Waymo_car_dealership
{
    class Mainapp
    {

        public static MultiCellBuffer Buffer = new MultiCellBuffer();        // Create a buffer cell
        public static Int32 price_cut_count = 0;                             // To keep track of number of price cuts
        public static Int32 order_thread_count = 0;                          // To keep track of number of orders
        public static void Main(string[] args)
        {

            Plant p1 = new Plant(1);                                            // Create 1 plant object 
            Thread plant1 = new Thread(new ThreadStart(p1.PlantFunc));

            Dealer d1 = new Dealer(0);                                          // Create 5 dealer Objects 
            Dealer d2 = new Dealer(1);
            Dealer d3 = new Dealer(2);
            Dealer d4 = new Dealer(3);
            Dealer d5 = new Dealer(4);
            Thread[] Dealers = new Thread[5];
            Plant.priceCut += new priceCutEvent(d1.carOnsale);            // Add event methods
            Plant.priceCut += new priceCutEvent(d2.carOnsale);
            Plant.priceCut += new priceCutEvent(d3.carOnsale);
            Plant.priceCut += new priceCutEvent(d4.carOnsale);
            Plant.priceCut += new priceCutEvent(d5.carOnsale);
            Thread dealer1 =new Thread(new ThreadStart(d1.DealerFunc));
            Thread dealer2 = new Thread(new ThreadStart(d2.DealerFunc));
            Thread dealer3 = new Thread(new ThreadStart(d3.DealerFunc));
            Thread dealer4 = new Thread(new ThreadStart(d4.DealerFunc));
            Thread dealer5 = new Thread(new ThreadStart(d5.DealerFunc));

            plant1.Start();
            dealer1.Start();                                                 // Start plant thread and dealaer threads
            dealer2.Start();
            dealer3.Start();
            dealer4.Start();
            dealer5.Start();
            

            plant1.Join();                                                // Wait for all the threads to terminate
            dealer1.Join();
            dealer2.Join();
            dealer3.Join();
            dealer4.Join();
            dealer5.Join(); 

            Console.WriteLine("Main thread is completed");

            Thread.Sleep(100000);
        }
    }
}
