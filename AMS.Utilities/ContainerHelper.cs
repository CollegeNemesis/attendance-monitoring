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
            _container.RegisterType<IUsersRepository, UsersRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IStudentsRepository, StudentsRepository>(new ContainerControlledLifetimeManager());
        }

        public static IUnityContainer Container
        {
            get { return _container; }
        }
    }
}
