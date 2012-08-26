using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
namespace Moo.Controls
{
    [ParseChildren(true, "Text"), PersistChildren(false)]
    public class LinkBarItem
    {
        public string Text { get; set; }
        public string URL { get; set; }
        public bool Selected { get; set; }
        public bool Shortcut { get; set; }
        public bool Special { get; set; }
        public bool Hidden { get; set; }

        public event EventHandler DataBinding = new EventHandler((foo, bar) => { });
        public object BindingContainer { get { return null; } }
        public void DataBind()
        {
            DataBinding(this, new EventArgs());
        }
    }
}