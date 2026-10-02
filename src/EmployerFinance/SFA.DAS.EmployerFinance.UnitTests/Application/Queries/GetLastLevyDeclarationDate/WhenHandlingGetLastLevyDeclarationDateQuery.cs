using SFA.DAS.EmployerFinance.Application.Queries.GetLastLevyDeclarationDate;
using SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;
using SFA.DAS.EmployerFinance.InnerApi.Responses.Finance;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.GetLastLevyDeclarationDate;

[TestFixture]
internal class WhenHandlingGetLastLevyDeclarationDateQuery
{
    [Test, MoqAutoData]
    public async Task Then_Returns_Expected_Result(
        long accountId,
        GetLevyLastSubmissionDateResponse response,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Greedy] GetLastLevyDeclarationDateQueryHandler handler)
    {
        // Arrange
        var query = new GetLastLevyDeclarationDateQuery(accountId);
        mockFinanceApiClient.Setup(client => client.Get<GetLevyLastSubmissionDateResponse>(new GetLevyLastSubmissionDateRequest(accountId)))
            .ReturnsAsync(response);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.LatestLevyDeclarationInDate.Should().BeCloseTo(response.LastSubmissionDate, TimeSpan.FromSeconds(1));
    }
}