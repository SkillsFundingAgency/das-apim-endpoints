using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Assessor;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingGetEndpointAssessmentsRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(int ukprn)
    {
        //Act
        var sut = new GetEndpointAssessmentsRequest(ukprn);

        //Assert
        sut.GetUrl.Should().Be($"api/ao/assessments?ukprn={ukprn}");
    }
}