using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Helper
{
    public static class UserUtil
    {
        public static String getCurrentUserName()
        {
            var currentUsername = !string.IsNullOrEmpty(System.Web.HttpContext.Current?.User?.Identity?.Name)
               ? HttpContext.Current.User.Identity.Name
               : "Anonymous";

            return currentUsername;
        }
    }
}