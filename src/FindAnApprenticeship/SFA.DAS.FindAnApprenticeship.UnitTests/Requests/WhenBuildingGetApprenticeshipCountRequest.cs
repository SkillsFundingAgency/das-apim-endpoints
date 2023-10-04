using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Requests
{
    public class WhenBuildingGetApprenticeshipCountRequest
    {
        [Test]
        public void Then_The_Request_Url_Is_Correctly_Built()
        {
            var actual = new GetApprenticeshipCountRequest();

            actual.GetUrl.Should().Be("/api/vacancies/count");
        }
    }
}