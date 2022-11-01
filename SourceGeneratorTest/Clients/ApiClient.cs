namespace SourceGeneratorTest.Clients;

[GeneratedApiClient("SourceGeneratorTest.Controllers")]
public partial class ApiClient : BaseApiClient
{
    public ApiClient()
    {
        BaseUrl = "https://localhost:6010/";//set from account tho
    }
}