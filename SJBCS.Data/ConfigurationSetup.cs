namespace SJBCS.Data
{
    public class Config
    {
        public AppConfiguration AppConfiguration { get; set; }

        public Config Copy()
        {
            Config config = (Config) this.MemberwiseClone();

            DataSource dataSource = new DataSource();
            dataSource.Metadata = string.Copy(this.AppConfiguration.Settings.DataSource.Metadata);
            dataSource.Hostname = string.Copy(this.AppConfiguration.Settings.DataSource.Hostname);
            dataSource.InitialCatalog = string.Copy(this.AppConfiguration.Settings.DataSource.InitialCatalog);
            dataSource.Username = string.Copy(this.AppConfiguration.Settings.DataSource.Username);
            dataSource.Password = string.Copy(this.AppConfiguration.Settings.DataSource.Password);

            SmsService smsService = new SmsService();
            smsService.Url = string.Copy(this.AppConfiguration.Settings.SmsService.Url);

            Settings settings = new Settings();
            settings.DataSource = dataSource;
            settings.SmsService = smsService;

            AppConfiguration appConfiguration = new AppConfiguration();
            appConfiguration.Settings = settings;

            config.AppConfiguration = appConfiguration;

            return config;
        }

    }

    public class AppConfiguration
    {
        public Settings Settings { get; set; }
    }

    public class Settings
    {
        public DataSource DataSource { get; set; }
        public SmsService SmsService { get; set; }
    }

    public class DataSource
    {
        public string Metadata { get; set; }
        public string Hostname { get; set; }
        public string InitialCatalog { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class SmsService
    {
        public string Url { get; set; }
    }
}
