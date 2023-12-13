using ConnectionRabbit.Handler;
using ConnectionRabbit.Infrastructure.RabbitMq.Extensions;
using ConnectionRabbit.Infrastructure.RabbitMq.Interfaces;
using ConnectionRabbit.Infrastructure.RabbitMq.Models;
using ConnectionRabbit.Infrastructure.RabbitMq.Services;
using ConnectionRabbit.Models;
using System;


try
{

    var builder = WebApplication.CreateBuilder(args);
    ConnectionContext options = new ConnectionContext
    {
        Host = builder.Configuration["RabbitMq:Host"],
        VirtualHost = builder.Configuration["RabbitMq:VirtualHost"],
        Username = builder.Configuration["RabbitMq:Username"],
        Password = builder.Configuration["RabbitMq:Password"],
        Port = int.Parse(builder.Configuration["RabbitMq:Port"]!.ToString()),
        ConsumerDispatchConcurrency = int.Parse(builder.Configuration["RabbitMq:DispatchConcurrency"]!.ToString()),
        LimitMessages = ushort.Parse(builder.Configuration["RabbitMq:LimitMessages"]!.ToString())
    };

    // Add services to the container.
    builder.Services.AddTransient<IEventHandler<GenericModel>, GenericHandler>();
    builder.Services.AddRabbitMq(options);
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();
    RabbitMQContext client = new RabbitMQContext(options);
    client.Subscribe<GenericModel>((IEventHandler<GenericModel>)app.Services.GetService(typeof(IEventHandler<GenericModel>)), "com.parser.begin");

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception)
{

	throw;
}

