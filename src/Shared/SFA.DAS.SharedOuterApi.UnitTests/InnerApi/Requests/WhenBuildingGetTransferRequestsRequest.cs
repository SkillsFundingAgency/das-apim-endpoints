using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetTransferRequestsRequest
    {
        [Test, MoqAutoData]
        public void And_No_Originator_Supplied_Then_The_GetUrl_Is_Correctly_Built(
           long accountId
           )
        {
            var actual = new GetTransferRequestsRequest(accountId, null);
            var expected = $"api/accounts/{accountId}/transfers";

            actual.GetUrl.Should().Be(expected);
        }

        [Test, MoqAutoData]
        public void And_Originator_Supplied_Then_The_GetUrl_Is_Correctly_Built(
                      long accountId,
                      TransferType originator
                      )
        {
            var actual = new GetTransferRequestsRequest(accountId, originator);
            var expected = $"api/accounts/{accountId}/transfers?originator={originator}";

            actual.GetUrl.Should().Be(expected);
        }
    }
}