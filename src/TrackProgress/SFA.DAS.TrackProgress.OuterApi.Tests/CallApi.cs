using FluentAssertions;

namespace SFA.DAS.TrackProgress.Tests;

public class CallApi : ApiFixture
{
    [Test]
    public async Task Can_call_API()
    {
        var response = await client.GetAsync("/");
        response.Should().Be404NotFound();
    }
}
