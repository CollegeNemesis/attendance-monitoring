using SJBCS.Services.Repository;
using Unity;
using Unity.Lifetime;

namespace AMS.Utilities
{
    public static class ContainerHelper
    {
        private static IUnityContainer _container;

        static ContainerHelper()
        {
            _container = new UnityContainer();
            _container.RegisterType<IAttendancesRepository, AttendancesRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IBiometricsRepository, BiometricsRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IContactsRepository, ContactsRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IDistributionListsRepository, DistributionListsRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILevelsRepository, LevelsRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IOrganizationsRepository, OrganizationsRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRelBiometricsRepository, RelBiometricsRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRelDistributionListsRepository, RelDistributionListsRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRelOrganizationsRepository, RelOrganizationsRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISectionsRepository, SectionsRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IStudentsRepository, StudentsRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IUsersRepository, UsersRepository>(new ContainerControlledLifetimeManager());
        }

        public static IUnityContainer Container
        {
            get { return _container; }
        }
    }
}
