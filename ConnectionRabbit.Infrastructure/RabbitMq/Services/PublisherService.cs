using ConnectionRabbit.Infrastructure.RabbitMq.Interfaces;
using ConnectionRabbit.Infrastructure.RabbitMq.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionRabbit.Infrastructure.RabbitMq.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IRabbitMQContext _rabbitMQContext;

        public PublisherService(IRabbitMQContext rabbitMQContext)
        {
            _rabbitMQContext = rabbitMQContext;
        }
        public void publishEvent(string message, QueueStructure queue)
        {
            _rabbitMQContext.Publish(message, queue);
        }

        public void publishEventReference(string message, QueueStructure queue, Dictionary<string, object>? headers = null)
        {
            if (queue == null)
            {
                throw new Exception();
            }
            _rabbitMQContext.Publish(message, queue, headers);
        }
    }
}
