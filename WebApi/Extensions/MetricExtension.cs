using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Extensions
{
    public static class MetricExtension
    {
        public static void LogMetric(this ILogger logger, String ObjectName, Object metrics)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.LogInformation($"metric:{ObjectName}:{{value}}", metrics);
        }
    }
}
