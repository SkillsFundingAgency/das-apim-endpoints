using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateSkillsAndStrengthsCommand;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply;
public class WhenHandlingCreateSkillsAndStrengthsCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_SkillsAndStrengths_Is_Created(
        CreateSkillsAndStrengthsCommand command,
        PutUpsertSkillsAndStrengthsApiResponse apiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        CreateSkillsAndStrengthsCommandHandler handler)
    {
        var expectedRequest = new PutUpsertSkillsAndStrengthsApiRequest(command.ApplicationId, command.CandidateId, Guid.NewGuid(), new PutUpsertSkillsAndStrengthsApiRequest.PutUpdateSkillsAndStrengthsApiRequestData());

        candidateApiClient
            .Setup(client => client.PutWithResponseCode<PutUpsertSkillsAndStrengthsApiResponse>(
                It.Is<PutUpsertSkillsAndStrengthsApiRequest>(r => r.PutUrl.StartsWith(expectedRequest.PutUrl.Substring(0, 86)))))
            .ReturnsAsync(new ApiResponse<PutUpsertSkillsAndStrengthsApiResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

        var actual = await handler.Handle(command, CancellationToken.None);

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.Id.Should().NotBeEmpty();
        }
    }
}
