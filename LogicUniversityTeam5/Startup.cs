using System;
using System.Web.Http;
using System.Web.Http.Cors;
using LogicUniversityTeam5.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartupAttribute(typeof(LogicUniversityTeam5.Startup))]
namespace LogicUniversityTeam5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            ConfigureAuth(app);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);

            ConfigureAuth(app);
            createRoles();
        }

        private void createRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            string[] roles = new string[] { "Employee", "Department Head", "Department Representative",
                                            "Delegate", "Store Clerk", "Store Supervisor", "Store Manager" };

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            foreach(string empRole in roles)
            {
                if (!roleManager.RoleExists(empRole))
                {
                    var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                    role.Name = empRole;
                    roleManager.Create(role);
                }
            }
        }

    }
}
