using LogicUniversityTeam5.IdentityHelper;
using LogicUniversityTeam5.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LogicUniversityTeam5.Controllers
{
    public class ChangeRoleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ChangeRoleController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        private void ChangeRoleOfUser(string newRoleName, string employeeId)
        {
            if (employeeId != null)
            {
                var user = _userManager.FindByEmployeeID(employeeId);
                string[] allUserRoles = _userManager.GetRoles(user.Id).ToArray();
                string[] newRole = new string[] { newRoleName };
                _userManager.RemoveFromRoles(user.Id, allUserRoles);
                _userManager.AddToRoles(user.Id, newRole);
                user.UserRole = newRoleName;
                _userManager.Update(user);

            }
        }

        public void ChangeRoleOfUserToDepartmentRep(string employeeId)
        {
            string newRoleName = "Department Representative";
            ChangeRoleOfUser(newRoleName, employeeId);
        }

        public void ChangeRoleOfUserToDelegate(string employeeId)
        {
            string newRoleName = "Delegate";
            ChangeRoleOfUser(newRoleName, employeeId);
        }

        public void ChangeRoleOfUserToEmployee(string employeeId)
        {
            string newRoleName = "Employee";
            ChangeRoleOfUser(newRoleName, employeeId);
        }

    }
}