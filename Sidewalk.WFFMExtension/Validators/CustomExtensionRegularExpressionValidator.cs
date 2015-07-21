using Sidewalk.WFFMExtension.Helpers;
using Sitecore.Form.Core.Validators;

namespace Sidewalk.WFFMExtension.Validators
{
    public class CustomExtensionRegularExpressionValidator : CustomRegularExpressionValidator
    {
        protected override bool EvaluateIsValid()
        {
            if (WFFMExtensionHelper.IsDisplayNone(Parent))
            {
                return true;
            }

            return base.EvaluateIsValid();

        }
    }
}
