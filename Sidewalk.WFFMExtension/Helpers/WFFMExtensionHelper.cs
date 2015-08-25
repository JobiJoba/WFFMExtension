using Sidewalk.WFFMExtension.Resources;
using Sitecore.Diagnostics;
using Sitecore.Form.Core.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ListItemCollection = Sitecore.Form.Web.UI.Controls.ListItemCollection;

namespace Sidewalk.WFFMExtension.Helpers
{
    public class WFFMExtensionHelper
    {
        private static Dictionary<string, string> _fieldList;
        /// <summary>
        /// This property return all the field type contained in the root folder of Field Types of WFFM
        /// </summary>
        public static Dictionary<string, string> FieldList
        {
            get
            {
                if (_fieldList == null)
                {
                    //TODO Add that to a settings
                    var rootFolder = Sitecore.Context.Database.GetItem(Constants.WffmFieldTypesRootFolderId);
                    var fields = rootFolder.Axes.GetDescendants()
                        .Where(o => o.TemplateID.Equals(Constants.WffmFieldTypeTemplateId)).Select(o => o["class"]);
                    _fieldList = fields.Distinct().ToDictionary(o => o);

                }
                return _fieldList;

            }
            set { _fieldList = value; }
        }

        /// <summary>
        /// Return the value of a field using the Form or the session variable and then set it into the session
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sessionName"></param>
        /// <param name="selectedValueFromComponent"></param>
        /// <returns></returns>
        public static string GetValueAndSet(string id, string sessionName, ListItemCollection selectedValueFromComponent)
        {
            Assert.IsNotNullOrEmpty(sessionName, "Session name cannot be empty");
            string selectedValue;
            var key = HttpContext.Current.Request.Form.AllKeys.FirstOrDefault(o => o.Contains(id));
            if (key != null)
            {
                selectedValue = HttpContext.Current.Request.Form[key];
                SessionUtil.SetSessionValue(sessionName, selectedValue);
            }
            else
            {
                selectedValue = SessionUtil.GetSessionValue<string>(sessionName);
                if (selectedValueFromComponent != null && selectedValueFromComponent.Count > 0)
                {
                    if (string.IsNullOrEmpty(selectedValue) || selectedValue.Equals(selectedValueFromComponent[0].Value))
                    {
                        selectedValue = selectedValueFromComponent[0].Value;
                        SessionUtil.SetSessionValue(sessionName, selectedValue);
                    }
                    else
                    {
                        SessionUtil.SetSessionValue(sessionName, selectedValue);
                    }
                }
                else
                {
                    SessionUtil.SetSessionValue(sessionName, string.Empty);
                }
            }
            return selectedValue;
        }

        /// <summary>
        /// Check if a control has a display none style 
        /// </summary>
        /// <param name="webControl"></param>
        /// <returns></returns>
        public static bool IsDisplayNone(Control webControl)
        {
            var isDisplayNone = false;

            var findValidControls = FindValidControls(webControl);

            var findedWebControl = ((WebControl)findValidControls);
            if (findedWebControl != null)
            {
                var fieldStyle = findedWebControl.Style.Value;

                if (!string.IsNullOrWhiteSpace(fieldStyle) && fieldStyle.Contains("display:none;"))
                {
                    isDisplayNone = true;
                }
            }

            return isDisplayNone;
        }

        /// <summary>
        /// Try to search a wffm control (Taking control from the Field List) - In case of a section, it will return the section control
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static Control FindValidControls(Control control)
        {
            if (control == null)
            {
                return null;
            }
            var controlSection = FindValidControlsForSection(control);
            if (controlSection != null)
            {
                return controlSection;
            }

            if (FieldList.ContainsKey(control.GetType().FullName))
            {
                return control;
            }
            return FindValidControls(control.Parent);
        }

        /// <summary>
        /// Return the first control from a control which have a Style.Value - Used in case of a section
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static Control FindValidControlsForSection(Control control)
        {
            if (control == null)
            {
                return null;
            }
            var controlSection = control as WebControl;
            if (controlSection != null && controlSection.Style.Value != null)
            {
                return control;
            }
            return FindValidControls(control.Parent);
        }
    }
}
