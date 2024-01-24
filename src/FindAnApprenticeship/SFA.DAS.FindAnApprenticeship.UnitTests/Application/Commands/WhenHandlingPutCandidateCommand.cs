using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Candidate;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands;
public class WhenHandlingPutCandidateCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Put_Is_Sent_And_Data_Returned(
        PutCandidateCommand command,
        PutCandidateApiResponse response,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        PutCandidateCommandHandler handler)
    {
        var expectedPutData = new PutCandidateApiRequest.PutCandidateApiRequestData
        {
            Email = command.Email
        };
        var expectedRequest = new PutCandidateApiRequest(command.GovUkIdentifier, expectedPutData);

        mockApiClient
                .Setup(client => client.PutWithResponseCode<PutCandidateApiResponse>(
                    It.Is<PutCandidateApiRequest>(r => r.PutUrl == expectedRequest.PutUrl)))
                .ReturnsAsync(new ApiResponse<PutCandidateApiResponse>(response, HttpStatusCode.OK, string.Empty));

        var result = await handler.Handle(command, CancellationToken.None);

        using (new AssertionScope())
        {
            result.GovUkIdentifier.Should().BeEquivalentTo(response.GovUkIdentifier);
            result.Email.Should().BeEquivalentTo(response.Email);
            result.FirstName.Should().BeEquivalentTo(response.FirstName);
            result.LastName.Should().BeEquivalentTo(response.LastName);
        }
    }

    [Test, MoqAutoData]
    public async Task And_Api_Returns_Null_Then_Return_Null(
        PutCandidateCommand command,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        PutCandidateCommandHandler handler)
    {
        var expectedPutData = new PutCandidateApiRequest.PutCandidateApiRequestData
        {
            Email = command.Email
        };
        var expectedRequest = new PutCandidateApiRequest(command.GovUkIdentifier, expectedPutData);

        mockApiClient
                .Setup(client => client.PutWithResponseCode<PutCandidateApiResponse>(
                    It.Is<PutCandidateApiRequest>(r => r.PutUrl == expectedRequest.PutUrl)))
                .ReturnsAsync(() => null);

        Func<Task> result = () => handler.Handle(command, CancellationToken.None);

        await result.Should().ThrowExactlyAsync<ArgumentNullException>();
    }
}
