using SFA.DAS.FindAnApprenticeship.Application.Commands.Candidate;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands;
public class WhenHandlingPostCandidateCommand
{
    [Test]
    [MoqInlineAutoData(UserStatus.Completed)]
    [MoqInlineAutoData(UserStatus.Deleted)]
    [MoqInlineAutoData(UserStatus.InProgress)]
    [MoqInlineAutoData(UserStatus.Incomplete)]
    public async Task Then_If_Candidate_Already_Exists_Then_Details_Are_Returned(
        UserStatus status,
        CreateCandidateCommand command,
        string govUkId,
        GetCandidateApiResponse candidate,
        PostCandidateApiResponse response,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        CreateCandidateCommandHandler handler)
    {
        command.GovUkIdentifier = govUkId;
        command.Email = candidate.Email;

        candidate.Status = status;

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
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
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
            result.Email.Should().BeEquivalentTo(command.Email);
            result.FirstName.Should().BeEquivalentTo(candidate.FirstName);
            result.LastName.Should().BeEquivalentTo(candidate.LastName);
            result.Id.Should().Be(candidate.Id);
            result.Status.Should().Be(candidate.Status);
            result.IsEmailAddressMigrated.Should().BeFalse();
        }
        mockApiClient.Verify(x => x.PutWithResponseCode<PutCandidateApiResponse>(
                It.Is<PutCandidateApiRequest>(r => r.PutUrl.Contains(candidate.Id.ToString()) && ((PutCandidateApiRequestData)r.Data).Email == command.Email)), Times.Once());
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
    public async Task Then_Candidate_Not_Exist_The_Post_Is_Sent_And_Data_Returned(
        CreateCandidateCommand command,
        string govUkId,
        PostCandidateApiResponse response,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        CreateCandidateCommandHandler handler)
    {
        command.GovUkIdentifier = govUkId;

        response.FirstName = null;
        response.LastName = null;
        response.PhoneNumber = null;
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

        var result = await handler.Handle(command, CancellationToken.None);

        using (new AssertionScope())
        {
            result.GovUkIdentifier.Should().BeEquivalentTo(response.GovUkIdentifier);
            result.Email.Should().BeEquivalentTo(response.Email);
            result.FirstName.Should().BeNull();
            result.LastName.Should().BeNull(); 
            result.PhoneNumber.Should().BeNull();
            result.DateOfBirth.Should().BeNull();
            result.Status.Should().Be(UserStatus.Incomplete);
        }
    }
}
