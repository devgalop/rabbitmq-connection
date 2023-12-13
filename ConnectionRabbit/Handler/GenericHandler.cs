using ConnectionRabbit.Infrastructure.RabbitMq.Interfaces;
using ConnectionRabbit.Infrastructure.RabbitMq.Models;
using ConnectionRabbit.Models;
using Newtonsoft.Json;

namespace ConnectionRabbit.Handler
{
    public class GenericHandler : IEventHandler<GenericModel>
    {
        public async Task<RabbitMqResponse> HandlerAsync(GenericModel eventMessage)
        {
            Console.WriteLine(JsonConvert.SerializeObject(eventMessage));
            return new() 
            {
                Processed = true,
                Retry = false
            };
        }
    }
}
