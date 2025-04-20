using GraphQLDemo.API.Schema;
using GraphQLDemo.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

// Register the chat message service
builder.Services.AddSingleton<IChatMessageService, ChatMessageService>();


var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});

app.Run();
