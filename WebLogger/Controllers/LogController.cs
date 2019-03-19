using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebLogger.Controllers
{
    /// <summary>
    /// The payload from the log service
    /// </summary>
    public class LogPayload
    {
        public string Message { get; set; }
        public string Level { get; set; }
    }

    [Route("api/log")]
    [ApiController]
    public class LogController : Controller
    {
        public LogController()
        {

        }

        [HttpPost]
        public void Post([FromForm] LogPayload payload)
        {
            // Analyse the payload to confirm it really is not a metric type
            if (!payload.Message.StartsWith("metric:"))
            {
                Debug.WriteLine($"{payload.Level} - {payload.Message}");
            }
        }
    }
}
