using System;
using SJBCS.Util;

namespace SJBCS.Students
{
    class StudentViewModel
    {
        #region UI Setup
        public StudentListViewModel _studentListViewModel = new StudentListViewModel();
        public StudentAddViewModel _studentAddViewModel = new StudentAddViewModel();
        public StudentUpdateViewModel _studentUpdateViewModel = new StudentUpdateViewModel();
        #endregion

        public StudentViewModel()
        {
            NavigateCommand = new RelayCommand<string>(OnNavigate);
        }
        private BindableBase _CurrentViewModel;
        public BindableBase CurrentViewModel
        {
            get { return _CurrentViewModel; }
            set { SetProperty(ref _CurrentViewModel, value); }
        }

        private void SetProperty(ref BindableBase currentViewModel, BindableBase value)
        {
            throw new NotImplementedException();
        }

        public RelayCommand<string> NavigateCommand { get; set; }

        private void OnNavigate(string destination)
        {
            switch (destination)
            {
                case "studentList":
                    CurrentViewModel = _studentListViewModel;
                    break;
                case "studentAdd":
                    CurrentViewModel = _studentAddViewModel;
                    break;
                case "studentUpdate":
                    CurrentViewModel = _studentUpdateViewModel;
                    break;

                default:
                    break;
            }
        }
    }
}
