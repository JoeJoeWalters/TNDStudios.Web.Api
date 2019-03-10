using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using TNDStudios.Data.Cosmos.DocumentCache;
using TNDStudios.Web.ApiManager.Controllers;
using TNDStudios.Web.ApiManager.Data.Salesforce;

namespace TNDStudios.Web.Api.Controllers.Salesforce.Person.V1
{
    [Route("api/salesforce/person/v1")]
    [ApiController]
    public class PersonController : SalesforceNotificationController<SalesforcePerson>
    {
        private DocumentHandler<SalesforceNotification<SalesforcePerson>> documentHandler;

        public override List<string> AllowedOrganisationIds { get; } =
            new List<string>()
            {
                "00D80000000cDmQEAU"
            };

        public PersonController(ILogger<SalesforceNotificationController<SalesforcePerson>> logger)
            : base(logger)
        {
            // Already got a document handler?
            if (documentHandler == null)
                documentHandler = new DocumentHandler<SalesforceNotification<SalesforcePerson>>(
                    "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                    "Salesforce_RecieverCache",
                    "SalesforcePerson");
        }

        public override ActionResult<Boolean> Processor(
            List<SalesforceNotification<SalesforcePerson>> notifications)
        {
            Boolean result = true;

            // We need to make sure all notifications are cached correctly to be a success
            // otherwise we must reject the whole message
            foreach (var notification in notifications)
            {
                // Pump the notificatin to the document cache making sure we define the id
                // that we want to use as the key
                Boolean itemResult = documentHandler.SendToCache(notification.Id, notification);

                // One failed, so all fail
                result = (!itemResult) ? itemResult : result; 
            }

            // Send the result wrapped in a Http Result
            return new ObjectResult(result);
        }
    }
}
