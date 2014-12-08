using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using cicaudittrail.Models;
using System.Diagnostics;

namespace cicaudittrail.Src
{
    public class CustomRoleProvider : RoleProvider
    {
        //not implemented
        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override string[] GetRolesForUser(string username)
        {
            Debug.WriteLine("GetRolesForUser username = " + username);
            System.Web.Security.MembershipUser CurrentUser = System.Web.Security.Membership.GetUser();
            Debug.WriteLine("GetRolesForUser CurrentUser = " + CurrentUser);
            if (CurrentUser != null) username = CurrentUser.Email;
            Debug.WriteLine("GetRolesForUser username final = " + username);

            var Cicuserrolerepository = new CicUserRoleRepository();
            return Cicuserrolerepository.FindRoleNamesByUser(username);
        }


        public override bool IsUserInRole(string username, string roleName)
        {
            System.Web.Security.MembershipUser CurrentUser = System.Web.Security.Membership.GetUser();
            if (CurrentUser != null) username = CurrentUser.Email;

            var Cicuserrolerepository = new CicUserRoleRepository();
            return Cicuserrolerepository.IsUserInRole(username, roleName);
        }

        public static bool IsUserInRole(string roleName)
        {
            var username = "";
            System.Web.Security.MembershipUser CurrentUser = System.Web.Security.Membership.GetUser();
            if (CurrentUser != null) username = CurrentUser.Email;

            var Cicuserrolerepository = new CicUserRoleRepository();
            return Cicuserrolerepository.IsUserInRole(username, roleName);
        }

        // not implemented yet
        public override string[] GetAllRoles()
        {
            return GetRolesForUser("");
        }


        // not implemented yet
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return GetRolesForUser("");
        }

        // not implemented yet
        public override string[] GetUsersInRole(string roleName)
        {
            return GetRolesForUser("");
        }


        // not implemented yet
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {

        }

        // not implemented yet
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {

        }

        // not implemented yet
        public override bool RoleExists(string roleName)
        {
            return false;
        }


        // not implemented yet
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            return true;
        }



        // not implemented yet
        public override void CreateRole(string roleName)
        {

        }


    }
}

