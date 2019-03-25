using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TNDStudios.Web.ApiManager.Controllers;
using TNDStudios.Web.ApiManager.Security.Authentication;
using objects = TNDStudios.Domain.Objects;

namespace TNDStudios.Web.Api.Controllers.Person.V1
{
    [Authorize]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("1.1")]
    [Route("api/timesheet")]
    [ApiController]
    public class TimesheetController : ManagedController
    {
        public TimesheetController(ILogger<TimesheetController> logger) : base(logger)
        {

        }

        // Version 1.0
        [HttpGet, MapToApiVersion("1.0")]
        [Validate(Type: "admin", Category: "cat", Permission: "read")]
        public ActionResult<IEnumerable<objects.Timesheet>> Get_V1_0() =>
            GetTimesheets();

        // Version 1.1
        [HttpGet, MapToApiVersion("1.1")]
        [Validate(Type: "admin", Category: "cat", Permission: "read")]
        public ActionResult<IEnumerable<objects.Timesheet>> Get_V1_1() =>
            GetTimesheets().ToList().Select(timesheet => { timesheet.InternalId = "V1.1"; return timesheet; }).ToArray();

        // Universal method for all versions to get "people"
        private objects.Timesheet[] GetTimesheets() =>
            new objects.Timesheet[]
                {
                    new objects.Timesheet()
                    {
                        InternalId = "InternalId",
                        ExternalId = "ExternalId",
                        PKId  = Guid.NewGuid()
                    }
                };
    }
}
