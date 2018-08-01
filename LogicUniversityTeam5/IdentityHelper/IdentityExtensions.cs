using LogicUniversityTeam5.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace LogicUniversityTeam5.IdentityHelper
{
    public static class IdentityExtensions
    {
        public static string GetEmployeeId(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            var ci = identity as ClaimsIdentity;
            if (ci != null)
            {
                return ci.FindFirstValue("EmployeeId");
            }
            return null;
        }

        public static string GetUserRole(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            var ci = identity as ClaimsIdentity;
            if (ci != null)
            {
                return ci.FindFirstValue("UserRole");
            }
            return null;
        }

        public static ApplicationUser FindByEmployeeID(this UserManager<ApplicationUser> um, string employeeId)
        {
            return um?.Users?.SingleOrDefault(x => x.EmployeeId == employeeId);
        }

    }
}