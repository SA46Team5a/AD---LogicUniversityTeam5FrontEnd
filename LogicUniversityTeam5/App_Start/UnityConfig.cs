using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using ServiceLayer;
using System.Net.Http.Headers;
using System.Web.Http;
using Unity.Lifetime;

namespace LogicUniversityTeam5
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            //container.RegisterType<IStockManagementService, StockManagementService>();
            container.RegisterType<IClassificationService, ClassificationService>();
            container.RegisterType<IDepartmentService, DepartmentService>();
            container.RegisterType<IRequisitionService, RequisitionService>();
            container.RegisterType<IDisbursementService, DisbursementService>();
            container.RegisterType<IOrderService, OrderService>();
            container.RegisterType<IReportService, ReportService>();

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

            config.DependencyResolver = new UnityResolver(container);

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SupportedMediaTypes
                .Add(new MediaTypeHeaderValue("text/html"));

        }
    }
}
