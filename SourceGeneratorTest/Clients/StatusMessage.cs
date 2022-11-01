namespace SourceGeneratorTest.Clients;

public class StatusMessage<T> : StatusMessage
{
    public T ResponseData { get; set; }
}

public class StatusMessage
{
    public bool Success { get; set; }
    public string Response { get; set; }
}