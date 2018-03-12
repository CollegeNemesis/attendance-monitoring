using System;
using AMS.Utilities;
using SJBCS.Data;
using SJBCS.GUI.AMS;
using SJBCS.GUI.Dialogs;
using SJBCS.GUI.Home;
using SJBCS.GUI.Report;
using SJBCS.GUI.SMS;
using SJBCS.GUI.Student;
using Unity;

namespace SJBCS.GUI
{
    public class TestWindowViewModel : BindableBase
    {
        public EnrollBiometricsViewModel enrollBiometricsViewModel;

        private object model;
        public object Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }
        public TestWindowViewModel()
        {
            enrollBiometricsViewModel = ContainerHelper.Container.Resolve<EnrollBiometricsViewModel>();
        }
    }
}
