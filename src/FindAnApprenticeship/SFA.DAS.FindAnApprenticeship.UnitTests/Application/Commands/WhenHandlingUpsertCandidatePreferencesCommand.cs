using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.CandidatePreferences;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands;
public class WhenHandlingUpsertCandidatePreferencesCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Put_Is_Sent_And_Data_Returned(
        UpsertCandidatePreferencesCommand command,
        ApiResponse<NullResponse> apiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        UpsertCandidatePreferencesCommandHandler handler)
    {
        var expectedRequest = new PutCandidatePreferencesApiRequest(command.CandidateId, new PutCandidatePreferencesRequestData());

        mockApiClient
            .Setup(client => client.PutWithResponseCode<NullResponse>(
                It.Is<PutCandidatePreferencesApiRequest>(c =>
                    c.PutUrl == expectedRequest.PutUrl)))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
    }

    [Test, MoqAutoData]
    public void And_Api_Returns_Null_Then_Return_Null(
        UpsertCandidatePreferencesCommand command,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        UpsertCandidatePreferencesCommandHandler handler)
    {
        mockApiClient.Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<PutCandidatePreferencesApiRequest>()))
            .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.BadRequest, "error"));

        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        act.Should().ThrowAsync<InvalidOperationException>();
    }
}
