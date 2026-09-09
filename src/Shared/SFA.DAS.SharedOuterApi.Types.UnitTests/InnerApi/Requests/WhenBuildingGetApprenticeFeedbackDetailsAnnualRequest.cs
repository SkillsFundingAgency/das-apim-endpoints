using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ApprenticeFeedback;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingGetApprenticeFeedbackDetailsAnnualRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(int ukprn)
    {
        //Act
        var sut = new GetApprenticeFeedbackDetailsAnnualRequest(ukprn);

        //Assert
        sut.GetUrl.Should().Be($"api/ApprenticeFeedbackResult/{ukprn}/annual");
    }
}