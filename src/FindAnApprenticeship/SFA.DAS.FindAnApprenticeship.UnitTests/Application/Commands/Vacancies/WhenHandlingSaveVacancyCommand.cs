using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Vacancies.SaveVacancy;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
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
            SaveVacancyCommandHandler handler)
        {
            candidateApiClient
                .Setup(client => client.PutWithResponseCode<PutSavedVacancyApiResponse>(
                    It.IsAny<PutSavedVacancyApiRequest>()))
                .ReturnsAsync(new ApiResponse<PutSavedVacancyApiResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

            var actual = await handler.Handle(command, CancellationToken.None);

            actual.Should().NotBeNull();
            actual.Id.Should().Be(apiResponse.Id);
        }

        [Test, MoqAutoData]
        public void And_Api_Returns_Null_Then_Return_Null(
            SaveVacancyCommand command,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            SaveVacancyCommandHandler handler)
        {
            candidateApiClient.Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<PutSavedVacancyApiRequest>()))
                .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.BadRequest, "error"));

            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            act.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
