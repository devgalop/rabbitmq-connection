using ConnectionRabbit.Infrastructure.RabbitMq.Interfaces;

namespace ConnectionRabbit.Models
{
    public class GenericModel : IEvent
    {
        public string? Id { get; set; }
    }
}
