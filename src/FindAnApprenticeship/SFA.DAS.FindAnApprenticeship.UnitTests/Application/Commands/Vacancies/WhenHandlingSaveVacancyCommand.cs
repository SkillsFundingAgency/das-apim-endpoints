using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Vacancies.SaveVacancy;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Net;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Vacancies
{
    [TestFixture]
    public class WhenHandlingSaveVacancyCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Saved_Vacancy_Is_Created(
            SaveVacancyCommand command,
            PutSavedVacancyApiResponse apiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            [Frozen] Mock<IMetrics> metrics,
            SaveVacancyCommandHandler handler)
        {
            candidateApiClient
                .Setup(client => client.PutWithResponseCode<PutSavedVacancyApiResponse>(
                    It.IsAny<PutSavedVacancyApiRequest>()))
                .ReturnsAsync(new ApiResponse<PutSavedVacancyApiResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

            var actual = await handler.Handle(command, CancellationToken.None);

            actual.Should().NotBeNull();
            actual.Id.Should().Be(apiResponse.Id);
            metrics.Verify(x => x.IncreaseVacancySaved(It.IsAny<string>(), 1), Times.Exactly(1));
        }

        [Test, MoqAutoData]
        public void And_Api_Returns_Null_Then_Return_Null(
            SaveVacancyCommand command,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            [Frozen] Mock<IMetrics> metrics,
            SaveVacancyCommandHandler handler)
        {
            candidateApiClient.Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<PutSavedVacancyApiRequest>()))
                .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.BadRequest, "error"));

            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            act.Should().ThrowAsync<InvalidOperationException>();

            metrics.Verify(x => x.IncreaseVacancySaved(It.IsAny<string>(), 1), Times.Never());
        }
    }
}