using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using TNDStudios.Web.ApiManager.Controllers;
using objects = TNDStudios.Domain.Objects;

namespace TNDStudios.Web.Api.Controllers.Salesforce.Person.V1
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("1.1")]
    [Route("api/salesforce/person")]
    [ApiController]
    public class PersonController : ManagedController
    {
        public PersonController(ILogger<ManagedController> logger) : base(logger)
        {

        }

        // Version 1.0
        [HttpGet, MapToApiVersion("1.0")]
        public ActionResult<Boolean> Get_V1_0()
        {
            return true;
        }

        // Version 1.1
        [HttpGet, MapToApiVersion("1.1")]
        public ActionResult<Boolean> Get_V1_1()
        {
            return true;
        }
    }
}
