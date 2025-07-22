using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.InnerApi;
[TestFixture]
public class WhenBuildingGetCivilServiceJobsApiRequest
{
    [Test]
    public void Then_The_Url_Is_Correctly_Constructed()
    {
        var actual = new GetCivilServiceJobsApiRequest();
        actual.GetUrl.Should().Be("/csj/v1/jobs?contractType=Apprenticeship");
    }
}