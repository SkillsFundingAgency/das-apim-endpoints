using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerFinance;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetTransferConnectionsRequest
    {
        [Test, MoqAutoData]
        public void And_No_TransferConnectionInvitationStatus_Supplied_Then_The_GetUrl_Is_Correctly_Built(
            long accountId
            )
        {
            var actual = new GetTransferConnectionsRequest { AccountId = accountId };
            var expected = $"api/accounts/internal/{accountId}/transfers/connections";

            actual.GetUrl.Should().Be(expected);
        }

        [Test, MoqAutoData]
        public void And_TransferConnectionInvitationStatus_Supplied_Then_The_GetUrl_Is_Correctly_Built(
                        long accountId,
                        TransferConnectionInvitationStatus status
                        )
        {
            var actual = new GetTransferConnectionsRequest { AccountId = accountId, Status = status };
            var expected = $"api/accounts/internal/{accountId}/transfers/connections?status={status}";

            actual.GetUrl.Should().Be(expected);
        }
    }
}