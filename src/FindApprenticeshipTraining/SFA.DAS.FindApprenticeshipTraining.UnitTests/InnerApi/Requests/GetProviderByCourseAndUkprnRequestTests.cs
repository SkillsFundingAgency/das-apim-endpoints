using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests;

public class GetProviderByCourseAndUkprnRequestTests
{
    [Test, AutoData]
    public void WhenBuildingGetProviderByCourseAndUkprnRequest_ThenUrlIsCorrectlyBuilt(int providerId, int courseId, decimal latitude, decimal longitude)
    {
        var actual = new GetProviderByCourseAndUkprnRequest(providerId, courseId, latitude, longitude);

        actual.GetUrl.Should().Be($"api/courses/{courseId}/providers/{providerId}?latitude={latitude}&longitude={longitude}");
    }

    [Test, AutoData]
    public void WhenBuildingGetProviderByCourseAndUkprnRequest_WithNoLocationOrShortListUserId_ThenUrlIsCorrectlyBuilt(int providerId, int courseId)
    {
        var actual = new GetProviderByCourseAndUkprnRequest(providerId, courseId);

        actual.GetUrl.Should().Be($"api/courses/{courseId}/providers/{providerId}?latitude=&longitude=");
    }
}
