using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.Data
{
    public class ConnectionHelper
    {
        public static Config Config;

        public static string CreateConnectionString()
        {
            const string appName = "EntityFramework";
            const string providerName = "System.Data.SqlClient";

            string metaData = Config.AppConfiguration.Settings.DataSource.Metadata;
            string dataSource = Config.AppConfiguration.Settings.DataSource.Hostname;
            string initialCatalog = Config.AppConfiguration.Settings.DataSource.InitialCatalog;
            string userId = Config.AppConfiguration.Settings.DataSource.Username;
            string password = Config.AppConfiguration.Settings.DataSource.Password;


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
            return new AmsModel(ConnectionHelper.CreateConnectionString());
        }
    }
}
