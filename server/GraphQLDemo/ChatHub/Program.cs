using ChatHub;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

builder.Services.AddHostedService<ChatHubWorker>();

var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
//app.UseWebSockets();
app.MapHub<ChatHub.ChatHub>("/chat-hub");

app.Run();
