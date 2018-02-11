using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoConnector
{
    class DatabaseManager
    {
        private string sqlConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Test;Integrated Security=True;User Id=mthy;Password=Kode2288;";
        private string sqlQurry;
        private string sqlFormattedDate;
        private int areaId;
        private int cardID;

        private SqlConnection sqlConn;
        private SqlCommand sqlcmd;
        private DatabaseResult dbr;

        public DatabaseManager(int areaId)
        {
            this.areaId = areaId;    
        }

        //NOTE DBUID is for test only, shoud get that data from the database
        public byte CheckCard(byte[] RecivedUID)
        {
            dbr = getCardData();

            cardID = arrayToInt(RecivedUID);

            Console.WriteLine("DB CARDID: " + dbr.CardUID + " Input: " + cardID);

            if (dbr.CardUID != cardID)
            {
                //the card is worng, reject it
                return 0;
            }

            if (dbr.CardIsCheckedIn)
            {
                //the card gets checked out
                return 2;
            }

            if (dbr.TooManyInArea)
            {
                //the area is full, reject it
                return 3;
            }

            //the card gets checked in
            return 1;
        }

        private DatabaseResult getCardData()
        {
            DatabaseResult newResult;
            using (sqlConn = new SqlConnection(sqlConnectionString))
            {
                sqlQurry = "SELECT kortUID, kortCheckedInd, omraadeAntal, omraadeMaxAntal FROM cardTest WHERE omraadeId = " + areaId + ";";
                sqlConn.Open();
                sqlcmd = new SqlCommand(sqlQurry, sqlConn);
                SqlDataReader reader = sqlcmd.ExecuteReader();

                while (reader.Read())
                {
                    int? uid = reader["kortUID"] == DBNull.Value ? null : (int?)reader["kortUID"];
                    bool? checkInstat = reader["kortCheckedInd"] == DBNull.Value ? null : (bool?)reader["kortCheckedInd"];
                    int? currentArea = reader["omraadeAntal"] == DBNull.Value ? null : (int?)reader["omraadeAntal"];
                    int? maxArea = reader["omraadeMaxAntal"] == DBNull.Value ? null : (int?)reader["omraadeMaxAntal"];

                    return new DatabaseResult((int)uid, (int)currentArea, (int)maxArea, dbBoolToInt(checkInstat));
                }
            }

            return new DatabaseResult(0,0,0,0);
        }

        //Not the best way, but it's the only consistent one i can find
        private int arrayToInt(byte[] input)
        {
            string temp = String.Empty;
            for (int i = 0; i < input.Length; i++)
            {
                temp += input[i].ToString();
            }
            return int.Parse(temp);
        }

        private DatabaseResult buildDatabaseResault(int cardUID, int checkInStatus, int currentNumber, int maxNumber)
        {
            return dbr = new DatabaseResult(cardID, currentNumber, maxNumber, checkInStatus);
        }

        //converts SQL bit to int
        private int dbBoolToInt(bool? input)
        {
            if (!input.HasValue)
            {
                return 0;
            }

            if ((bool)input)
            {
                return 1;
            }

            return 0;
        }

        public void InsertIntoDB(string sqlQurry)
        {
            using (sqlConn = new SqlConnection(sqlConnectionString))
            {
                sqlConn.Open();
                Console.WriteLine(sqlConn.Database);
                DateTime myDateTime = DateTime.Now;
                sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                Console.WriteLine(sqlFormattedDate);
                sqlQurry = "USE Test; INSERT INTO cardTest(kortUID, kortCheckedInd, omraadeAntal, omraadeMaxAntal, timetamp) VALUES(1721263213, 0, 10, 15, '" + sqlFormattedDate + "')";
                SqlCommand sqlcmd = new SqlCommand(sqlQurry, sqlConn);
                SqlDataReader reader = sqlcmd.ExecuteReader();
                reader.Close();
            }
        }
    }
}
