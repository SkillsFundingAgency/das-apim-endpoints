using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerFeedback;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingGetEmployerFeedbackAnnualReviewsRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Build(string timePeriod)
    {
        var actual = new GetEmployerFeedbackAnnualReviewsRequest(timePeriod);

        actual.GetUrl.Should().Be($"api/EmployerFeedbackResult/reviews?timeperiod={timePeriod}");
    }
}
