using Sidewalk.WFFMExtension.Enums;
using Sidewalk.WFFMExtension.Resources;
using Sidewalk.WFFMExtension.Rules;
using Sidewalk.WFFMExtension.WebserviceModels;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Form.Core.Utility;
using Sitecore.Rules;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;

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

        public Database GetDatabase(bool pageMode)
        {
            if (pageMode)
            {
                return Factory.GetDatabase("web");
            }

            return Factory.GetDatabase("master");
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<ActionResponse> GetActions(string id, string selectedValue, bool pageMode, string sessionName)
        {

            string[] splittedId = id.Split('_');
            int arrayId = 0;
            for (int i = 0; i < splittedId.Count(); i++)
            {
                if (splittedId[i].Equals("form"))
                {
                    arrayId = i + 1;
                }
            }



            Item form = GetDatabase(pageMode).GetItem(new ID(splittedId[arrayId]));
            return GetActionResponses(form, selectedValue, sessionName);

        }

        private List<ActionResponse> GetActionResponses(Item rootItem, string selectedValue, string sessionName)
        {
            List<ActionResponse> actions = new List<ActionResponse>();

            if (rootItem != null)
            {
                Item[] fieldListWithSection = rootItem.Axes.GetDescendants();
                List<Item> allDescendants = new List<Item>();
                //Doing that in case of sections
                foreach (Item item in fieldListWithSection)
                {
                    allDescendants.AddRange(item.Children);
                }
                allDescendants.AddRange(fieldListWithSection);

                foreach (Item fieldItem in allDescendants)
                {

                    RuleContext ruleContext = new RuleContext();
                    IEnumerable<Rule<RuleContext>> rules = RuleFactory.GetRules<RuleContext>(new[] { fieldItem }, "Rules").Rules;

                    foreach (Rule<RuleContext> rule in rules)
                    {
                        if (rule.Condition != null && rule.Condition.GetType().Name.Contains("ConditionalHide"))
                        {
                            var ruleConditionalHide =
                                ((Sidewalk.WFFMExtension.Rules.ConditionalHide<Sitecore.Rules.RuleContext>)
                                    rule.Condition);
                            if (ruleConditionalHide.SessionVariable == sessionName)
                            {
                                RuleStack stack = new RuleStack();
                                ruleContext.Parameters.Add("selectedValue", selectedValue);
                                rule.Condition.Evaluate(ruleContext, stack);

                                if (ruleContext.IsAborted)
                                {
                                    continue;
                                }


                                string fieldClass = fieldItem.Name;
                                ControlType controlType = fieldItem.TemplateID == Constants.WffmSectionTemplateId ? ControlType.Section : ControlType.Field;

                                if (controlType == ControlType.Section)
                                {
                                    Item firstCildItem = fieldItem.Children.FirstOrDefault();
                                    if (firstCildItem != null)
                                    {
                                        fieldClass = firstCildItem.Name;
                                    }
                                }


                                if (rule.Condition.GetType() == typeof(ConditionalHide<RuleContext>))
                                {
                                    ConditionalHide<RuleContext> condHide = (ConditionalHide<RuleContext>)rule.Condition;
                                    SessionUtil.SetSessionValue(condHide.SessionVariable, selectedValue);
                                }

                                actions.Add(new ActionResponse { ControlType = controlType, CssClassSelector = fieldClass.Replace(" ", "+"), HideControl = (stack.Count != 0) && ((bool)stack.Pop()) });
                            }
                        }
                    }
                }
            }
            return actions;
        }
    }
}
