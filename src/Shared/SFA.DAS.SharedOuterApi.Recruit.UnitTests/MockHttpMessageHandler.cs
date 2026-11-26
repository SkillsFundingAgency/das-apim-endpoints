using System.Collections.ObjectModel;

namespace SFA.DAS.SharedOuterApi.Recruit.UnitTests;

public class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly List<HttpRequestMessage> _requests = [];
    public readonly Queue<HttpResponseMessage> Responses = new();
    public ReadOnlyCollection<HttpRequestMessage> Requests => new(_requests);

    public MockHttpMessageHandler(IEnumerable<HttpResponseMessage> responses)
    {
        foreach (var response in responses)
        {
            Responses.Enqueue(response);
        }
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _requests.Add(request);
        return Task.FromResult(Responses.Dequeue());
    }
}