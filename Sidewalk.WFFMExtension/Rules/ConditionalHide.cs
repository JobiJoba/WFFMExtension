using Sitecore.Form.Core.Utility;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Sidewalk.WFFMExtension.Rules
{
    public class ConditionalHide<T> : StringOperatorCondition<T> where T : RuleContext
    {
        /// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public string Value
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the name of the field.
		/// </summary>
		/// <value>The name of the field.</value>
		public string SessionVariable
		{
			get;
			set;
		}
		/// <summary>
		/// Executes the specified rule context.
		/// </summary>
		/// <param name="ruleContext">The rule context.</param>
		/// <returns>
		///  <c>True</c>, if the condition succeeds, otherwise <c>false</c>.
		/// </returns>
		protected override bool Execute(T ruleContext)
		{
		    if (Value == null || string.IsNullOrEmpty(Value))
		        return false;

            string value = Value;

            string value2 = SessionUtil.GetSessionValue<string>(SessionVariable) ?? string.Empty;

		    if (ruleContext.Parameters.ContainsKey("selectedValue"))
		    {
		        value2 = ruleContext.Parameters["selectedValue"].ToString();
		    }

            return Compare(value2, value);
		    
		}
    }
}
