using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace SJBCS.Data
{
    public class AmsDbContext : AmsModel
    {
        protected string metaData = "res://*/AmsModel.csdl|res://*/AmsModel.ssdl|res://*/AmsModel.msl";
        protected string dataSource = "DESKTOP-BQK5NF7";
        protected string initialCatalog = "AMS";
        protected string userId = "amsadmin";
        protected string password = "password1";

        public AmsModel GetConnection()
        {
            return ConnectionHelper.CreateConnection(userId,
                password,
                metaData,
                dataSource,
                initialCatalog);
        }

    }
}
