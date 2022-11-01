namespace SourceGeneratorTest.Clients;

public class BaseApiClient
{
    public string BaseUrl { get; protected set; }

    public async Task<StatusMessage<T>?> SendRequest<T>(string path, HttpMethod method, object? body = null)
    {
        //todo implement request with json response deserialized to T
        return null;
    }

    public async Task<StatusMessage> SendRequest(string path, HttpMethod method, object? body)
    {
        //todo implement request without typed response
        return null;
    }
}