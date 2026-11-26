using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ApprenticeFeedback;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingGetApprenticeFeedbackAnnualReviewsRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Build(string timePeriod)
    {
        var actual = new GetApprenticeFeedbackAnnualReviewsRequest(timePeriod);

        actual.GetUrl.Should().Be($"api/ApprenticeFeedbackResult/reviews?timeperiod={timePeriod}");
    }
}
