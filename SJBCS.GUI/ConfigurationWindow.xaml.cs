using Newtonsoft.Json;
using SJBCS.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SJBCS.GUI
{
    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window
    {
        public ConfigurationWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string json = File.ReadAllText(ConfigurationManager.AppSettings["configPath"]);
            Config config = JsonConvert.DeserializeObject<Config>(json);

            config.AppConfiguration.Settings.DataSource.InitialCatalog = "AMS";

            json = JsonConvert.SerializeObject(config);
            File.WriteAllText(ConfigurationManager.AppSettings["configPath"], json);

            this.Close();
        }
    }
}
