using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerFeedback;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingGetEmployerFeedbackDetailsAnnualRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(int ukprn)
    {
        //Act
        var sut = new GetEmployerFeedbackDetailsAnnualRequest(ukprn);

        //Assert
        sut.GetUrl.Should().Be($"api/EmployerFeedbackResult/{ukprn}/annual");
    }
}