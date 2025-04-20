
# Using appsettings.json and User Secrets for Local Development

When working with C# ASP.NET applications and integrating with Confluent Cloud APIs, it's important to avoid committing sensitive information, such as API keys, to source control. This guide explains how to use `appsettings.json` in combination with User Secrets for secure local development.

## Steps to Configure API Keys Securely

### 1. Add Configuration in `appsettings.json`
Create an `appsettings.json` file in your project if it doesn't already exist. Add a placeholder for your Confluent Cloud API keys:

```json
{
    "Producer": {
        "SaslUsername": "<Confluent-API-Key>",
        "SaslPassword": "<Confluent-API-Key Secret>",
    }
}
```

### 2. Use User Secrets for Local Development
User Secrets is a feature in ASP.NET Core that allows you to store sensitive information securely during development.

#### Enable User Secrets
Run the following command in your project directory to enable User Secrets:

```bash
dotnet user-secrets init
```

This will add a `UserSecretsId` entry to your `.csproj` file.

#### Add API Keys to User Secrets
Use the following command to set your API keys in User Secrets:

```bash
dotnet user-secrets set "ConfluentCloud:ApiKey" "your-api-key"
dotnet user-secrets set "ConfluentCloud:ApiSecret" "your-api-secret"
dotnet user-secrets set "Producer:SaslUsername" "your-api-key"
dotnet user-secrets set "Consumer:SaslUsername" "your-api-key"
dotnet user-secrets set "Producer:SaslPassword" "your-api-secret"
dotnet user-secrets set "Consumer:SaslPassword" "your-api-secret"
dotnet user-secrets set "SchemaRegistry:BasicAuthUserInfo" "<Confluent-Schema-Registry-API-Key>:<Confluent-Schema-Registry-API-Key-Secret>"
```

### 3. Ensure Secrets Are Not Committed
Add the `appsettings.json` file to your `.gitignore` file if it contains sensitive placeholders. User Secrets are stored outside the project directory and are not included in source control.

### 4. Deploying to Production
For production environments, use environment variables or a secure secrets management solution to inject the API keys.

## Additional Resources
- [Microsoft Docs: Safe storage of app secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets)
- [Confluent Cloud Documentation](https://docs.confluent.io/cloud/current/)

By following these steps, you can securely manage your Confluent Cloud API keys during local development without exposing them in source control.

