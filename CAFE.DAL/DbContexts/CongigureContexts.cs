
using System.Data.Entity;
using Microsoft.Practices.Unity;

namespace CAFE.DAL.DbContexts
{
    public class CongigureContexts
    {
        public static void ConfigureDependencies(IUnityContainer container, LifetimeManager lifetime)
        {
            container.RegisterType<DbContext, ApplicationDbContext>(lifetime);
        }
    }
}
