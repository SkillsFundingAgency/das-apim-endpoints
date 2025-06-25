using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Funding.InnerApi.Requests.Learning;

namespace SFA.DAS.Funding.UnitTests.InnerApi.Requests.Apprenticeships
{
    public class WhenBuildingTheGetApprenticeshipsRequest
    {
        [Test, AutoData]
        public void Then_The_GetAllUrl_Is_Correctly_Built(long ukprn)
        {
            var actual = new GetLearningsRequest(ukprn);

            actual.GetAllUrl.Should().Be($"{ukprn}/learnings");
        }
    }
}