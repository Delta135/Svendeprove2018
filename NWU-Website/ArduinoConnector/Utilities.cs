using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
