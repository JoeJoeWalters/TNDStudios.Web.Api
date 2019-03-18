using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebLogger.Controllers
{
    /// <summary>
    /// We need a complex type for modelbinding because 
    /// of content-type: "application/x-www-form-urlencoded" 
    /// in <see cref="WebServiceTarget"/>
    /// </summary>
    public class ComplexType
    {
        public string Message { get; set; }
        public string Level { get; set; }
    }

    [Route("api/logger")]
    [ApiController]
    public class LogController : Controller
    {
        public LogController()
        {

        }

        /// <summary>
        /// Post
        /// </summary>
        [HttpPost]
        public void Post([FromForm] ComplexType complexType)
        {
            // Analyse Metrics
            if (complexType.Message.StartsWith("metric:"))
                Debug.WriteLine($"{complexType.Level} - {complexType.Message}");
        }
    }
}
