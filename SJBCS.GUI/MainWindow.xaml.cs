using AMS.Utilities;
using System;
using System.Threading;
using System.Windows;
using Unity;

namespace SJBCS.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static System.Threading.Mutex singleton = new Mutex(true, "SJBCS");

        public MainWindow()
        {
            if (!singleton.WaitOne(TimeSpan.Zero, true))
            {
                MessageBox.Show("Another instance of this application is running.");
                this.Close();
            }
            InitializeComponent();
            DataContext = ContainerHelper.Container.Resolve<MainWindowViewModel>();
        }
    }
}
