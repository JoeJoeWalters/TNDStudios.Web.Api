using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using TNDStudios.Data.DocumentCache;
using TNDStudios.Data.DocumentCache.Cosmos;
using TNDStudios.Web.ApiManager.Controllers;
using TNDStudios.Web.ApiManager.Data.Salesforce;

namespace TNDStudios.Web.Api.Controllers.Salesforce.Person.V1
{
    [Route("api/salesforce/person/v1")]
    [ApiController]
    public class PersonController : SalesforceNotificationController<SalesforcePerson>
    {
        /// <summary>
        /// Document caching handler
        /// </summary>
        private IDocumentHandler<SalesforceNotification<SalesforcePerson>> documentHandler;

        /// <summary>
        /// The organisations allowed to access this controller
        /// </summary>
        public override List<string> AllowedOrganisationIds { get; } = Startup.AllowedOrgIds;

        /// <summary>
        /// Set up logging and the document cache handler
        /// </summary>
        /// <param name="logger"></param>
        public PersonController(ILogger<PersonController> logger)
            : base(logger)
        {
            // Already got a document handler?
            if (documentHandler == null)
                documentHandler = new CosmosDocumentHandler<SalesforceNotification<SalesforcePerson>>(
                    logger,
                    Startup.CosmosDB,
                    "Salesforce_ReceiverCache",
                    "SalesforcePerson");
        }

        /// <summary>
        /// Override the notification processor to connect it to the notification caching
        /// </summary>
        /// <param name="notifications">The list of notifications from the well formed request</param>
        /// <returns>Ack to Salesforce</returns>
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
                Boolean itemResult = documentHandler.Save(notification.Id, notification);

                // One failed, so all fail
                result = (!itemResult) ? itemResult : result;
            }

            // Send the result wrapped in a Http Result
            return new ObjectResult(result);
        }
    }
}
