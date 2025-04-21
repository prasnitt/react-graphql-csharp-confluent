using GraphQLDemo.API.Services;
using Shared;

namespace GraphQLDemo.API.Schema;

public class Query
{
    private readonly IChatMessageService _chatMessageService;

    public Query(IChatMessageService chatMessageService)
    {
        _chatMessageService = chatMessageService;
    }

    public IEnumerable<ChatMessage> GetMessages()
    {
        var messages = _chatMessageService.GetMessages().ToList();
        messages.Sort((x, y) => x.Timestamp.CompareTo(y.Timestamp));

        return messages;
    }

    public ChatMessage GetMessage(string id)
    {
        return _chatMessageService.GetMessage(id);
    }

    // TODO: Add query methods here
    [GraphQLDeprecated("This is deprecated")]
    public string Hello() => "Hello, GraphQL!";


}

