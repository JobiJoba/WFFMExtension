using Sitecore.Form.Core.Utility;
using Sitecore.Forms.Core.Rules;

namespace Sidewalk.WFFMExtension.Rules
{
    public class ReadValueFromSessionVariable<T> : ReadValue<T> where T : ConditionalRuleContext
    {
        protected override object GetValue()
        {
            return SessionUtil.GetSessionValue<string>(Name) ?? string.Empty;
        }
    }
}
