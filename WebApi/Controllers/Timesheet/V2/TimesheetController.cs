using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using TNDStudios.Web.ApiManager.Controllers;
using TNDStudios.Web.ApiManager.Security.Authentication;
using WebApi.Extensions;
using objects = TNDStudios.Domain.Objects;

namespace TNDStudios.Web.Api.Controllers.Person.V2
{
    [Authorize]
    [ApiVersion("2.0")]
    [Route("api/timesheet")]
    [ApiController]
    public class TimesheetController : ManagedController
    {
        public TimesheetController(ILogger<TimesheetController> logger) : base(logger)
        {

        }

        // Version 2.0
        [HttpGet, MapToApiVersion("2.0")]
        [Validate(Type: "admin", Category: "cat", Permission: "read")]
        public ActionResult<IEnumerable<objects.Timesheet>> Get_V2_0() =>
            new objects.Timesheet[]
            {
                new objects.Timesheet()
                {
                    InternalId = "v2.0",
                    ExternalId = "ExternalId",
                    PKId  = Guid.NewGuid()
                }
            };

        [HttpPost, MapToApiVersion("2.0")]
        [Validate(Type: "admin", Category: "cat", Permission: "write")]
        public ActionResult<Boolean> Post_V2_0()
        {
            Logger.LogMetric("Timesheet", MetricType.Received, (Double)1);

            return true;
        }

    }
}
