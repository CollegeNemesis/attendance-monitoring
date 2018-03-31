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
        public static string CreateConnectionString()
        {
            const string appName = "EntityFramework";
            const string providerName = "System.Data.SqlClient";

            string json = File.ReadAllText(ConfigurationManager.AppSettings["configPath"]);
            Config config = JsonConvert.DeserializeObject<Config>(json);

            string metaData = config.AppConfiguration.Settings.DataSource.Metadata;
            string dataSource = config.AppConfiguration.Settings.DataSource.Hostname;
            string initialCatalog = config.AppConfiguration.Settings.DataSource.InitialCatalog;
            string userId = config.AppConfiguration.Settings.DataSource.Username;
            string password = config.AppConfiguration.Settings.DataSource.Password;


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
            try
            {
                return new AmsModel(ConnectionHelper.CreateConnectionString());
            }
            catch (Exception error)
            {
                throw error;
            }

        }
    }
}
