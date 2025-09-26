using SFA.DAS.Recruit.Application.Queries.GetVacanciesByUkprn;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetVacanciesByUkprn;
[TestFixture]
internal class WhenHandlingGetVacanciesByUkprnQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetVacanciesByUkprnQuery query,
        GetPagedVacancySummaryApiResponse apiResponse,
        GetProviderAlertsApiResponse alertsApiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        GetVacanciesByUkprnQueryHandler handler)
    {
        //Arrange
        var expectedGetUrl = new GetVacanciesByUkprnApiRequest(query.Ukprn, query.Page, query.PageSize, query.SortColumn, query.SortOrder, query.FilterBy, query.SearchTerm);
        recruitApiClient
            .Setup(x => x.Get<GetPagedVacancySummaryApiResponse>(
                It.Is<GetVacanciesByUkprnApiRequest>(c => c.GetUrl.Equals(expectedGetUrl.GetUrl))))
            .ReturnsAsync(apiResponse);

        var expectedAlertsUrl = new GetProviderAlertsApiRequest(query.Ukprn, query.UserId);
        recruitApiClient
            .Setup(x => x.Get<GetProviderAlertsApiResponse>(
                It.Is<GetProviderAlertsApiRequest>(c => c.GetUrl.Equals(expectedAlertsUrl.GetUrl))))
            .ReturnsAsync(alertsApiResponse);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
        actual.ProviderTransferredVacanciesAlert.Should().Be(alertsApiResponse.ProviderTransferredVacanciesAlert);
        actual.WithdrawnVacanciesAlert.Should().Be(alertsApiResponse.WithdrawnVacanciesAlert);
    }
}