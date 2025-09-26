using SFA.DAS.Recruit.Application.Queries.GetDashboardVacanciesCountByUkprn;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetDashboardVacanciesCountByUkprn;
[TestFixture]
public class WhenHandlingGetDashboardVacanciesCountByUkprn
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetDashboardVacanciesCountByUkprnQuery query,
        GetDashboardVacanciesCountApiResponse apiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        GetDashboardVacanciesCountByUkprnQueryHandler handler)
    {
        //Arrange
        var expectedGetUrl = new GetDashboardVacanciesCountByUkprnApiRequest(query.Ukprn, query.PageNumber, query.PageSize, query.SortColumn, query.IsAscending, query.Status);
        recruitApiClient
            .Setup(x => x.Get<GetDashboardVacanciesCountApiResponse>(
                It.Is<GetDashboardVacanciesCountByUkprnApiRequest>(r => r.GetUrl == expectedGetUrl.GetUrl)))
            .ReturnsAsync(apiResponse);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.Items.Should().BeEquivalentTo(apiResponse.Items);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Null_Returned(
        GetDashboardVacanciesCountByUkprnQuery query,
        GetDashboardVacanciesCountApiResponse apiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        GetDashboardVacanciesCountByUkprnQueryHandler handler)
    {
        //Arrange
        var expectedGetUrl = new GetDashboardVacanciesCountByUkprnApiRequest(query.Ukprn, query.PageNumber, query.PageSize, query.SortColumn, query.IsAscending, query.Status);
        recruitApiClient
            .Setup(x => x.Get<GetDashboardVacanciesCountApiResponse>(
                It.Is<GetDashboardVacanciesCountByUkprnApiRequest>(r => r.GetUrl == expectedGetUrl.GetUrl)))
            .ReturnsAsync((GetDashboardVacanciesCountApiResponse)null!);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.Items.Count.Should().Be(0);
    }
}
