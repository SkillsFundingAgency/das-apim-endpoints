using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Requests
{
    public class WhenBuildingGetApprenticeshipCountRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Correctly_Built(double lat, double lon, List<string> categories, int distance, string whatSearchTerm)
        {
            var actual = new GetApprenticeshipCountRequest();

            actual.GetUrl.Should().Be($"/api/vacancies/count");
        }

        [Test, AutoData]
        public void When_Categories_Are_Null_Or_Empty_Then_The_Request_Url_Is_Correctly_Built(double lat, double lon, int distance, string whatSearchTerm)
        {
            var actual = new GetApprenticeshipCountRequest();

            actual.GetUrl.Should().Be($"/api/vacancies/count");
        }
    }
}