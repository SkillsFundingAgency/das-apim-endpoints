using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ApprenticeAan.Application.InnerApi.MyApprenticeships;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.InnerApi.MyApprenticeships;

public class PostMyApprenticeshipRequestTests
{
    [Test, AutoData]
    public void HasCorrectPostUrl(Guid id)
    {
        PostMyApprenticeshipRequest sut = new(id);

        sut.PostUrl.Should().Be($"apprentices/{id}/MyApprenticeship");
    }
}
