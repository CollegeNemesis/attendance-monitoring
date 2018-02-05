using SJBCS.Model;
using SJBCS.Wrapper;
using SJBCS.ViewModel;
using SJBCS.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;

namespace SJBCS.ViewModel
{
    class ManageBiometricsViewModel: INotifyPropertyChanged
    {
        private string _studentID;
        private String _notificationHeight;
        private AMSEntities DBContext;
        private RelBiometricWrapper _relBiometricWrapper;
        private ObservableCollection<MenuItem> _fingerprintList;
        private ObservableCollection<Object> _fingerList;

        public ICommand OpenBiometricsEnrollmentViewCommand => new CommandImplementation(OpenBiometricsEnrollmentView);
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Object> FingerList
        {
            get
            {
                return _fingerList;
            }
        }
        public String NotificationHeight
        {
            get
            {
                return _notificationHeight;
            }
            set
            {
                _notificationHeight = value;
            }
        }
        public ManageBiometricsViewModel(string studentID)
        {
            DBContext = new AMSEntities();
            _studentID = studentID;
            _relBiometricWrapper = new RelBiometricWrapper();
            _fingerList = _relBiometricWrapper.RetrieveViaKeyword(DBContext, studentID, _studentID);
            _fingerprintList = new ObservableCollection<MenuItem>();
            if (_fingerList.FirstOrDefault() == null)
            {
                Console.WriteLine("No records");
                _notificationHeight = "Auto";
            }
            else
            {

                _notificationHeight = "0";
            }
            _fingerprintList = null;
        }

        private void RaisePropertyChanged(string v)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(v));
            }
        }
        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }

        private async void OpenBiometricsEnrollmentView(Object obj)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new BiometricsEnrollmentView
            {
                DataContext = new BiometricsEnrollmentViewModel(_studentID)
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ExtendedOpenedEventHandler, ExtendedClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
            RaisePropertyChanged(null);
        }


        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }
        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
        }
        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false) return;

            //OK, lets cancel the close...
            eventArgs.Cancel();

            //...now, lets update the "session" with some new content!
            eventArgs.Session.UpdateContent(new AddStudentView());
            //note, you can also grab the session when the dialog opens via the DialogOpenedEventHandler

            //lets run a fake operation for 3 seconds then close this baby.
            Task.Delay(TimeSpan.FromSeconds(3))
                .ContinueWith((t, _) => eventArgs.Session.Close(false), null,
                    TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
