using GraphQLDemo.API.Services;

namespace GraphQLDemo.API.Schema;

public class Mutation
{
    private readonly IChatMessageService _chatMessageService;
    private readonly string _expectedPassPhrase;
    public Mutation(IChatMessageService chatMessageService)
    {
        _chatMessageService = chatMessageService;
        _expectedPassPhrase = Environment.GetEnvironmentVariable("PASS_PHRASE") ?? string.Empty;
    }

    public async Task<string> SendMessageAsync(
        [GraphQLNonNullType] string sender,
        [GraphQLNonNullType] string content,
        string passPhrase)
    {
        if (!IsAuthenticated(passPhrase))
        {
            throw new GraphQLException(new Error("Invalid passphrase", "INVALID_PASSPHRASE"));
        }
        return await _chatMessageService.AddMessageAsync(sender, content);
    }

    public bool IsAuthenticated(string passPhrase)
    {
        if (string.IsNullOrEmpty(_expectedPassPhrase))
        {
            return true;
        }

        if (!string.IsNullOrEmpty(passPhrase) && passPhrase.Equals(_expectedPassPhrase, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;
    }

    public bool ClearMessages()
    {
        _chatMessageService.ClearMessages();
        return true;
    }


}

