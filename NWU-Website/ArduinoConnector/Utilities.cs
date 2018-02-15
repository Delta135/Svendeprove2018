using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArduinoConnector
{
    public static class Utilities
    {
        //DatabaseResult Factory
        public static DatabaseResult GetDatabaseResult(int cardUID, int currentNmberInArea, int maxNumberInArea, bool isCheckedIn)
        {
            DatabaseResult newDbr = new DatabaseResult(cardUID, currentNmberInArea, maxNumberInArea, isCheckedIn);

            //is there room in the area?
            if (currentNmberInArea <= maxNumberInArea && currentNmberInArea != maxNumberInArea)
            {
                //Yes
                newDbr.TooManyInArea = false;
            }
            else
            {
                //No
                newDbr.TooManyInArea = true;
            }

            return newDbr;
        }

        //test the database by sending random data to it
        public static void DatabaseTest()
        {
            DatabaseManager dbMan = new DatabaseManager();
            Random rng = new Random();
            bool isCheckedIn = false;
            bool.TryParse(rng.Next(0,2).ToString(), out isCheckedIn);

            //do it forever
            while (true)
            {
                dbMan.InsertData(rng.Next(1,6), rng.Next(5, 301), isCheckedIn);

                //wait for one second
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }
}
