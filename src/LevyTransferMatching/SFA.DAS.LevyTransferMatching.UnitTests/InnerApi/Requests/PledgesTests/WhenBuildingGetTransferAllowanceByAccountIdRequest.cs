using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Finance;

namespace SFA.DAS.LevyTransferMatching.UnitTests.InnerApi.Requests.PledgesTests
{
    public class WhenBuildingGetTransferAllowanceByAccountIdRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(long id)
        {
            var actual = new GetTransferAllowanceByAccountIdRequest(id);

            actual.GetUrl.Should().Be($"api/accounts/{id}/transferAllowanceByAccountId");
        }
    }
}
