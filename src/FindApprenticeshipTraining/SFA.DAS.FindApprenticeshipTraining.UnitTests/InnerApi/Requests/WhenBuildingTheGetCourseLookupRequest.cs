using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests;

public class WhenBuildingTheGetCourseLookupRequest
{
    [Test, AutoData]
    public void GetUrl_WhenBuildingLookupRequest_ReturnsExpectedUrl(GetCourseLookupRequest actual)
    {
        actual.GetUrl.Should().Be($"api/courses/lookup/{actual.Id}");
    }
}