using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;
using Moo.DB;
namespace Moo.Text
{
    /// <summary>
    /// Wiki文本解析
    /// </summary>
    public static class WikiParser
    {
        public static string Parse(string text)
        {
            return new WikiPlex.WikiEngine().Render(text);
        }

        class UserComparer : IComparer<User>
        {
            public int Compare(User a, User b)
            {
                return a.ID - b.ID;
            }
        }

        public static string DoAt(MooDB db, string text, Post post, User currentUser, bool sendMail)
        {
            SortedSet<User> userBeAt = new SortedSet<User>(new UserComparer());
            StringBuilder sb = new StringBuilder(text);
            for (int i = 0; i < sb.Length; i++)
            {
                if (sb[i] == '@' && (i == 0 || char.IsWhiteSpace(sb[i - 1])) && i < sb.Length - 1 && !char.IsWhiteSpace(sb[i + 1]))
                {
                    int end = i + 1;
                    StringBuilder userNameSb = new StringBuilder();
                    while (end < sb.Length && !char.IsWhiteSpace(sb[end]))
                    {
                        userNameSb.Append(sb[end]);
                        end++;
                    }
                    string userName = userNameSb.ToString();
                    User user = (from u in db.Users
                                 where u.Name == userName
                                 select u).SingleOrDefault<User>();
                    if (user != null)
                    {
                        string link = "[url:@" + user.Name + "|../User/?id=" + user.ID + "]";
                        sb.Remove(i, end - i);
                        sb.Insert(i, link);
                        i += link.Length;
                        if (sendMail && !userBeAt.Contains(user))
                        {
                            userBeAt.Add(user);
                        }
                    }
                }
            }
            foreach (User user in userBeAt)
            {
                db.Mails.AddObject(new Mail()
                {
                    Title = "我@了您哦~",
                    Content = "我在帖子[url:" + post.Name + "|../Post/?id=" + post.ID + "]中*@*了您哦~快去看看！\r\n\r\n*原文如下*：\r\n" + sb.ToString(),
                    From = currentUser,
                    To = user,
                    IsRead = false
                });
            }
            return sb.ToString();
        }
    }
}