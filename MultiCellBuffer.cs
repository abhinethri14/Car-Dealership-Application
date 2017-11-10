using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Waymo_car_dealership
{
    public class MultiCellBuffer
    {

        private string[] cells = new string[2] { "-1", "-1" };  // Intialize cell values to -1
        Semaphore s = new Semaphore(0, 2);                      // semaphore of value 2 to manage the availability of the cells
        public void setOneCell(string res)
        {
            while (cells[0] != "-1" && cells[1] != "-1")       // If both the cells are not empty then wait 
                Monitor.Wait(this);
            s.WaitOne(1);                                     // Acquire resource
            if (cells[0] == "-1")                             // Write the string to cell
                    cells[0] = res;
                else if (cells[1] == "-1")
                    cells[1] = res;
            Monitor.PulseAll(this);                          // Inform all the waiting threads

        }
     


        public string getOneCell()
        {
            string val="-1";
            while (cells[0] == "-1" && cells[1] == "-1")       // If cells are empty then wait
                Monitor.Wait(this);
            if (cells[0] != "-1")                              // Read from one of the cells
            {
                val = cells[0];
                cells[0] = "-1";
            }
            else if(cells[1] != "-1")
                    { 
                val = cells[1];
                cells[1] = "-1";
            }

            s.Release();                                    // Release the resource
            Monitor.PulseAll(this);                          // Inform all the waiting threads
            return val;
        }
    }
}
