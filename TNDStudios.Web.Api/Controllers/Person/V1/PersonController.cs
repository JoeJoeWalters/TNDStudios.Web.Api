using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using objects = TNDStudios.Domain.Objects;
using TNDStudios.Web.ApiManager.Security.Authentication;
using TNDStudios.Web.ApiManager.Controllers;
using Microsoft.Extensions.Logging;

namespace TNDStudios.Web.Api.Controllers.Person.V1
{
    [Authorize]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [Route("api/person")]
    [ApiController]
    public class PersonController : ManagedController
    {
        public PersonController(ILogger<ManagedController> logger) : base(logger)
        {

        }

        // Version 1.0
        [HttpGet, MapToApiVersion("1.0")]
        [Validate(Type: "admin", Category: "cat", Permission: "read")]
        [HttpGet]
        public ActionResult<IEnumerable<objects.Person>> Get_V1_0() =>
            new objects.Person[]
            {
                new objects.Person()
                {
                    Forename = "Person",
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

        [HttpGet, MapToApiVersion("1.1")]
        [Validate(Type: "admin", Category: "cat", Permission: "read")]
        [HttpGet]
        public ActionResult<IEnumerable<objects.Person>> Get_V1_1() => 
            Get_V1_0();

    }
}
