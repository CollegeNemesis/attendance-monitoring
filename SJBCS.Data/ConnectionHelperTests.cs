using NUnit.Framework;
using SJBCS.Data;
using System;
using System.Configuration;
using System.Linq;

namespace SJBCS.Data
{
    public class ConnectionHelperTests
    {
        protected string metaData = "res://*/AmsModel.csdl|res://*/AmsModel.ssdl|res://*/AmsModel.msl";
        protected string dataSource = "DESKTOP-BQK5NF7";
        protected string initialCatalog = "AMS";
        protected string userId = "amsadmin";
        protected string password = "password1";

        [Test]
        public void ConnectionHelper_CreateConnection1()
        {
            using (AmsModel dbContext = ConnectionHelper.CreateConnection())
            {
                Console.WriteLine(dbContext.Students.ToList().Count);
            }
        }

        [Test]
        public void ConnectionHelper_CreateConnection2()
        {
            using (AmsModel dbContext = ConnectionHelper.CreateConnection(userId,
                password,
                metaData,
                dataSource,
                initialCatalog))
            {
                Console.WriteLine(dbContext.Students.ToList().Count);
            }
        }

        [Test]
        public void ConnectionHelper_CreateConnection3()
        {
            var metaData = ConfigurationManager.AppSettings["MetaData"];
            var dataSource = ConfigurationManager.AppSettings["DataSource"];
            var initialCatalog = ConfigurationManager.AppSettings["InitialCatalog"];
            var userId = ConfigurationManager.AppSettings["UserId"];
            var password = ConfigurationManager.AppSettings["Password"];

            using (AmsModel dbContext = ConnectionHelper.CreateConnection(userId,
                password,
                metaData,
                dataSource,
                initialCatalog))
            {
                Console.WriteLine(dbContext.Students.ToList().Count);
            }
        }
    }
}
