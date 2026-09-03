using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.AdminRoatp.InnerApi.Requests;

namespace SFA.DAS.AdminRoatp.UnitTests.InnerApi.Requests;

public class GetProviderCourseTypesRequestTests
{
    [Test, AutoData]
    public void WhenCreatingRequest_ThenSetsCorrectUrl(int ukprn)
    {
        // Arrange
        var sut = new GetProviderCourseTypesRequest(ukprn);

        // Assert
        sut.GetUrl.Should().Be($"providers/{ukprn}/course-types");
    }
}
