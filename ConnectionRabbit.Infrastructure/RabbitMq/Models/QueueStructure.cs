using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionRabbit.Infrastructure.RabbitMq.Models
{
    public class QueueStructure
    {
        public bool IsActive { get; set; }
        public string? Name { get; set; }
        public string? RoutingKey { get; set; }
        public string? Exchange { get; set; }
        public bool Durability { get; set; }
        public bool Exclusive { get; set; }
        public bool AutoDelete { get; set; }
        public Dictionary<string, string>? Arguments { get; set; }
    }
}
