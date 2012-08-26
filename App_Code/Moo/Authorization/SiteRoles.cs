using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Moo.DB;
namespace Moo.Authorization
{
    /// <summary>
    /// 长期驻留的所有用户组信息
    /// </summary>
    public static class SiteRoles
    {
        public static Dictionary<int, SiteRole> ByID { get; set; }
        public static Dictionary<RoleType, SiteRole> ByType { get; set; }
        public static void Initialize()
        {
            ByID = new Dictionary<int, SiteRole>();
            ByType = new Dictionary<RoleType, SiteRole>();

            using (MooDB db = new MooDB())
            {
                IEnumerable<Role> roles = from r in db.Roles
                                          select r;
                foreach (Role role in roles)
                {
                    SiteRole siteRole=new SiteRole(role);
                    ByID.Add(siteRole.ID, siteRole);
                    ByType.Add(siteRole.Type, siteRole);
                }
            }
        }
        public static Dictionary<int,SiteRole>.ValueCollection GetRoles()
        {
            return ByID.Values;
        }
    }
}