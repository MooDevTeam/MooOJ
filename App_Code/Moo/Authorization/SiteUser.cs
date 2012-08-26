using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using Moo.DB;
namespace Moo.Authorization
{
    /// <summary>
    /// 请求期间的简略当前用户登录信息
    /// </summary>
    public class SiteUser : IIdentity
    {
        public string AuthenticationType { get { return "Custom"; } }
        public bool IsAuthenticated { get { return true; } }

        public int ID { get; set; }
        public int Token { get; set; }
        public string Name { get; set; }
        public SiteRole Role { get; set; }

        SiteUser() { }

        public SiteUser(User user)
        {
            ID = user.ID;
            Name = user.Name;
            Role = SiteRoles.ByID[user.Role.ID];
        }

        public User GetDBUser(MooDB db)
        {
            return (from u in db.Users
                    where u.ID == ID
                    select u).Single<User>();
        }

        /*
        public string Serialize()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ID).Append(',');
            sb.Append(Name).Append(',');
            sb.Append(Role.ID);
            return sb.ToString();
        }

        public static SiteUser Deserialize(string serialized)
        {
            string[] fields = serialized.Split(',');
            string[] roles = fields[2].Split(':');

            SiteUser result = new SiteUser();
            result.ID = int.Parse(fields[0]);
            result.Name = fields[1];
            result.Role = SiteRoles.ByID[int.Parse(fields[2])];
            return result;
        }

        public override string ToString()
        {
            return Serialize();
        }
         * */
    }
}