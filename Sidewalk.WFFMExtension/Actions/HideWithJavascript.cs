using Sitecore.Form.Core.Utility;
using Sitecore.Forms.Core.Rules;

namespace Sidewalk.WFFMExtension.Actions
{
    public class HideWithJavascript<T> : Sitecore.Rules.Actions.RuleAction<T> where T : ConditionalRuleContext
    {
        public override void Apply(T ruleContext)
        {
            Sitecore.Diagnostics.Assert.ArgumentNotNull(ruleContext, "ruleContext");
            if (ruleContext.Control != null)
            {
                ((System.Web.UI.WebControls.WebControl)ruleContext.Control).Attributes.Add("style", "display:none;");
                return;
            }
            if (ruleContext.Model != null)
            {
                ReflectionUtils.SetProperty(ruleContext.Model, "Visible", false);
            }
        }
    }
}
