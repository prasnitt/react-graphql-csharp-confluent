using ChatHub;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.Options;
using Shared;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();

var allowedOrigins = builder.Configuration
 .GetSection("Cors:AllowedOrigins")
 .Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ConfiguredCors", policy =>
    {
        policy
            .WithOrigins(allowedOrigins!)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddSignalR();
builder.Services.AddHostedService<ChatHubWorker>();

builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection("Consumer"));
builder.Services.Configure<SchemaRegistryConfig>(builder.Configuration.GetSection("SchemaRegistry"));
builder.Services.AddSingleton<ISchemaRegistryClient>(sp =>
{
    var config = sp.GetRequiredService<IOptions<SchemaRegistryConfig>>();
    return new CachedSchemaRegistryClient(config.Value);
});

builder.Services.AddSingleton<IConsumer<String, ChatMessage>>(sp =>
{
    var config = sp.GetRequiredService<IOptions<ConsumerConfig>>();
    return new ConsumerBuilder<String, ChatMessage>(config.Value)
        .SetValueDeserializer(new JsonDeserializer<ChatMessage>().AsSyncOverAsync())
        .Build();
});


var app = builder.Build();
app.UseRouting();
app.UseCors("ConfiguredCors");
app.MapGet("/", () => "Hello World!");
//app.UseWebSockets();
app.UseHealthChecks("/healthy");
app.MapHub<ChatHub.ChatHub>("/chat-hub");

app.Run();
