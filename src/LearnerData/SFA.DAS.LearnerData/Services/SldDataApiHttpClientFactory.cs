namespace SFA.DAS.LearnerData.Services;

// The shared api clients ask for the default HttpClient, so this hands them the named client that pins the SLD certificate
public class SldDataApiHttpClientFactory(IHttpClientFactory innerFactory) : IHttpClientFactory
{
    public const string ClientName = "SldDataApi";

    public HttpClient CreateClient(string name) => innerFactory.CreateClient(ClientName);
}
