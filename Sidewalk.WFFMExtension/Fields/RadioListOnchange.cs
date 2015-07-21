using Sidewalk.WFFMExtension.Helpers;
using Sitecore.Form.Core.Attributes;
using Sitecore.Form.Core.Visual;
using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sidewalk.WFFMExtension.Fields
{
    public class RadioListOnchange : Sitecore.Form.Web.UI.Controls.RadioList
    {
        public RadioListOnchange()
            : this(HtmlTextWriterTag.Div)
        {
        }

        public RadioListOnchange(HtmlTextWriterTag tag)
            : base(tag)
        {
        }

        [VisualCategory("WFFM Extension")]
        [VisualFieldType(typeof(EditField)), VisualProperty("Session variable name", 1), DefaultValue("Session variable name")]
        public string SessionName { get; set; }



     
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            foreach (ListItem button in buttonList.Items)
            {
                button.Attributes.Add("class", "js-onchange");
            }
            string selectedValue =  WFFMExtensionHelper.GetValueAndSet(ID, SessionName, SelectedValue);

            buttonList.SelectedIndex =
            buttonList.Items.IndexOf(buttonList.Items.FindByValue(selectedValue));
        }
    }
}
