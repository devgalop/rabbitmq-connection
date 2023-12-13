using ConnectionRabbit.Infrastructure.RabbitMq.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionRabbit.Infrastructure.RabbitMq.Interfaces
{
    public interface IPublisherService
    {
        void publishEventReference(string message, QueueStructure queue, Dictionary<string, object>? headers = null);
        void publishEvent(string message, QueueStructure queue);
    }
}
