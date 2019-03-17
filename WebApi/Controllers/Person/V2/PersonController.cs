using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using TNDStudios.Web.ApiManager.Controllers;
using TNDStudios.Web.ApiManager.Security.Authentication;
using objects = TNDStudios.Domain.Objects;

namespace TNDStudios.Web.Api.Controllers.Person.V2
{
    [Authorize]
    [ApiVersion("2.0")]
    [Route("api/person")]
    [ApiController]
    public class PersonController : ManagedController
    {
        public PersonController(ILogger<PersonController> logger) : base(logger)
        {

        }

        // Version 2.0
        [HttpGet, MapToApiVersion("2.0")]
        [Validate(Type: "admin", Category: "cat", Permission: "read")]
        public ActionResult<IEnumerable<objects.Person>> Get_V2_0() =>
            new objects.Person[]
            {
                new objects.Person()
                {
                    Forename = "V2.0",
                    Surname = "Name",
                    InternalId = "InternalId",
                    ExternalId = "ExternalId",
                    Middlenames = "Middlenames",
                    NINumber = "NINumber",
                    PKId  = Guid.NewGuid(),
                    DOB = new DateTime(2001, 5, 21),
                    Title = "Mr"
                }
            };
    }
}
