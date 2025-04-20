namespace GraphQLDemo.API.Schema;

public class Query
{
    // TODO: Add query methods here
    public string Hello() => "Hello, GraphQL!";
    public string Goodbye() => "Goodbye, GraphQL!";
    public string Greet(string name) => $"Hello, {name}!";
    public string Farewell(string name) => $"Goodbye, {name}!";
    public string GreetWithDefaultName(string name = "World") => $"Hello, {name}!";
    public string FarewellWithDefaultName(string name = "World") => $"Goodbye, {name}!";

    // TODO: remove above

}

