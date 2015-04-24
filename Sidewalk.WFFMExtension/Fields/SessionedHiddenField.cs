using System;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using Sitecore.Form.Core.Attributes;
using Sitecore.Form.Core.Utility;
using Sitecore.Form.Core.Visual;
using Sitecore.Form.Web.UI.Controls;

namespace Sidewalk.WFFMExtension.Fields
{
    public class SessionedHiddenField : SingleLineText
    {
        public SessionedHiddenField()
            : this(HtmlTextWriterTag.Div)
        {
        }
        public SessionedHiddenField(HtmlTextWriterTag tag)
            : base(tag)
        {
        }

        protected override void DoRender(HtmlTextWriter writer)
        {
            writer.AddStyleAttribute("display", "none");
            base.DoRender(writer);
        }

        [VisualCategory("WFFM Extension")]
        [VisualFieldType(typeof(EditField)), VisualProperty("Session variable name", 1), DefaultValue("Session variable name")]
        public string SessionName { get; set; }

       

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            var key = HttpContext.Current.Request.Form.AllKeys.FirstOrDefault(o => o.Contains(ID));
            if (key != null)
            {
                var selectedValue = HttpContext.Current.Request.Form[key];

                SessionUtil.SetSessionValue(SessionName, selectedValue);
            }
        }
    }
}
