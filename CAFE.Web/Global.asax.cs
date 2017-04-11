using System;
using System.Globalization;
using System.Threading;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CAFE.DAL.Migrations;
using CronScheduling;
using System.Linq;
using System.IO;

namespace CAFE.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Error()
        {
            Exception ex = Server.GetLastError();

            FileStream fs = new FileStream(Server.MapPath("ExceptionsLog.txt"), FileMode.Append, FileAccess.Write);
            StreamWriter swr = new StreamWriter(fs);
            swr.Write("[" + DateTime.Now.ToString() + " ]: " + ex.ToString() + "\n\n");
            swr.Close();
        }
        protected void Application_Start()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

            //This code solving the problem related with plugins and configuration section.
            //In several plugins needs to get custom configuration section that serialize to type which
            //declared in plugin assembly and therefore when try to get config class instanse from config file
            //some excetion occurs because that type load after app initialized.
            AppDomain.CurrentDomain.AssemblyResolve += (o, args) =>
            {
                var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                return loadedAssemblies.FirstOrDefault(asm => asm.FullName == args.Name);
            };

            var container = UnityConfig.RegisterComponents();
            MapperConfig.Config(container);

            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Migrations
            CafeMigrator.Migrate();

            var cron = CronDaemon.Start<Action>(a =>
            {
                a();
            });

            cron.Add(() =>
            {
                VersionChecker.Instance.HaveNewVersion();
            }, Cron.Daily(), Int32.MaxValue);
            cron.Add(() =>
            {
                VersionChecker.Instance.HaveNewVersion();
            }, Cron.Minutely(), 1);

            GlobalConfiguration.Configuration
              .Formatters
              .JsonFormatter
              .SerializerSettings
              .ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();

            GlobalConfiguration.Configuration
            .Formatters
            .JsonFormatter
            .SerializerSettings
            .NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        }
    }
}
