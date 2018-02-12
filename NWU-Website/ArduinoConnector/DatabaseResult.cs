using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoConnector
{
    public class DatabaseResult
    {
        private bool cardIsCheckedIn;
        private bool tooManyInArea;

        private int cardUID;
        private int currentAreaNumber;
        private int maxAreaNumber;

        public bool CardIsCheckedIn
        {
            get
            {
                return cardIsCheckedIn;
            }

            set
            {
                cardIsCheckedIn = value;
            }
        }

        public bool TooManyInArea
        {
            get
            {
                return tooManyInArea;
            }

            set
            {
                tooManyInArea = value;
            }
        }

        public int CardUID
        {
            get
            {
                return cardUID;
            }

            set
            {
                cardUID = value;
            }
        }

        public int CurrentAreaNumber
        {
            get
            {
                return currentAreaNumber;
            }

            set
            {
                currentAreaNumber = value;
            }
        }

        public int MaxAreaNumber
        {
            get
            {
                return maxAreaNumber;
            }

            set
            {
                maxAreaNumber = value;
            }
        }

        public DatabaseResult(int uid, int currentNumber, int maxNumber, bool checkInStatus)
        {
            CardUID = uid;
            CurrentAreaNumber = currentNumber;
            MaxAreaNumber = maxNumber;

            CardIsCheckedIn = checkInStatus;
            TooManyInArea = calculateAreaNumber(currentNumber, maxNumber);
        }

        private bool calculateAreaNumber(int current, int max)
        {
            //is there room in the area?
            if (current <= max && current != max)
            {
                return false;//yes
            }

            return true;//no
        }
    }
}
