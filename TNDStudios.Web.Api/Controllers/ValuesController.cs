using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNDStudios.Domain.Objects;
using TNDStudios.Web.ApiManager.Security.Authentication;
using TNDStudios.Web.ApiManager.Controllers;
using Microsoft.Extensions.Logging;

namespace TNDStudios.Web.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ManagedController
    {
        public ValuesController(ILogger<ManagedController> logger) : base(logger)
        {

        }

        // GET api/values
        [Validate(Type: "admin", Company: "ba", Permission: "read")]
        [HttpGet]
        public ActionResult<IEnumerable<Person>> Get()
        {
            CurrentUser?.Claims?.ForEach(claim => { });

            return new Person[]
            {
                new Person()
                {
                    Forename = "Joe",
                    Surname = "Walters",
                    InternalId = "InternalId",
                    ExternalId = "ExternalId",
                    Middlenames = "Matthew",
                    NINumber = "NINumber",
                    PKId  = Guid.NewGuid(),
                    DOB = new DateTime(1976, 5, 23),
                    Title = "Mr"
                }
            };
        }
    }
}
