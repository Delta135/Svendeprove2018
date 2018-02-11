using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoConnector
{
    class DatabaseManager
    {
        private bool cardIsCheckedIn;
        private bool tooManyInArea;
        private byte[] DBResault;

        public DatabaseManager()
        {
            //test
            cardIsCheckedIn = false;
            tooManyInArea = false;
        }

        //NOTE DBUID is for test only, shoud get that data from the database
        public byte CheckCard(byte[] DBUID, byte[] RecivedUID)
        {
            if (!CheckUID(DBUID, RecivedUID))
            {
                //the card is worng, reject it
                return 0;
            }

            if (cardIsCheckedIn)
            {
                //the card gets checked out
                return 2;
            }

            if (tooManyInArea)
            {
                //the area is full, reject it
                return 3;
            }

            //the card gets checked in
            return 1;
        }

        private bool CheckUID(byte[] DBUID, byte[]RecivedUID)
        {
            for (int i = 0; i < DBUID.Length; i++)
            {
                if (DBUID[i] != RecivedUID[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
