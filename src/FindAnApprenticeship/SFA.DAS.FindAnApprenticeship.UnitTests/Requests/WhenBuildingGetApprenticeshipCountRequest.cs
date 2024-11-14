using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Requests
{
    public class WhenBuildingGetApprenticeshipCountRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Correctly_Built(WageType wageType)
        {
            var actual = new GetApprenticeshipCountRequest(wageType);

            actual.GetUrl.Should().Be($"/api/vacancies/count?wageType={wageType}");
        }

        [Test, AutoData]
        public void When_WageType_Is_Null_Or_Empty_Then_The_Request_Url_Is_Correctly_Built()
        {
            var actual = new GetApprenticeshipCountRequest();

            actual.GetUrl.Should().Be($"/api/vacancies/count?wageType=");
        }
    }
}