using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionRabbit.Infrastructure.RabbitMq.Models
{
    public class ConnectionContext
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? VirtualHost { get; set; }
        public string? Host { get; set; }
        public int Port { get; set; }
        public ushort LimitMessages { get; set; }
        public int ConsumerDispatchConcurrency { get; set; }
    }
}
