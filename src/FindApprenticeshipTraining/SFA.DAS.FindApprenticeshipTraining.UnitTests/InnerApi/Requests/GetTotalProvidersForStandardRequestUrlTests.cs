using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests;

public class GetTotalProvidersForStandardRequestUrlTests
{
    [Test, AutoData]
    public void WhenBuildingGetTotalProvidersForStandardRequest_ReturnsExpectedUrl(int standardId)
    {
        //Arrange Act
        var actual = new GetTotalProvidersForStandardRequest(standardId);

        //Assert
        actual.GetUrl.Should().Be($"/api/courses/{standardId}/providers/count");
    }
}