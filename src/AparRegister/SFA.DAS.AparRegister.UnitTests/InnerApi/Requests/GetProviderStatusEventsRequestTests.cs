using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AparRegister.InnerApi.Requests;

namespace SFA.DAS.AparRegister.UnitTests.InnerApi.Requests;

public class GetProviderStatusEventsRequestTests
{
    [Test, AutoData]
    public void GetUrl_ReturnsExpectedValue(int sinceEventId, int pageSize, int pageNumber)
    {
        GetProviderStatusEventsRequest sut = new(sinceEventId, pageSize, pageNumber);

        sut.GetUrl.Should().Be($"organisations/status-events?sinceEventId={sinceEventId}&pageSize={pageSize}&pageNumber={pageNumber}");
    }
}
