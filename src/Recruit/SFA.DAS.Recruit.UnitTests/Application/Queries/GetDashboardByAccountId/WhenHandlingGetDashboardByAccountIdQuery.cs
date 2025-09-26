using SFA.DAS.Recruit.Application.Queries.GetDashboardByAccountId;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetDashboardByAccountId
{
    [TestFixture]
    public class WhenHandlingGetDashboardByUkprnQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Data_Returned(
            GetDashboardByAccountIdQuery query,
            GetEmployerDashboardApiResponse apiResponse,
            GetEmployerAlertsApiResponse alertsApiResponse,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            GetDashboardByAccountIdQueryHandler handler)
        {
            //Arrange
            var expectedGetUrl = new GetDashboardByAccountIdApiRequest(query.AccountId);
            recruitApiClient
                .Setup(x => x.Get<GetEmployerDashboardApiResponse>(
                    It.Is<GetDashboardByAccountIdApiRequest>(c => c.GetUrl.Equals(expectedGetUrl.GetUrl))))
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
}
