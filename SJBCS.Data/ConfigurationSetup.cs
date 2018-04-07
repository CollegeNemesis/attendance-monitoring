namespace SJBCS.Data
{
    public class Config
    {
        public AppConfiguration AppConfiguration { get; set; }
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
