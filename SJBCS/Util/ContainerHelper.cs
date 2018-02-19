using SJBCS.Services;
using Unity;
using Unity.Lifetime;

namespace SJBCS.Util
{
    public static class ContainerHelper
    {
        private static IUnityContainer _container;

        static ContainerHelper()
        {
            _container = new UnityContainer();
            _container.RegisterType<IUsersRepository, UsersRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IStudentsRepository, StudentsRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISectionsRepository, SectionsRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILevelsRepository, LevelsRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IContactsRepository, ContactsRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IGroupsRepository, GroupsRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IBiometricsRepository, BiometricsRepository>(new ContainerControlledLifetimeManager());
        }

        public static IUnityContainer Container
        {
            get { return _container; }
        }
    }
}
