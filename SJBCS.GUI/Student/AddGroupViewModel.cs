using AMS.Utilities;
using SJBCS.Data;
using SJBCS.Services.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJBCS.GUI.Student
{
    public class AddGroupViewModel : BindableBase
    {
        private IOrganizationsRepository _organizationsRepository;

        private ObservableCollection<Organization> _organizations;

        public ObservableCollection<Organization> Organizations
        {
            get { return _organizations; }
            set { SetProperty(ref _organizations, value); }
        }


        public AddGroupViewModel(IOrganizationsRepository organizationsRepository)
        {
            _organizationsRepository = organizationsRepository;
            Organizations = new ObservableCollection<Organization>(_organizationsRepository.GetOrganizations());
        }
    }
}
