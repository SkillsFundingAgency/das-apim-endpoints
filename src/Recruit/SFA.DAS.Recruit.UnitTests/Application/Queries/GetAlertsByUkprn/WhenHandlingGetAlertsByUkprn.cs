using SFA.DAS.Recruit.Application.Queries.GetAlertsByUkprn;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetAlertsByUkprn;
[TestFixture]
internal class WhenHandlingGetAlertsByUkprn
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetAlertsByUkprnQuery query,
        GetProviderAlertsApiResponse alertsApiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        GetAlertsByUkprnQueryHandler handler)
    {
        //Arrange
        var expectedAlertsUrl = new GetProviderAlertsApiRequest(query.Ukprn, query.UserId);
        recruitApiClient
            .Setup(x => x.Get<GetProviderAlertsApiResponse>(
                It.Is<GetProviderAlertsApiRequest>(c => c.GetUrl.Equals(expectedAlertsUrl.GetUrl))))
            .ReturnsAsync(alertsApiResponse);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.Should().BeEquivalentTo(alertsApiResponse, options => options.ExcludingMissingMembers());
        actual.ProviderTransferredVacanciesAlert.Should().Be(alertsApiResponse.ProviderTransferredVacanciesAlert);
        actual.WithdrawnVacanciesAlert.Should().Be(alertsApiResponse.WithdrawnVacanciesAlert);
    }
}
