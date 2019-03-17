using Newtonsoft.Json;
using System;
using TNDStudios.Web.ApiManager.Data.Salesforce;

namespace TNDStudios.Web.Api.Controllers.Salesforce.Person.V1
{
    [JsonObject]
    public class SalesforcePerson : SalesforceObjectBase
    {
        [JsonProperty("sf:Name")]
        public String Name { get; set; }

        [JsonProperty("sf:Email")]
        public String Email { get; set; }
    }
}
