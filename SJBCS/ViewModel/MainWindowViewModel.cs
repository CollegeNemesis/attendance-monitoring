using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using SJBCS.View;
using SJBCS.Model;

namespace SJBCS.ViewModel
{
    class MainWindowViewModel
    {
        public MainWindowViewModel(ISnackbarMessageQueue snackbarMessageQueue)
        {
            if (snackbarMessageQueue == null) throw new ArgumentNullException(nameof(snackbarMessageQueue));

            MenuItems = new[]
            {
                new MenuItem("Home", new HomeView{DataContext = new HomeViewModel()}),
                new MenuItem("SMS Simulcast", new SMSView()),
                new MenuItem("Students", new StudentView{DataContext = new StudentViewModel() }),
                new MenuItem("Sections", new SectionView{DataContext = new SectionViewModel()}),
                new MenuItem("Clubs and Organizations", new ClubOrgView()),
                new MenuItem("Reports", new ReportView())
            };
        }

        public MenuItem[] MenuItems { get; }
    }
}
