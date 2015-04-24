using System.Web.UI;

namespace Sidewalk.WFFMExtension.FormRender
{
    public class AddOnClientClickFormRender : Sitecore.Form.Core.Renderings.FormRender
    {
        protected override void DoRender(HtmlTextWriter output)
        {

            base.DoRender(output);

            if (System.Web.HttpContext.Current.Items["IncludeExtension"] == null)
            {
                output.Write("<script type=\"text/javascript\" src=\"/sitecore modules/Web/WFFM Extension/wffmExtension.js\"></script>");
            }
            System.Web.HttpContext.Current.Items["IncludeExtension"] = true;
        }
    }
}