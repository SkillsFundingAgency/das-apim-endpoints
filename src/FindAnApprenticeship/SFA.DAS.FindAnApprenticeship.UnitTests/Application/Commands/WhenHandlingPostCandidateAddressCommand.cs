using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.Address;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands;
public class WhenHandlingPostCandidateAddressCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Address_Record_Is_Created(
        CreateAddressCommand command,
        LocationItem locationItem,
        ApiResponse<PostCandidateAddressApiResponse> apiResponse,
        [Frozen] Mock<ILocationLookupService> locationLookupService,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        CreateAddressCommandHandler handler)
    {
        var expectedRequest = new PutCandidateAddressApiRequest(command.CandidateId, new PutCandidateAddressApiRequestData());

        locationLookupService.Setup(x => x.GetLocationInformation(command.Postcode, default, default, false)).ReturnsAsync(locationItem);

        candidateApiClient
            .Setup(client => client.PutWithResponseCode<PostCandidateAddressApiResponse>(
                It.Is<PutCandidateAddressApiRequest>(r => r.PutUrl == expectedRequest.PutUrl)))
                .ReturnsAsync(apiResponse);

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.Should().NotBeNull();
        locationLookupService.Verify(x => x.GetLocationInformation(command.Postcode, default, default, false), Times.Once);
    }
}
