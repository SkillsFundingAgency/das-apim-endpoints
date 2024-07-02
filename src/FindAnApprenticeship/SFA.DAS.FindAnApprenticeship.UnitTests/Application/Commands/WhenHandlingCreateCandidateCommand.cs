﻿using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Candidate;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses.Enums;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands;
public class WhenHandlingPostCandidateCommand
{
    [Test, MoqAutoData]
    public async Task Then_If_Candidate_Already_Exists_Then_Details_Are_Returned(
        CreateCandidateCommand command,
        string govUkId,
        GetCandidateApiResponse candidate,
        PostCandidateApiResponse response,
        GetLegacyUserByEmailApiResponse legacyUserByEmailApiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        [Frozen] Mock<IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration>> mockLegacyApiClient,
        CreateCandidateCommandHandler handler)
    {
        command.GovUkIdentifier = govUkId;
        command.Email = candidate.Email;

        var expectedGetCandidateRequest = new GetCandidateApiRequest(govUkId);
        mockApiClient.Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(
                It.Is<GetCandidateApiRequest>(r => r.GetUrl == expectedGetCandidateRequest.GetUrl)))
                .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(candidate, HttpStatusCode.OK, string.Empty));

        var result = await handler.Handle(command, CancellationToken.None);

        using (new AssertionScope())
        {
            result.GovUkIdentifier.Should().BeEquivalentTo(candidate.GovUkIdentifier);
            result.Email.Should().BeEquivalentTo(candidate.Email);
            result.FirstName.Should().BeEquivalentTo(candidate.FirstName);
            result.LastName.Should().BeEquivalentTo(candidate.LastName);
            result.Id.Should().Be(candidate.Id);
            result.Status.Should().Be(candidate.Status);
        }
    }
    [Test, MoqAutoData]
    public async Task Then_If_Candidate_Already_Exists_And_Email_Is_Different_Then_Updated_And_Details_Are_Returned(
        CreateCandidateCommand command,
        string govUkId,
        GetCandidateApiResponse candidate,
        PostCandidateApiResponse response,
        GetLegacyUserByEmailApiResponse legacyUserByEmailApiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        [Frozen] Mock<IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration>> mockLegacyApiClient,
        CreateCandidateCommandHandler handler)
    {
        command.GovUkIdentifier = govUkId;

        var expectedGetCandidateRequest = new GetCandidateApiRequest(govUkId);
        mockApiClient.Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(
                It.Is<GetCandidateApiRequest>(r => r.GetUrl == expectedGetCandidateRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(candidate, HttpStatusCode.OK, string.Empty));
        mockApiClient.Setup(x => x.PutWithResponseCode<PutCandidateApiResponse>(
                It.Is<PutCandidateApiRequest>(r => r.PutUrl.Contains(candidate.Id.ToString()))))
            .ReturnsAsync(new ApiResponse<PutCandidateApiResponse>(null, HttpStatusCode.OK, string.Empty));

        var result = await handler.Handle(command, CancellationToken.None);

        using (new AssertionScope())
        {
            result.GovUkIdentifier.Should().BeEquivalentTo(candidate.GovUkIdentifier);
            result.Email.Should().BeEquivalentTo(candidate.Email);
            result.FirstName.Should().BeEquivalentTo(candidate.FirstName);
            result.LastName.Should().BeEquivalentTo(candidate.LastName);
            result.Id.Should().Be(candidate.Id);
            result.Status.Should().Be(candidate.Status);
        }
        mockApiClient.Verify(x => x.PutWithResponseCode<PutCandidateApiResponse>(
                It.Is<PutCandidateApiRequest>(r => r.PutUrl.Contains(candidate.Id.ToString()) && ((PutCandidateApiRequestData)r.Data).Email == command.Email)), Times.Once());
    }

    [Test, MoqAutoData]
    public async Task Then_The_Post_Is_Sent_And_Data_Returned(
        CreateCandidateCommand command,
        string govUkId,
        PostCandidateApiResponse response,
        GetLegacyUserByEmailApiResponse legacyUserByEmailApiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        [Frozen] Mock<IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration>> mockLegacyApiClient,
        CreateCandidateCommandHandler handler)
    {
        command.GovUkIdentifier = govUkId;
        response.FirstName = legacyUserByEmailApiResponse.RegistrationDetails?.FirstName;
        response.LastName = legacyUserByEmailApiResponse.RegistrationDetails?.LastName;

        var expectedPostData = new PostCandidateApiRequestData
        {
            Email = command.Email
        };

        var expectedGetCandidateRequest = new GetCandidateApiRequest(govUkId);
        mockApiClient.Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(
                It.Is<GetCandidateApiRequest>(r => r.GetUrl == expectedGetCandidateRequest.GetUrl)))
                .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(null, HttpStatusCode.NotFound, string.Empty));

        var expectedRequest = new PostCandidateApiRequest(command.GovUkIdentifier, expectedPostData);

        mockApiClient
                .Setup(client => client.PostWithResponseCode<PostCandidateApiResponse>(
                    It.Is<PostCandidateApiRequest>(r => r.PostUrl == expectedRequest.PostUrl), true))
                .ReturnsAsync(new ApiResponse<PostCandidateApiResponse>(response, HttpStatusCode.OK, string.Empty));

        var legacyGetRequest = new GetLegacyUserByEmailApiRequest(command.Email);
        mockLegacyApiClient
            .Setup(client => client.Get<GetLegacyUserByEmailApiResponse>(
                It.Is<GetLegacyUserByEmailApiRequest>(r => r.GetUrl == legacyGetRequest.GetUrl)))
            .ReturnsAsync(legacyUserByEmailApiResponse);

        var result = await handler.Handle(command, CancellationToken.None);

        using (new AssertionScope())
        {
            result.GovUkIdentifier.Should().BeEquivalentTo(response.GovUkIdentifier);
            result.Email.Should().BeEquivalentTo(response.Email);
            result.FirstName.Should().BeEquivalentTo(legacyUserByEmailApiResponse.RegistrationDetails?.FirstName);
            result.LastName.Should().BeEquivalentTo(legacyUserByEmailApiResponse.RegistrationDetails?.LastName);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_The_Post_Is_Sent_And_If_Legacy_Api_Has_DateOfBirth_As_DateTime_Min_Value_Then_Null_Sent(
        CreateCandidateCommand command,
        string govUkId,
        PostCandidateApiResponse response,
        GetLegacyUserByEmailApiResponse legacyUserByEmailApiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        [Frozen] Mock<IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration>> mockLegacyApiClient,
        CreateCandidateCommandHandler handler)
    {
        command.GovUkIdentifier = govUkId;

        legacyUserByEmailApiResponse.RegistrationDetails.DateOfBirth = DateTime.MinValue;
        response.FirstName = legacyUserByEmailApiResponse.RegistrationDetails?.FirstName;
        response.LastName = legacyUserByEmailApiResponse.RegistrationDetails?.LastName;

        var expectedPostData = new PostCandidateApiRequestData
        {
            Email = command.Email
        };

        var expectedGetCandidateRequest = new GetCandidateApiRequest(govUkId);
        mockApiClient.Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(
                It.Is<GetCandidateApiRequest>(r => r.GetUrl == expectedGetCandidateRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(null, HttpStatusCode.NotFound, string.Empty));

        var expectedRequest = new PostCandidateApiRequest(command.GovUkIdentifier, expectedPostData);

        mockApiClient
                .Setup(client => client.PostWithResponseCode<PostCandidateApiResponse>(
                    It.Is<PostCandidateApiRequest>(r => 
                        r.PostUrl == expectedRequest.PostUrl
                        && ((PostCandidateApiRequestData)r.Data).DateOfBirth == null), true))
                .ReturnsAsync(new ApiResponse<PostCandidateApiResponse>(response, HttpStatusCode.OK, string.Empty));

        var legacyGetRequest = new GetLegacyUserByEmailApiRequest(command.Email);
        mockLegacyApiClient
            .Setup(client => client.Get<GetLegacyUserByEmailApiResponse>(
                It.Is<GetLegacyUserByEmailApiRequest>(r => r.GetUrl == legacyGetRequest.GetUrl)))
            .ReturnsAsync(legacyUserByEmailApiResponse);

        var result = await handler.Handle(command, CancellationToken.None);

        using (new AssertionScope())
        {
            result.GovUkIdentifier.Should().BeEquivalentTo(response.GovUkIdentifier);
            result.Email.Should().BeEquivalentTo(response.Email);
            result.FirstName.Should().BeEquivalentTo(legacyUserByEmailApiResponse.RegistrationDetails?.FirstName);
            result.LastName.Should().BeEquivalentTo(legacyUserByEmailApiResponse.RegistrationDetails?.LastName);
        }
    }

    [Test, MoqAutoData]
    public async Task And_Api_Returns_Null_Then_Return_Null(
        CreateCandidateCommand command,
        string govUkId,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        CreateCandidateCommandHandler handler)
    {
        command.GovUkIdentifier = govUkId;

        var expectedPostData = new PostCandidateApiRequestData
        {
            Email = command.Email
        };

        var expectedGetCandidateRequest = new GetCandidateApiRequest(govUkId);
        mockApiClient.Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(
                It.Is<GetCandidateApiRequest>(r => r.GetUrl == expectedGetCandidateRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(null, HttpStatusCode.NotFound, string.Empty));

        var expectedRequest = new PostCandidateApiRequest(command.GovUkIdentifier, expectedPostData);

        mockApiClient
                .Setup(client => client.PostWithResponseCode<PostCandidateApiResponse>(
                    It.Is<PostCandidateApiRequest>(r => r.PostUrl == expectedRequest.PostUrl), true))
                .ReturnsAsync(() => null);

        Func<Task> result = () => handler.Handle(command, CancellationToken.None);

        await result.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Test, MoqAutoData]
    public async Task Then_LegacyApi_Returns_Null_The_Post_Is_Sent_And_Data_Returned(
        CreateCandidateCommand command,
        string govUkId,
        PostCandidateApiResponse response,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        [Frozen] Mock<IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration>> mockLegacyApiClient,
        CreateCandidateCommandHandler handler)
    {
        command.GovUkIdentifier = govUkId;

        response.FirstName = null;
        response.LastName = null;
        var expectedPostData = new PostCandidateApiRequestData
        {
            Email = command.Email
        };

        var expectedGetCandidateRequest = new GetCandidateApiRequest(govUkId);
        mockApiClient.Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(
                It.Is<GetCandidateApiRequest>(r => r.GetUrl == expectedGetCandidateRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(null, HttpStatusCode.NotFound, string.Empty));

        var expectedRequest = new PostCandidateApiRequest(command.GovUkIdentifier, expectedPostData);

        mockApiClient
            .Setup(client => client.PostWithResponseCode<PostCandidateApiResponse>(
                It.Is<PostCandidateApiRequest>(r => r.PostUrl == expectedRequest.PostUrl), true))
            .ReturnsAsync(new ApiResponse<PostCandidateApiResponse>(response, HttpStatusCode.OK, string.Empty));

        var legacyGetRequest = new GetLegacyUserByEmailApiRequest(command.Email);
        mockLegacyApiClient
            .Setup(client => client.Get<GetLegacyUserByEmailApiResponse>(
                It.Is<GetLegacyUserByEmailApiRequest>(r => r.GetUrl == legacyGetRequest.GetUrl)))
            .ReturnsAsync(() => null);

        var result = await handler.Handle(command, CancellationToken.None);

        using (new AssertionScope())
        {
            result.GovUkIdentifier.Should().BeEquivalentTo(response.GovUkIdentifier);
            result.Email.Should().BeEquivalentTo(response.Email);
            result.FirstName.Should().BeNull();
            result.LastName.Should().BeNull();
        }
    }

    [Test, MoqAutoData]
    public async Task Then_Legacy_Applications_Are_Migrated(
        CreateCandidateCommand command,
        string govUkId,
        PostCandidateApiResponse response,
        GetLegacyUserByEmailApiResponse legacyUserByEmailApiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        [Frozen] Mock<IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration>> mockLegacyApiClient,
        [Frozen] Mock<ILegacyApplicationMigrationService> legacyApplicationMigrationService,
        CreateCandidateCommandHandler handler)
    {
        command.GovUkIdentifier = govUkId;
        response.FirstName = legacyUserByEmailApiResponse.RegistrationDetails?.FirstName;
        response.LastName = legacyUserByEmailApiResponse.RegistrationDetails?.LastName;

        var expectedPostData = new PostCandidateApiRequestData
        {
            Email = command.Email
        };

        var expectedGetCandidateRequest = new GetCandidateApiRequest(govUkId);
        mockApiClient.Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(
                It.Is<GetCandidateApiRequest>(r => r.GetUrl == expectedGetCandidateRequest.GetUrl)))
                .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(null, HttpStatusCode.NotFound, string.Empty));

        var expectedRequest = new PostCandidateApiRequest(command.GovUkIdentifier, expectedPostData);

        mockApiClient
                .Setup(client => client.PostWithResponseCode<PostCandidateApiResponse>(
                    It.Is<PostCandidateApiRequest>(r => r.PostUrl == expectedRequest.PostUrl), true))
                .ReturnsAsync(new ApiResponse<PostCandidateApiResponse>(response, HttpStatusCode.OK, string.Empty));

        var legacyGetRequest = new GetLegacyUserByEmailApiRequest(command.Email);
        mockLegacyApiClient
            .Setup(client => client.Get<GetLegacyUserByEmailApiResponse>(
                It.Is<GetLegacyUserByEmailApiRequest>(r => r.GetUrl == legacyGetRequest.GetUrl)))
            .ReturnsAsync(legacyUserByEmailApiResponse);

        await handler.Handle(command, CancellationToken.None);

        legacyApplicationMigrationService.Verify(
            x => x.MigrateLegacyApplications(response.Id, command.Email), Times.Once);

    }
}
