using Newtonsoft.Json;
using System;
using TNDStudios.Web.ApiManager.Data.Salesforce;

namespace TNDStudios.Web.Api.Controllers.Salesforce.Timesheet.V1
{
    [JsonObject]
    public class SalesforceTimesheet : SalesforceObjectBase
    {
        [JsonProperty("sf:ItemNumber")]
        public String ItemNumber { get; set; }

        [JsonProperty("sf:TotalItems")]
        public String TotalItems { get; set; }

        [JsonProperty("sf:RateType")]
        public String Name { get; set; }

        [JsonProperty("sf:Volume")]
        public String Email { get; set; }
    }
}
