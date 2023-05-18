using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Apprentices;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.InnerApi.Apprentices;

public class GetApprenticeRequestTests
{
    [Test, AutoData]
    public void GetUrl_HasCorrectUrl(Guid apprenticeId)
    {
        GetApprenticeRequest sut = new(apprenticeId);

        sut.GetUrl.Should().Be($"apprentices/{apprenticeId}");
    }
}
