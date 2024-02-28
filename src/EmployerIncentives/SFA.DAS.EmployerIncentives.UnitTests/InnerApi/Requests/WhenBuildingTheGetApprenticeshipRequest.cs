using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.Commitments;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetApprenticeshipRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(long apprenticeshipId)
        {
            var actual = new GetApprenticeshipRequest(apprenticeshipId);

            actual.GetUrl.Should().Be($"api/apprenticeships/{apprenticeshipId}");
        }
    }
}