using GraphQLDemo.API.Dto;
using GraphQLDemo.API.Services;

namespace GraphQLDemo.API.Schema;

public class Mutation
{
    private readonly IChatMessageService _chatMessageService;

    public Mutation(IChatMessageService chatMessageService)
    {
        _chatMessageService = chatMessageService;
    }

    public string SendMessage(
        [GraphQLNonNullType] string sender,
        [GraphQLNonNullType] string content)
    {
        var message = new ChatMessage(sender, content);
        _chatMessageService.AddMessage(message);

        return message.Id;
    }

    public bool ClearMessages()
    {
        _chatMessageService.ClearMessages();
        return true;
    }
}

