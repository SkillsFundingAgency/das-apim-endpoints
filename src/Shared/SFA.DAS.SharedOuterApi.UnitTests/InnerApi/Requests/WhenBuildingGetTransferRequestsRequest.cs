using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetTransferRequestsRequest
    {
        [Test, MoqAutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(
          long accountId
          )
        {
            var actual = new GetTransferRequestsRequest(accountId);
            var expected = $"api/accounts/{accountId}/transfers";

            actual.GetUrl.Should().Be(expected);
        }
    }
}