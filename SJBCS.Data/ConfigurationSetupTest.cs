using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.IO;

namespace SJBCS.Data
{
    class ConfigurationSetupTest
    {
        [Test]
        public void CreateConfig()
        {
            var config = new Config()
            {
                AppConfiguration = new AppConfiguration()
                {
                    Settings = new Settings()
                    {
                        DataSource = new DataSource()
                        {
                            Metadata = "res://*/AmsModel.csdl|res://*/AmsModel.ssdl|res://*/AmsModel.msl",
                            Hostname = "DESKTOP-BQK5NF7",
                            InitialCatalog = "AMS",
                            Username = "amsadmin",
                            Password = "password1",
                        },
                        SmsService = new SmsService()
                        {
                            Url = "http://192.168.43.1:8080/"
                        }
                    }
                }
            };

            string json = JsonConvert.SerializeObject(config);
            Console.WriteLine(json);
            File.WriteAllText("config.json", json);
        }

        [Test]
        public void LoadConfig()
        {
            string json = File.ReadAllText("config.json");
            Config config = JsonConvert.DeserializeObject<Config>(json);
            Console.WriteLine(config.AppConfiguration.Settings.DataSource.InitialCatalog);
        }

        [Test]
        public void ModifyConfig()
        {
            string json = File.ReadAllText("config.json");
            Config config = JsonConvert.DeserializeObject<Config>(json);
            config.AppConfiguration.Settings.DataSource.InitialCatalog = "AMS";
            json = JsonConvert.SerializeObject(config);
            File.WriteAllText("config.json", json);
        }

        [Test]
        public void TestTime ()
        {
            TimeSpan timeOut = new TimeSpan(8, 0, 1);
            TimeSpan endTime = new TimeSpan(7, 30, 0);
            if (timeOut > endTime.Add(new TimeSpan(0, 30, 0)))
                Console.WriteLine("Overstay.");
            Console.WriteLine(timeOut);
            Console.WriteLine(endTime.Add(new TimeSpan(0, 30, 0)));
        }
    }
}
