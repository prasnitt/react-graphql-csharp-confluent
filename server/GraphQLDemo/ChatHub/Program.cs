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

builder.Services.AddSingleton<ClientConnectionManager>();
builder.Services.AddSignalR();
builder.Services.AddHostedService<ChatHubWorker>();

builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection("Consumer"));
builder.Services.Configure<SchemaRegistryConfig>(builder.Configuration.GetSection("SchemaRegistry"));
builder.Services.AddSingleton<ISchemaRegistryClient>(sp =>
{
    var config = sp.GetRequiredService<IOptions<SchemaRegistryConfig>>();
    if (string.IsNullOrEmpty(config.Value.BasicAuthUserInfo))
    {
        config.Value.BasicAuthUserInfo = Environment.GetEnvironmentVariable("CONFLUENT_SCHEMA_REGISTRY_AUTH");
    }
    return new CachedSchemaRegistryClient(config.Value);
});

builder.Services.AddSingleton<IConsumer<String, ChatMessage>>(sp =>
{
    var config = sp.GetRequiredService<IOptions<ConsumerConfig>>();
    var logger = sp.GetRequiredService<ILogger<Program>>();
    if (string.IsNullOrEmpty(config.Value.SaslPassword))
    {
        logger.LogWarning("SaslPassword is not set in the configuration. Falling back to the environment variable 'CONFLUENT_SASL_PASSWORD'.");
        config.Value.SaslPassword = Environment.GetEnvironmentVariable("CONFLUENT_SASL_PASSWORD");
    }

    if (string.IsNullOrEmpty(config.Value.SaslPassword))
    {
        logger.LogWarning("SaslPassword is still not set");
    }

    if (string.IsNullOrEmpty(config.Value.GroupId))
    {
        logger.LogWarning("GroupId has not set");
        config.Value.GroupId = Environment.GetEnvironmentVariable("CONFLUENT_GROUP_ID");
    }

    logger.LogWarning($" {config.Value.GroupId} is final group id");

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
