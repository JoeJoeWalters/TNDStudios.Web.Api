using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using TNDStudios.Data.DocumentCache;
using TNDStudios.Data.DocumentCache.Cosmos;
using TNDStudios.Web.ApiManager.Controllers;
using TNDStudios.Web.ApiManager.Data.Salesforce;
using WebApi.Extensions;

namespace TNDStudios.Web.Api.Controllers.Salesforce.Timesheet.V1
{
    [Route("api/salesforce/timesheet/v1")]
    [ApiController]
    public class TimesheetController : SalesforceNotificationController<SalesforceTimesheet>
    {
        /// <summary>
        /// Document caching handler
        /// </summary>
        private IDocumentHandler<SalesforceNotification<SalesforceTimesheet>> documentHandler;

        /// <summary>
        /// The organisations allowed to access this controller
        /// </summary>
        public override List<string> AllowedOrganisationIds { get; } = Startup.AllowedOrgIds;

        /// <summary>
        /// Set up logging and the document cache handler
        /// </summary>
        /// <param name="logger"></param>
        public TimesheetController(ILogger<TimesheetController> logger)
            : base(logger)
        {
            // Already got a document handler?
            if (documentHandler == null)
                documentHandler = new CosmosDocumentHandler<SalesforceNotification<SalesforceTimesheet>>(
                    logger,
                    Startup.CosmosDB,
                    "Salesforce_ReceiverCache",
                    "SalesforceTimesheetLine");
        }

        /// <summary>
        /// Override the notification processor to connect it to the notification caching
        /// </summary>
        /// <param name="notifications">The list of notifications from the well formed request</param>
        /// <returns>Ack to Salesforce</returns>
        public override ActionResult<Boolean> Processor(
            List<SalesforceNotification<SalesforceTimesheet>> notifications)
        {
            Boolean result = true;

            // We need to make sure all notifications are cached correctly to be a success
            // otherwise we must reject the whole message
            foreach (var notification in notifications)
            {
                // Pump the notificatin to the document cache making sure we define the id
                // that we want to use as the key
                Boolean itemResult = documentHandler.Save(notification.Id, notification);

                if (itemResult)
                {
                    Logger.LogMetric("SalesforceTimesheetLine", MetricType.Received, (Double)1);
                }
                else
                    Logger.LogMetric("SalesforceTimesheetLine", MetricType.Failed, (Double)1);

                // One failed, so all fail
                result = (!itemResult) ? itemResult : result;
            }

            // Is everything now available to process? i.e. all parts have been received?
            if (result)
            {
                // TODO:
            }

            // Send the result wrapped in a Http Result
            return new ObjectResult(result);
        }
    }
}
