using GraphQLDemo.API.Services;

namespace GraphQLDemo.API.Schema;

public class Mutation
{
    private readonly IChatMessageService _chatMessageService;

    public Mutation(IChatMessageService chatMessageService)
    {
        _chatMessageService = chatMessageService;
    }

    public async Task<string> SendMessageAsync(
        [GraphQLNonNullType] string sender,
        [GraphQLNonNullType] string content)
    {
        return await _chatMessageService.AddMessageAsync(sender, content);
    }

    public bool ClearMessages()
    {
        _chatMessageService.ClearMessages();
        return true;
    }
}

