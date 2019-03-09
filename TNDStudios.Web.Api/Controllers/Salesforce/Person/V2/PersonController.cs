using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TNDStudios.Web.ApiManager.Controllers;
using TNDStudios.Web.ApiManager.Data.Salesforce;
using objects = TNDStudios.Domain.Objects;

namespace TNDStudios.Web.Api.Controllers.Salesforce.Person.V2
{
    [JsonObject]
    public class SalesforcePerson : SalesforceObjectBase
    {
        [JsonProperty("sf:Name")]
        public String Name { get; set; }

        [JsonProperty("sf:Email")]
        public String Email { get; set; }
    }
    
    [Route("api/salesforce/person/v2")]
    [ApiController]
    public class PersonController : SalesforceNotificationController<SalesforcePerson>
    {
        public override List<string> AllowedOrganisationIds { get; } =
            new List<string>()
            {
                "00D80000000cDmQEAU"
            };

        public PersonController(ILogger<SalesforceNotificationController<SalesforcePerson>> logger)
            : base(logger)
        {
        }

        public override ActionResult<Boolean> Processor(
            List<SalesforceNotification<SalesforcePerson>> notifications)
        {
            return base.Processor(notifications);
        }
    }
}
