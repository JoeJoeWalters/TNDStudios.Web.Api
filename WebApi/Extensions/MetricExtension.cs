using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Extensions
{
    public enum MetricType
    {
        Unknown = 0,
        Received = 1,
        Sent = 2,
        Failed = 3
    }

    public static class MetricExtension
    {
        
        public static void LogMetric(this ILogger logger, String ObjectName, MetricType metricType, Double volume)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.LogInformation($"metric|{ObjectName}:{((Int64)metricType).ToString()}:{volume.ToString()}");
        }
    }
}
