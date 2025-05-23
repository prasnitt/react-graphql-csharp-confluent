using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using GraphQLDemo.API.Schema;
using GraphQLDemo.API.Services;
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


builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

// Register the chat message service
builder.Services.AddSingleton<IChatMessageService, ChatMessageService>();

builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection("Producer"));
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

builder.Services.AddSingleton<IProducer<String, ChatMessage>>(sp =>
{
    var config = sp.GetRequiredService<IOptions<ProducerConfig>>();
    var logger = sp.GetRequiredService<ILogger<Program>>();
    if (string.IsNullOrEmpty(config.Value.SaslPassword))
    {
        logger.LogWarning("SaslPassword is not set in the configuration. Falling back to the environment variable 'CONFLUENT_SASL_PASSWORD'.");
        config.Value.SaslPassword = Environment.GetEnvironmentVariable("CONFLUENT_SASL_PASSWORD");
    }

    var schemaRegistryClient = sp.GetRequiredService<ISchemaRegistryClient>();

    // Debugging
    //config.Value.Debug = "all";

    return new ProducerBuilder<String, ChatMessage>(config.Value)
        .SetValueSerializer(new JsonSerializer<ChatMessage>(schemaRegistryClient))
        .Build();
});


var app = builder.Build();

app.UseRouting();
app.UseCors("ConfiguredCors");

app.UseHealthChecks("/healthy");
app.MapGraphQL("/graphql");

// Redirect root to GraphQL endpoint
app.MapGet("/", context =>
{
    context.Response.Redirect("/graphql");
    return Task.CompletedTask;
});

// Force Kafka producer to initialize early (without that the first request will take a long time)
WarmUpKafkaProducer(app);

app.Run();



void WarmUpKafkaProducer(WebApplication app)
{
    try
    {
        var producer = app.Services.GetRequiredService<IProducer<string, ChatMessage>>();
        producer.Flush();

        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Kafka producer initialized early using Flush().");
    }
    catch (Exception ex)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Kafka producer early initialization failed.");
    }
}