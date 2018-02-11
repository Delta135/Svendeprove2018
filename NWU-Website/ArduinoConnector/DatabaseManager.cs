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
        private int areaId;//the id of the area the card reader is at
        private int cardID;

        private SqlConnection sqlConn;
        private SqlCommand sqlcmd;
        private DatabaseResult dbr;

        public DatabaseManager(int areaId)
        {
            this.areaId = areaId;    
        }

        public byte CheckCard(byte[] RecivedUID)
        {
            dbr = getCardData();

            cardID = arrayToInt(RecivedUID);

            //Console.WriteLine("DB CARDID: " + dbr.CardUID + " Input: " + cardID);

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

        //retive carddata from the database
        private DatabaseResult getCardData()
        {
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

                    return buildDatabaseResault((int)uid, (int)currentArea, (int)maxArea, (bool)checkInstat);
                    //return new DatabaseResult((int)uid, (int)currentArea, (int)maxArea, (bool)checkInstat);
                }
            }

            return buildDatabaseResault(0,0,0,false);
        }

        //convert the array into a whole number
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


        //builds the databaseresult
        private DatabaseResult buildDatabaseResault(int cardUID, int currentNumber, int maxNumber, bool checkInStatus)
        {
            return dbr = new DatabaseResult(cardUID, currentNumber, maxNumber, checkInStatus);
        }

        //public void InsertIntoDB(string sqlQurry)
        //{
        //    using (sqlConn = new SqlConnection(sqlConnectionString))
        //    {
        //        sqlConn.Open();
        //        Console.WriteLine(sqlConn.Database);
        //        DateTime myDateTime = DateTime.Now;
        //        private string sqlFormattedDate;
        //        sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        //        Console.WriteLine(sqlFormattedDate);
        //        sqlQurry = "USE Test; INSERT INTO cardTest(kortUID, kortCheckedInd, omraadeAntal, omraadeMaxAntal, timetamp) VALUES(1721263213, 0, 10, 15, '" + sqlFormattedDate + "')";
        //        SqlCommand sqlcmd = new SqlCommand(sqlQurry, sqlConn);
        //        SqlDataReader reader = sqlcmd.ExecuteReader();
        //        reader.Close();
        //    }
        //}
    }
}
