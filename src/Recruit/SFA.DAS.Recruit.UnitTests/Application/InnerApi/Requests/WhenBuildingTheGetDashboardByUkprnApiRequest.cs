using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests
{
    public class WhenBuildingTheGetDashboardByUkprnApiRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(int ukprn, ApplicationStatus status)
        {
            //Act
            var actual = new GetDashboardByUkprnApiRequest(ukprn, status);
            
            //Assert
            actual.GetUrl.Should().Be($"api/provider/{ukprn}/applicationReviews/dashboard?status={status}");
        }
    }
}