﻿using System;
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
            Initialize(user);
        }

        public User GetDBUser(MooDB db)
        {
            return (from u in db.Users
                    where u.ID == ID
                    select u).Single<User>();
        }

        public void Initialize(User user)
        {
            ID = user.ID;
            Name = user.Name;
            Role = SiteRoles.ByID[user.Role.ID];
        }
    }
}