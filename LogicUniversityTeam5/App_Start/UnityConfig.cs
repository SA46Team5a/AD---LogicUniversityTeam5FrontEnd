using LogicUniversityTeam5.Controllers;
using LogicUniversityTeam5.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Mvc5;
using ServiceLayer;
using System.Net.Http.Headers;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;

namespace LogicUniversityTeam5
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IStockManagementService, StockManagementService>();
            container.RegisterType<IClassificationService, ClassificationService>();
            container.RegisterType<IDepartmentService, DepartmentService>();
            container.RegisterType<IRequisitionService, RequisitionService>();
            container.RegisterType<IDisbursementService, DisbursementService>();
            container.RegisterType<IOrderService, OrderService>();
            container.RegisterType<IReportService, ReportService>();
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();
            container.RegisterType<DbContext, ApplicationDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<UserManager<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<AccountController>(new InjectionConstructor());

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }

    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterType<IClassificationService, ClassificationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IDepartmentService, DepartmentService>(new HierarchicalLifetimeManager());
            container.RegisterType<IRequisitionService, RequisitionService>(new HierarchicalLifetimeManager());
            container.RegisterType<IDisbursementService, DisbursementService>(new HierarchicalLifetimeManager());
            container.RegisterType<IOrderService, OrderService>(new HierarchicalLifetimeManager());
            container.RegisterType<IReportService, ReportService>(new HierarchicalLifetimeManager());
            container.RegisterType<IStockManagementService, StockManagementService>();

            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            config.DependencyResolver = new UnityResolver(container);

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}
