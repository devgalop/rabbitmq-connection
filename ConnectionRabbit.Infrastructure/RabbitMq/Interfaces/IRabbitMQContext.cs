using ConnectionRabbit.Infrastructure.RabbitMq.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionRabbit.Infrastructure.RabbitMq.Interfaces
{
    public interface IRabbitMQContext
    {
        void Publish(string publishBody, QueueStructure queue, Dictionary<string, object>? headers = null);
        void Subscribe<TEvent>(IEventHandler<TEvent> handler, string queue) where TEvent : IEvent;
    }
}
