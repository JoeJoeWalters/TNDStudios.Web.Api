using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using TNDStudios.Web.ApiManager.Controllers;
using objects = TNDStudios.Domain.Objects;

namespace TNDStudios.Web.Api.Controllers.Salesforce.Person.V2
{
    [ApiVersion("2.0")]
    [Route("api/salesforce/person")]
    [ApiController]
    public class PersonController : ManagedController
    {
        public PersonController(ILogger<ManagedController> logger) : base(logger)
        {

        }

        // Version 2.0
        [HttpGet, MapToApiVersion("2.0")]
        public ActionResult<Boolean> Get_V2_0()
        {
            return true;
        }
    }
}
