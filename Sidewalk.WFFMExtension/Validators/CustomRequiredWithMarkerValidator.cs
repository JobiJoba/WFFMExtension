﻿using Sidewalk.WFFMExtension.Helpers;
using Sitecore.Form.Validators;

namespace Sidewalk.WFFMExtension.Validators
{
    public class CustomRequiredWithMarkerValidator : RequiredWithMarkerValidator
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
