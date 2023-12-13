using ConnectionRabbit.Infrastructure.RabbitMq.Interfaces;
using ConnectionRabbit.Infrastructure.RabbitMq.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConnectionRabbit.Infrastructure.RabbitMq.Services
{
    public class RabbitMQContext : IRabbitMQContext
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly ConnectionContext _connectionContext;
        private IConnection _connection;
        private IModel _channel;


        public RabbitMQContext(ConnectionContext connectionContext)
        {
            _connectionFactory = new ConnectionFactory()
            {
                HostName = connectionContext.Host,
                VirtualHost = connectionContext.VirtualHost,
                UserName = connectionContext.Username,
                Password = connectionContext.Password,
                Port = connectionContext.Port,
                AutomaticRecoveryEnabled = true,
                DispatchConsumersAsync = true,
                ConsumerDispatchConcurrency = connectionContext.ConsumerDispatchConcurrency
            };
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.BasicQos(0, connectionContext.LimitMessages, false);
            _connectionContext = connectionContext;
        }

        public void Publish(string publishBody, QueueStructure queue, Dictionary<string, object>? headers = null)
        {
            string arrayJson = publishBody;
            byte[] body = Encoding.UTF8.GetBytes(arrayJson);

            IBasicProperties props = _channel.CreateBasicProperties();
            props.ContentType = "text/plain";
            if (headers != null)
            {
                props.Headers = headers;
            }
            else
            {
                props.Headers = new Dictionary<string, object>();
            }
            _channel.BasicPublish(exchange: queue.Exchange,
                                routingKey: queue.RoutingKey,
                                basicProperties: props,
                                body: body
                                );
        }


        public void Subscribe<TEvent>(IEventHandler<TEvent> handler, string queue) where TEvent : IEvent
        {
            AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                byte[] body = ea.Body.ToArray();

                string message = Encoding.UTF8.GetString(body);
                try
                {
                    TEvent? resultado = JsonSerializer.Deserialize<TEvent>(message, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (resultado == null) 
                    {
                        _channel.BasicAck(ea.DeliveryTag, false);
                        return;
                    }
                    PropertyInfo? RetriesProp = resultado.GetType().GetProperty("Retries");
                    if (RetriesProp != null)
                        RetriesProp.SetValue(resultado, checkCount(ea.BasicProperties.Headers));
                    RabbitMqResponse result = await handler.HandlerAsync(resultado);
                    if (result.Processed)
                    {
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        if (checkCount(ea.BasicProperties.Headers) == 3)
                        {
                            _channel.BasicAck(ea.DeliveryTag, false);
                        }
                        else
                        {
                            _channel.BasicNack(ea.DeliveryTag, false, false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
                    //logger.Error($"Error processing RabbitMq event, message{ex.Message}");
                    if (checkCount(ea.BasicProperties.Headers) == 3)
                    {
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        _channel.BasicNack(ea.DeliveryTag, false, false);
                    }
                }
            };
            _channel.BasicConsume(queue, false, consumer: consumer);
        }

        private int checkCount(IDictionary<string, object> currentObj)
        {
            if (currentObj != null &&
                currentObj["x-death"] != null)
            {
                try
                {
                    return int.Parse(JsonSerializer.Deserialize<List<Dictionary<string, object>>>(JsonSerializer.Serialize(currentObj["x-death"]))[0]["count"].ToString());
                }
                catch
                {
                    return 0;
                }
            }
            return 0;
        }
    }
}
