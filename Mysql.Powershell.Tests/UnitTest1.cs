using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using MySql.Powershell;
using MySqlConnector;
using System.IO;
using System.Management.Automation;

namespace Mysql.Powershell.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var readSp = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "TestSP.sql"));
             
            var connectionString = new MySqlConnectionStringBuilder { 
                Server = "<localhost>", Port = 3306, Database = "<db1>", 
                UserID = "root", 
                Password = "<pass1>"
            }.ConnectionString;

            using(var con = new MySqlConnection(connectionString))
            {
                con.Open();

                var scrpt = new MySqlScript(con, readSp);
                scrpt.Execute();
            }

        }
    }
}
