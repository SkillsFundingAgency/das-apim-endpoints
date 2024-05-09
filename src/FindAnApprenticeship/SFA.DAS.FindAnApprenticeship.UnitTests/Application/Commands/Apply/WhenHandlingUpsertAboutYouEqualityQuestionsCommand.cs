using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateEqualityQuestionsCommand;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply
{
    public class WhenHandlingUpsertAboutYouEqualityQuestionsCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_SkillsAndStrengths_Is_Created(
        UpsertAboutYouEqualityQuestionsCommand command,
        GetAboutYouItemApiResponse apiResponse,
        PutUpsertAboutYouItemApiResponse createSkillsAndStrengthsApiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        UpsertAboutYouEqualityQuestionsCommandHandler handler)
        {
            var expectedRequest = new PutUpsertAboutYouItemApiRequest(command.ApplicationId, command.CandidateId, Guid.NewGuid(), new PutUpsertAboutYouItemApiRequest.PutUpdateAboutYouItemApiRequestData());

            candidateApiClient
                .Setup(client => client.PutWithResponseCode<PutUpsertAboutYouItemApiResponse>(
                    It.Is<PutUpsertAboutYouItemApiRequest>(r => r.PutUrl.StartsWith(expectedRequest.PutUrl.Substring(0, 86)))))
                .ReturnsAsync(new ApiResponse<PutUpsertAboutYouItemApiResponse>(createSkillsAndStrengthsApiResponse, HttpStatusCode.OK, string.Empty));

            var expectedApiRequest = new GetAboutYouItemApiRequest(command.ApplicationId, command.CandidateId);
            candidateApiClient
                .Setup(client => client.Get<GetAboutYouItemApiResponse>(
                    It.Is<GetAboutYouItemApiRequest>(r => r.GetUrl == expectedApiRequest.GetUrl)))
                .ReturnsAsync(apiResponse);

            var actual = await handler.Handle(command, CancellationToken.None);

            using (new AssertionScope())
            {
                actual.Should().NotBeNull();
                actual.Id.Should().NotBeEmpty();
                actual.Id.Should().Be(createSkillsAndStrengthsApiResponse.Id);
            }
        }

        [Test, MoqAutoData]
        public async Task Then_The_Update_Application_Status_Api_Response_NotFound_CommandResult_Is_Returned_As_Expected(
            UpsertAboutYouEqualityQuestionsCommand command,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            [Frozen] UpsertAboutYouEqualityQuestionsCommandHandler handler)
        {
            var expectedRequest = new PutUpsertAboutYouItemApiRequest(command.ApplicationId, command.CandidateId, Guid.NewGuid(), new PutUpsertAboutYouItemApiRequest.PutUpdateAboutYouItemApiRequestData());

            candidateApiClient
                .Setup(client => client.PutWithResponseCode<PutUpsertAboutYouItemApiResponse>(
                    It.Is<PutUpsertAboutYouItemApiRequest>(r => r.PutUrl.StartsWith(expectedRequest.PutUrl.Substring(0, 86)))))
                 .ReturnsAsync((Func<ApiResponse<PutUpsertAboutYouItemApiResponse>>)null!);

            Func<Task> act = async () => { await handler.Handle(command, CancellationToken.None); };

            await act.Should().ThrowAsync<ArgumentException>();
        }
    }
}
