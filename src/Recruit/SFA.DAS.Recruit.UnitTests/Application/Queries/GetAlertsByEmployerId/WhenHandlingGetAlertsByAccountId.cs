using SFA.DAS.Recruit.Application.Queries.GetAlertsByAccountId;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetAlertsByEmployerId;
[TestFixture]
internal class WhenHandlingGetAlertsByAccountId
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetAlertsByAccountIdQuery query,
        GetEmployerAlertsApiResponse alertsApiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        GetAlertsByAccountIdQueryHandler handler)
    {
        //Arrange
        var expectedAlertsUrl = new GetEmployerAlertsApiRequest(query.AccountId, query.UserId);
        recruitApiClient
            .Setup(x => x.Get<GetEmployerAlertsApiResponse>(
                It.Is<GetEmployerAlertsApiRequest>(c => c.GetUrl.Equals(expectedAlertsUrl.GetUrl))))
            .ReturnsAsync(alertsApiResponse);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.Should().BeEquivalentTo(alertsApiResponse, options => options.ExcludingMissingMembers());
        actual.BlockedProviderAlert.Should().Be(alertsApiResponse.BlockedProviderAlert);
        actual.BlockedProviderTransferredVacanciesAlert.Should().Be(alertsApiResponse.BlockedProviderTransferredVacanciesAlert);
        actual.EmployerRevokedTransferredVacanciesAlert.Should().Be(alertsApiResponse.EmployerRevokedTransferredVacanciesAlert);
        actual.WithDrawnByQaVacanciesAlert.Should().Be(alertsApiResponse.WithDrawnByQaVacanciesAlert);
    }
}