// See https://aka.ms/new-console-template for more information

using Microsoft.AspNetCore.SignalR.Client;
using Shared;

Console.WriteLine("Hello, World");
var name = Environment.GetEnvironmentVariable("MY_NAME"); ;
var connection = new HubConnectionBuilder()
    .WithUrl($"https://localhost:7124/chat-hub?name={name}")
    .WithAutomaticReconnect()
    .Build();

connection.On<ChatMessage>("ReceiveMessage", message =>
{
    Console.WriteLine($"[{message.Timestamp}] {message.Sender}: '{message.Content}'");
});

await connection.StartAsync();

var chatMessage = new ChatMessage(name, "Hello from console!");

await connection.InvokeAsync("SendMessage", chatMessage);

Console.WriteLine("⏳ Waiting for messages. Press Ctrl+C to exit.");
await Task.Delay(Timeout.Infinite);