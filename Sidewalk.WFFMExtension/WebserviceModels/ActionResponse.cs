using Newtonsoft.Json;
using Sidewalk.WFFMExtension.Enums;

namespace Sidewalk.WFFMExtension.WebserviceModels
{
    public class ActionResponse
    {
        [JsonProperty("cssClass")]
        public string CssClassSelector { get; set; }
        
        [JsonProperty("controlType")]
        public ControlType ControlType { get; set; }
        
        [JsonProperty("hideControl")]
        public bool HideControl { get; set; }
    }
}
