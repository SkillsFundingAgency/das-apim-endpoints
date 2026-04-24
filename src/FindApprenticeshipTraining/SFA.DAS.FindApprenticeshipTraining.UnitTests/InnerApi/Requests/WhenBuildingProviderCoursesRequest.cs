using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Common;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests;

public sealed class WhenBuildingProviderCoursesRequest
{
    [Test, AutoData]
    public void GetUrl_WithUkprn_ReturnsExpectedUrl(long ukprn)
    {
        var sut = new ProviderCoursesRequest(ukprn);

        sut.GetUrl.Should().Be($"api/providers/{ukprn}/courses");
    }

    [Test, AutoData]
    public void Version_Default_ReturnsApiVersionTwo(long ukprn)
    {
        var sut = new ProviderCoursesRequest(ukprn);

        sut.Version.Should().Be(ApiVersionNumber.Two);
    }
}