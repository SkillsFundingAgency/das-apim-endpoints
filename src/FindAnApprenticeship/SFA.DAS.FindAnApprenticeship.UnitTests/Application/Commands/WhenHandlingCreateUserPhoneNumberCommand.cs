using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.PhoneNumber;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands;
public class WhenHandlingCreateUserPhoneNumberCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Put_Is_Sent_And_Data_Returned(
        CreatePhoneNumberCommand command,
        PutCandidateApiResponse apiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        CreatePhoneNumberCommandHandler handler)
    {
        var expectedPutData = new PutCandidateApiRequestData
        {
            Email = command.Email,
            PhoneNumber = command.PhoneNumber
        };

        var expectedRequest = new PutCandidateApiRequest(command.GovUkIdentifier, expectedPutData);

        mockApiClient
            .Setup(client => client.PutWithResponseCode<PutCandidateApiResponse>(
                It.Is<PutCandidateApiRequest>(c =>
                    c.PutUrl == expectedRequest.PutUrl
                    && ((PutCandidateApiRequestData)c.Data).Email == command.Email)))
            .ReturnsAsync(new ApiResponse<PutCandidateApiResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
    }

    [Test, MoqAutoData]
    public void And_Api_Returns_Null_Then_Return_Null(
        CreatePhoneNumberCommand command,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        CreatePhoneNumberCommandHandler handler)
    {
        mockApiClient.Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<PutCandidateApiRequest>()))
            .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.BadRequest, "error"));

        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        act.Should().ThrowAsync<InvalidOperationException>();
    }
}
