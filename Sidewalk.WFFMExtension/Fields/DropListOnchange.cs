using Sidewalk.WFFMExtension.Helpers;
using Sitecore.Form.Core.Attributes;
using Sitecore.Form.Core.Visual;
using System;
using System.ComponentModel;

namespace Sidewalk.WFFMExtension.Fields
{
    public class DropListOnchange : Sitecore.Form.Web.UI.Controls.DropList
    {
        [VisualCategory("WFFM Extension")]
        [VisualFieldType(typeof(EditField)), VisualProperty("Session variable name", 1), DefaultValue("Session variable name")]
        public string SessionName { get; set; }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            droplist.CssClass = "scfDropList js-onchange";
            droplist.Attributes.Add("data-sessionname", SessionName);

            string selectedValue = WFFMExtensionHelper.GetValueAndSet(ID, SessionName, SelectedValue);

            droplist.SelectedIndex =
                droplist.Items.IndexOf(droplist.Items.FindByValue(selectedValue));
        }

    }
}
