using Sitecore.Form.Core.Attributes;
using Sitecore.Form.Core.Utility;
using Sitecore.Form.Core.Visual;
using System;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Sidewalk.WFFMExtension.Fields
{
    public class SessionedSigneLineText : Sitecore.Form.Web.UI.Controls.SingleLineText
    {
        public SessionedSigneLineText()
            : this(HtmlTextWriterTag.Div)
        {
        }

        public SessionedSigneLineText(HtmlTextWriterTag tag)
            : base(tag)
        {
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
