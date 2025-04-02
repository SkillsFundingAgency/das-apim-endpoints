using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Application.Queries.GetUserOverview;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.UnitTests.Application.Queries;

public class GetUserOverviewQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Then_GetLatestDetails_from_Reference_Api(
         GetUserOverviewQuery query,
         GetUserOverviewResponse userApiResponse,
         List<Account> userAccounts,
         [Frozen] Mock<IAccountsService> accountsService,
         [Frozen] Mock<IInternalApiClient<EmployerProfilesApiConfiguration>> employerProfilesApi,
         GetUserOverviewQueryHandler handler)
    {
        accountsService.Setup(x => x.GetUserAccounts(query.UserId)).ReturnsAsync(userAccounts);

        var expectedUrl = $"api/users/{query.UserId}";

        employerProfilesApi.Setup(x => x.Get<GetUserOverviewResponse>(It.Is<GetUserByIdRequest>(r => r.GetUrl == expectedUrl)))
        .ReturnsAsync(userApiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(userApiResponse.Id);
        result.FirstName.Should().Be(userApiResponse.FirstName);
        result.LastName.Should().Be(userApiResponse.LastName);
        result.Email.Should().Be(userApiResponse.Email);
        result.IsActive.Should().Be(userApiResponse.IsActive);
        result.IsLocked.Should().Be(userApiResponse.IsLocked);
        result.IsSuspended.Should().Be(userApiResponse.IsSuspended);

        result.AccountSummaries.Should().NotBeNull();
        result.AccountSummaries.First().DasAccountName.Should().Be(userAccounts.First().DasAccountName);
        result.AccountSummaries.First().HashedAccountId.Should().Be(userAccounts.First().HashedAccountId);
    }
}
