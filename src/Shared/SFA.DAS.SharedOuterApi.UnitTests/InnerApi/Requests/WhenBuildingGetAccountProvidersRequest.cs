using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;
internal class WhenBuildingGetAccountProvidersRequest
{
    public class WhenBuildingGetProviderAccountLegalEntitiesRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(long accountId)
        {
            var actual = new GetAccountProvidersRequest(accountId);

            actual.GetUrl.Should().Be($"accounts/{accountId}/providers");
        }
    }
}
