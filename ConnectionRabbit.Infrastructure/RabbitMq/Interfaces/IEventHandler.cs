using ConnectionRabbit.Infrastructure.RabbitMq.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionRabbit.Infrastructure.RabbitMq.Interfaces
{
    public interface IEventHandler<IEvent>
    {
        Task<RabbitMqResponse> HandlerAsync(IEvent @eventMessage);
    }
}
