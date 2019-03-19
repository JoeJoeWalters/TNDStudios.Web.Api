using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLogger.Repositories
{
    public class MetricContainer
    {
        public DateTime Received { get; set; }
        public String ObjectType { get; set; }
        public Double Metric { get; set; }
    }

    /// <summary>
    /// Place to store metrics received from other applications
    /// </summary>
    public class MetricRepository
    {
        public List<MetricContainer> Data { get; set; } = new List<MetricContainer>() { };

        public MetricRepository()
        {

        }
    }
}
