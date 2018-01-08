using MaterialDesignThemes.Wpf;
using SJBCS.Model;
using SJBCS.View;
using SJBCS.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SJBCS.ViewModel
{
    public class SectionViewModel : INotifyPropertyChanged
    {
        private static AMSEntities DBContext = new AMSEntities();
        private ObservableCollection<Object> _levelList;
        private ObservableCollection<Object> _sectionList;
        private Section _section = new Section();
        private Level _level = new Level();
        private SectionWrapper _sectionWrapper = new SectionWrapper();
        private LevelWrapper _levelWrapper = new LevelWrapper();
        private string _status;



        public SectionViewModel()
        {
            _section = new Section();
            _level = new Level();
            _sectionWrapper = new SectionWrapper();
            _levelWrapper = new LevelWrapper();
            _sectionList = _sectionWrapper.RetrieveAll(DBContext, _section);
            _levelList = _levelWrapper.RetrieveAll(DBContext, _level);
            AddCommand = new CommandImplementation(Add);
            EditCommand = new CommandImplementation(Update);

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }

        public string Status
        {
            get { return _status; }
        }
        public ObservableCollection<Object> SectionList
        {
            get
            {
                return _sectionList;
            }
            set
            {
                _sectionList = value;
            }
        }

        public ObservableCollection<Object> LevelList
        {
            get
            {
                return _levelList;
            }
            set
            {
                _levelList = value;
            }
        }

        public string SectionName
        {
            get { return _section.SectionName; }
            set { _section.SectionName = value; }
        }

        public Level GradeLevel
        {
            set { _section.LevelID = value.LevelID; }
        }

        public string StartTime
        {
            get { return _section.StartTime.ToString(); }
            set
            {
                _section.StartTime = new TimeSpan((Convert.ToDateTime(value)).Hour, (Convert.ToDateTime(value)).Minute, (Convert.ToDateTime(value)).Second);
            }
        }

        public string EndTime
        {
            get { return _section.EndTime.ToString(); }
            set
            {
                _section.EndTime = new TimeSpan((Convert.ToDateTime(value)).Hour, (Convert.ToDateTime(value)).Minute, (Convert.ToDateTime(value)).Second);
            }
        }

        private void Add(object obj)
        {
            _sectionWrapper.Add(DBContext, _section);
            _status = "Section Added.";
            RefreshBindings();
        }

        private void RefreshBindings()
        {
            _section = new Section();
            _level = new Level();
            _sectionWrapper = new SectionWrapper();
            _levelWrapper = new LevelWrapper();
            _sectionList = _sectionWrapper.RetrieveAll(DBContext, _section);
            _levelList = _levelWrapper.RetrieveAll(DBContext, _level);
            RaisePropertyChanged(null);
        }

        private void Update(object obj)
        {
            _sectionWrapper.Add(DBContext, _section);
            _sectionList = _sectionWrapper.RetrieveAll(DBContext, _section);
        }

        private void RaisePropertyChanged(string v)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(v));
            }
        }
    }
}

