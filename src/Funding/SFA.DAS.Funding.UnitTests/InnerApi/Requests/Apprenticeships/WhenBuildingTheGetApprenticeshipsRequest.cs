using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Funding.InnerApi.Requests.Apprenticeships;

namespace SFA.DAS.Funding.UnitTests.InnerApi.Requests.ProviderEarnings
{
    public class WhenBuildingTheGetApprenticeshipsRequest
    {
        [Test, AutoData]
        public void Then_The_GetAllUrl_Is_Correctly_Built(long ukprn)
        {
            var actual = new GetApprenticeshipsRequest(ukprn);

            actual.GetAllUrl.Should().Be($"{ukprn}/apprenticeships");
        }
    }
}