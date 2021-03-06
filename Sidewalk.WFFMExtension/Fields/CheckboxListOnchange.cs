﻿using Sidewalk.WFFMExtension.Helpers;
using Sitecore.Form.Core.Attributes;
using Sitecore.Form.Core.Visual;
using System;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sidewalk.WFFMExtension.Fields
{
    public class CheckboxListOnchange : Sitecore.Form.Web.UI.Controls.CheckboxList
    {
        public CheckboxListOnchange()
            : this(HtmlTextWriterTag.Div)
        {
        }

        public CheckboxListOnchange(HtmlTextWriterTag tag)
            : base(tag)
        {
        }

        [VisualCategory("WFFM Extension")]
        [VisualFieldType(typeof(EditField)), VisualProperty("Session variable name", 1), DefaultValue("Session variable name")]
        public string SessionName { get; set; }



        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            generalPanel.CssClass += " cb-onchange";

            foreach (ListItem button in buttonList.Items)
            {
                button.Attributes.Add("class", "js-onchange");
                button.Attributes.Add("data-sessionname", SessionName);
            }

            var selectedValue = WFFMExtensionHelper.GetValueAndSet(ID, SessionName, items);

            var splittedSelectedValue = selectedValue.Split(',').Select(o => o.Trim()).ToList();
            foreach (ListItem item in Items)
            {
                if (splittedSelectedValue.Contains(item.Value))
                {
                    item.Selected = true;
                }
            }
        }
    }
}
