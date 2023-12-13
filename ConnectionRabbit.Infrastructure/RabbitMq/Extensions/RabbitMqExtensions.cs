using ConnectionRabbit.Infrastructure.RabbitMq.Interfaces;
using ConnectionRabbit.Infrastructure.RabbitMq.Models;
using ConnectionRabbit.Infrastructure.RabbitMq.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionRabbit.Infrastructure.RabbitMq.Extensions
{
    public static class RabbitMqExtensions
    {
        public static void AddRabbitMq(this IServiceCollection services, ConnectionContext context) 
        {
            //Validate connection properties
            if (context == null || 
                string.IsNullOrEmpty(context.Host) || 
                string.IsNullOrEmpty(context.Username) || 
                string.IsNullOrEmpty(context.Password) || 
                context.Port < 1)
            {
                throw new Exception();
            }
            RabbitMQContext client = new RabbitMQContext(context);
            services.AddSingleton<ConnectionContext>(_ => context);
            services.AddSingleton<RabbitMQContext>(_ => client);
        }
    }
}
