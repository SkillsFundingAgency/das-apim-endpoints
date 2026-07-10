using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.RecruitJobs.Api.Controllers;
using System.Net;
using System.Threading;

namespace SFA.DAS.RecruitJobs.Api.UnitTests.Controllers.VacancyControllerTests;

[TestFixture]
internal class WhenPuttingVacancyAnalyticsByVacancyReference
{
    [Test, MoqAutoData]
    public async Task Then_The_Request_To_Put_Message_Is_Sent_Correctly(
        long vacancyReference,
        PutVacancyAnalyticsRequest request,
        [Frozen] Mock<SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Greedy] VacanciesController sut)
    {
        // arrange
        recruitApiClient
            .Setup(x => x.Put(It.IsAny<PutVacancyanalyticsByVacancyReferenceApiRequest>()))
            .Returns(Task.CompletedTask);

        // act
        var result = await sut.PutOne(recruitApiClient.Object, vacancyReference, request);

        // assert
        result.Should().BeOfType<Created>();
    }

    [Test, MoqAutoData]
    public async Task PutOne_SendsCorrectRequestToRecruitApi(long vacancyReference,
        PutVacancyAnalyticsRequest request,
        [Frozen] Mock<SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Greedy] VacanciesController sut)
    {
        // Arrange
        PutVacancyanalyticsByVacancyReferenceApiRequest? capturedRequest = null;
        recruitApiClient
            .Setup(x => x.Put(It.IsAny<PutVacancyanalyticsByVacancyReferenceApiRequest>()))
            .Callback<object>(r => capturedRequest = (PutVacancyanalyticsByVacancyReferenceApiRequest)r)
            .Returns(Task.CompletedTask);

        // Act
        await sut.PutOne(
            recruitApiClient.Object,
            vacancyReference,
            request,
            CancellationToken.None);

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.VacancyReference.Should().Be(vacancyReference);
        capturedRequest.Data.AnalyticsData.Should().BeEquivalentTo(request.AnalyticsData);
    }

    [Test, MoqAutoData]
    public async Task PutOne_WhenExceptionThrown_ReturnsInternalServerError(long vacancyReference,
        [Frozen] Mock<SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Greedy] VacanciesController sut)
    {
        // Arrange
        recruitApiClient
            .Setup(x => x.Put(It.IsAny<PutVacancyanalyticsByVacancyReferenceApiRequest>()))
            .ThrowsAsync(new Exception("Boom"));

        var request = new PutVacancyAnalyticsRequest
        {
            AnalyticsData = []
        };

        // Act
        var result = await sut.PutOne(
            recruitApiClient.Object,
            1,
            request,
            CancellationToken.None);

        // Assert
        var problem = result.Should().BeOfType<ProblemHttpResult>().Subject;
        problem.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
