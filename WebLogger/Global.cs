using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLogger.Repositories;

namespace WebLogger
{
    public static class Global
    {
        public static MetricRepository Metrics { get; set; }

        static Global()
        {
            Metrics = new MetricRepository();
        }
    }
}
