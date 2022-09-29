namespace SFA.DAS.TrackProgress.Tests;

public static class HttpClientProviderExtension
{
    public static HttpClient ForProvider(this HttpClient client, long providerId)
    {
        client.DefaultRequestHeaders.Remove(SubscriptionHeaderConstants.ForProviderId);
        client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, $"Provider-{providerId}-TrackProgressOuterApi");
        return client;
    }
}