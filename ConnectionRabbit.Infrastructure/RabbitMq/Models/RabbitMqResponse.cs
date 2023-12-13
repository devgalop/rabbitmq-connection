using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionRabbit.Infrastructure.RabbitMq.Models
{
    public class RabbitMqResponse
    {
        public bool Processed { get; set; }
        public bool Retry { get; set; }
    }
}
