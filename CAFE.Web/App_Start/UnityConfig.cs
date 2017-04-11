using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;
using System.Web.Http;
using CAFE.Core;
using CAFE.Core.Configuration;
using CAFE.Core.Integration;
using CAFE.Core.Messaging;
using CAFE.Core.Notification;
using CAFE.Core.Plugins;
using CAFE.Core.Resources;
using CAFE.Core.Searching;
using CAFE.Core.Security;
using CAFE.DAL.DbContexts;
using CAFE.DAL.Interfaces;
using CAFE.DAL.Models;
using CAFE.DAL.Repositories;
using CAFE.Services;
using CAFE.Services.Integration;
using CAFE.Services.Messaging;
using CAFE.Services.Plugins;
using CAFE.Services.Resources;
using CAFE.Services.Searching;
using CAFE.Services.Security;
using CAFE.Web.Configuration;
using CAFE.Web.Notification;
using CAFE.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using NLog;
using Unity.WebApi;

namespace CAFE.Web
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> _container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return _container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

           // GlobalConfiguration.Configuration.Filters.Add(new AnnotationItemValidateAttribute());

            container.RegisterInstance(GlobalConfiguration.Configuration);

            container.RegisterType<IConfigurationProvider, WebConfigConfigurationProvider>();

            container.RegisterType<IEmailService, IdentityEmailService>();
            container.RegisterType<ISmsService, IdentitySmsService>();

            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));

            container.RegisterType<ISecurityService, SecurityService>();
            container.RegisterType<ISecurityServiceAsync, SecurityService>();

            container.RegisterType<IEmailTemplatesRenderer, SimpleEmailTemplatesRenderer>();

            container.RegisterType<ISearchService, SearchService>();

            container.RegisterType<IConversationService, ConversationService>();
            container.RegisterType<IAccessRequestService, AccessRequestService>();
            container.RegisterType<IAccessibleResourcesProvider, AccessibleResourcesProvider>();
            container.RegisterType<IExtensibleVocabularyService, ExtensibleVocabularyService>();
            
            container.RegisterType<IVocabularyService, VocabularyService>();

            container.RegisterType<IAuthenticationManager>(
                new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));
            container.RegisterType<IUserStore<User>, RepositoryBasedUserStore>();
            container.RegisterType<UserManager<User>, ApplicationUserManager>();
            container.RegisterType<SignInManager<User, string>, ApplicationSignInManager>();

            container.RegisterType<IAnnotationItemIntegrationService, XmlAnnotationItemIntegrationService>();
            container.RegisterType<IUserDataIntegrationService, ArchiveUserDataIntegrationService>();

            container.RegisterInstance(typeof (ILogger), LogManager.GetLogger("Common"));

            CongigureContexts.ConfigureDependencies(container, new PerRequestLifetimeManager());

            container.RegisterType<IPluginsProvider, MefPluginsProvider>();


        }
        public static IUnityContainer RegisterComponents()
        {

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(_container.Value);
            return _container.Value;
        }
    }
}
