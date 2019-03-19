using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebLogger.Controllers
{
    /// <summary>
    /// The payload from the metric service
    /// </summary>
    public class MetricPayload
    {
        public string Message { get; set; }
        public string Level { get; set; }
    }

    [Route("api/metric")]
    [ApiController]
    public class MetricController : Controller
    {
        public MetricController()
        {

        }

        [HttpPost]
        public void Post([FromForm] MetricPayload payload)
        {
            // Analyse the payload to confirm it really is a metric type
            if (payload.Message.StartsWith("metric:"))
            {
                Debug.WriteLine($"{payload.Level} - {payload.Message}");
            }
        }
    }
}
