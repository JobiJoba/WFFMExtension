using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;
using Sidewalk.WFFMExtension.Enums;
using Sidewalk.WFFMExtension.Resources;
using Sidewalk.WFFMExtension.Rules;
using Sidewalk.WFFMExtension.WebserviceModels;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Form.Core.Utility;
using Sitecore.Rules;

namespace Sidewalk.WFFMExtension.Web.WebServices
{
    /// <summary>
    /// Summary description for ExecuteConditions
    /// </summary>
    [WebService(Namespace = "Sidewalk.WFFMExtension.Web.WebServices")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    public class ExecuteConditions : WebService
    {

        public Database Db
        {
            get { return Sitecore.Context.Database ?? Factory.GetDatabase("web"); }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<ActionResponse> GetActions(string id, string selectedValue)
        {
         
            var splittedId = id.Split('_');
            var arrayId = 0;
            for (int i = 0; i < splittedId.Count(); i++)
            {
                if (splittedId[i].Equals("form"))
                {
                    arrayId = i + 1;
                }
            }

            var form = Db.GetItem(new ID(splittedId[arrayId]));
            return GetActionResponses(form, selectedValue);
        }

        private List<ActionResponse> GetActionResponses(Item rootItem, string selectedValue)
        {
            var actions = new List<ActionResponse>();
            
            if (rootItem != null)
            {
                var fieldListWithSection = rootItem.Axes.GetDescendants();
                var allDescendants = new List<Item>();
                //Doing that in case of sections
                foreach (Item item in fieldListWithSection)
                {
                    allDescendants.AddRange(item.Children);
                }
                allDescendants.AddRange(fieldListWithSection);

                foreach (Item fieldItem in allDescendants)
                {
                    
                    var ruleContext = new RuleContext();
                    var rules = RuleFactory.GetRules<RuleContext>(new[] {fieldItem}, "Rules").Rules;

                    foreach (Rule<RuleContext> rule in rules)
                    {
                        if (rule.Condition != null && rule.Condition.GetType().Name.Contains("ConditionalHide"))
                        {
                            var stack = new RuleStack();
                            ruleContext.Parameters.Add("selectedValue", selectedValue);
                            rule.Condition.Evaluate(ruleContext, stack);

                            if (ruleContext.IsAborted)
                            {
                                continue;
                            }
                            
                           
                            var fieldClass = fieldItem.Name;
                            var controlType = fieldItem.TemplateID == Constants.WffmSectionTemplateId ? ControlType.Section : ControlType.Field;
                   
                            if (controlType == ControlType.Section)
                            {
                                var firstCildItem = fieldItem.Children.FirstOrDefault();
                                if (firstCildItem != null)
                                {
                                    fieldClass = firstCildItem.Name;
                                }
                            }

                           
                            if (rule.Condition.GetType() == typeof (ConditionalHide<RuleContext>))
                            {
                                var condHide = (ConditionalHide<RuleContext>) rule.Condition;
                                SessionUtil.SetSessionValue(condHide.SessionVariable, selectedValue);
                            }

                            actions.Add(new ActionResponse {ControlType = controlType, CssClassSelector = fieldClass.Replace(" ","+"), HideControl = (stack.Count != 0) && ((bool) stack.Pop())});
                        }
                    }
                }
            }
            return actions;
        }
    }
}
