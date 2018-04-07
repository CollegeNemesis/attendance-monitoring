using NUnit.Framework;
using System;
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
    }
}
