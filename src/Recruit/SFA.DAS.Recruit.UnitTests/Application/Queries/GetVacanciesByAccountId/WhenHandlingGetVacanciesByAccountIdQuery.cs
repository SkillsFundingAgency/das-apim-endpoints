using SFA.DAS.Recruit.Application.Queries.GetVacanciesByAccountId;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetVacanciesByAccountId;
[TestFixture]
internal class WhenHandlingGetVacanciesByUkprnQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetVacanciesByAccountIdQuery query,
        GetPagedVacancySummaryApiResponse apiResponse,
        GetEmployerAlertsApiResponse alertsApiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        GetVacanciesByAccountIdQueryHandler handler)
    {
        //Arrange
        var expectedGetUrl = new GetVacanciesByAccountIdApiRequest(query.AccountId, query.Page, query.PageSize, query.SortColumn, query.SortOrder, query.FilterBy, query.SearchTerm);
        recruitApiClient
            .Setup(x => x.Get<GetPagedVacancySummaryApiResponse>(
                It.Is<GetVacanciesByAccountIdApiRequest>(c => c.GetUrl.Equals(expectedGetUrl.GetUrl))))
            .ReturnsAsync(apiResponse);

        var expectedAlertsUrl = new GetEmployerAlertsApiRequest(query.AccountId, query.UserId);
        recruitApiClient
            .Setup(x => x.Get<GetEmployerAlertsApiResponse>(
                It.Is<GetEmployerAlertsApiRequest>(c => c.GetUrl.Equals(expectedAlertsUrl.GetUrl))))
            .ReturnsAsync(alertsApiResponse);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
        actual.BlockedProviderAlert.Should().Be(alertsApiResponse.BlockedProviderAlert);
        actual.BlockedProviderTransferredVacanciesAlert.Should().Be(alertsApiResponse.BlockedProviderTransferredVacanciesAlert);
        actual.EmployerRevokedTransferredVacanciesAlert.Should().Be(alertsApiResponse.EmployerRevokedTransferredVacanciesAlert);
        actual.WithDrawnByQaVacanciesAlert.Should().Be(alertsApiResponse.WithDrawnByQaVacanciesAlert);
    }
}
