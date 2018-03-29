using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.Data
{
    public class ConnectionHelper
    {
        public static string CreateConnectionString()
        {
            const string appName = "EntityFramework";
            const string providerName = "System.Data.SqlClient";

            string metaData = "res://*/AmsModel.csdl|res://*/AmsModel.ssdl|res://*/AmsModel.msl";
            string dataSource = "DESKTOP-BQK5NF7";
            string initialCatalog = "AMS";
            string userId = "amsadmin";
            string password = "password1";


            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
            sqlBuilder.DataSource = dataSource;
            sqlBuilder.InitialCatalog = initialCatalog;
            sqlBuilder.MultipleActiveResultSets = true;
            sqlBuilder.IntegratedSecurity = true;
            sqlBuilder.UserID = userId;
            sqlBuilder.Password = password;
            sqlBuilder.ApplicationName = appName;

            EntityConnectionStringBuilder efBuilder = new EntityConnectionStringBuilder();
            efBuilder.Metadata = metaData;
            efBuilder.Provider = providerName;
            efBuilder.ProviderConnectionString = sqlBuilder.ConnectionString;

            return efBuilder.ConnectionString;
        }

        public static string CreateConnectionString (string userId, string password, string metaData, string dataSource, string initialCatalog)
        {
            const string appName = "EntityFramework";
            const string providerName = "System.Data.SqlClient";


            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
            sqlBuilder.DataSource = dataSource;
            sqlBuilder.InitialCatalog = initialCatalog;
            sqlBuilder.MultipleActiveResultSets = true;
            sqlBuilder.IntegratedSecurity = true;
            sqlBuilder.UserID = userId;
            sqlBuilder.Password = password;
            sqlBuilder.ApplicationName = appName;

            EntityConnectionStringBuilder efBuilder = new EntityConnectionStringBuilder();
            efBuilder.Metadata = metaData;
            efBuilder.Provider = providerName;
            efBuilder.ProviderConnectionString = sqlBuilder.ConnectionString;

            return efBuilder.ConnectionString;
        }

        public static AmsModel CreateConnection()
        {
            //return new AmsModel(ConnectionHelper.CreateConnectionString(userId, password, metaData, dataSource, initialCatalog));
            return new AmsModel(ConnectionHelper.CreateConnectionString());
        }

        public static AmsModel CreateConnection(string userId, string password, string metaData, string dataSource, string initialCatalog)
        {
            return new AmsModel(ConnectionHelper.CreateConnectionString(userId, password, metaData, dataSource, initialCatalog));

        }
    }
}
