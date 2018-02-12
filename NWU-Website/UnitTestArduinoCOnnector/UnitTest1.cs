﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArduinoConnector;

namespace UnitTestArduinoCOnnector
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod_calculateAreaNumber()
        {
            //Test calculateAreaNumber
            DatabaseResult dbResult = new DatabaseResult(0, 20, 15, false);

            Assert.IsTrue(dbResult.TooManyInArea);

            dbResult = new DatabaseResult(0, 380, 560, false);

            Assert.IsFalse(dbResult.TooManyInArea);
        }

        [TestMethod]
        public void TestMethod_arrayToInt()
        {
            DatabaseManager dbMan = new DatabaseManager();
            PrivateObject testDbMan = new PrivateObject(dbMan);

            //dummy data
            byte[] testVal = new byte[] { 1, 2, 3 };
            int value = (int)testDbMan.Invoke("arrayToInt", testVal); //test private method

            Assert.AreEqual(value, 123);
        }

        [TestMethod]
        public void TestMethod_buildDatabaseResault()
        {
            DatabaseManager dbMan = new DatabaseManager();
            PrivateObject testDbMan = new PrivateObject(dbMan);

            //dummy data
            int UID = 374651249, current = 90, max = 130;
            bool checkedIn = false;

            DatabaseResult dbr =  (DatabaseResult)testDbMan.Invoke("buildDatabaseResault", UID, current, max, checkedIn);

            Assert.AreEqual(dbr.CardUID, UID);
            Assert.AreEqual(dbr.CurrentAreaNumber, current);
            Assert.AreEqual(dbr.MaxAreaNumber, max);
            Assert.IsFalse(dbr.CardIsCheckedIn);
        }
    }
}
