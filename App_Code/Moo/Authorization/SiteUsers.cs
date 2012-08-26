using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Moo.Authorization
{
    /// <summary>
    ///SiteUser 管理
    /// </summary>
    public static class SiteUsers
    {
        public static Dictionary<int, SiteUser> ByID { get; set; }

        static SiteUsers(){
            ByID = new Dictionary<int, SiteUser>();
        }
    }
}