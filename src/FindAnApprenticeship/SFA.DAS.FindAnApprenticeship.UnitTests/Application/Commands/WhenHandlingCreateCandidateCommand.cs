using System.Net;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Candidate;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

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
        // arrange
        command.GovUkIdentifier = govUkId;

        var expectedPostData = new PostCandidateApiRequestData
        {
            Email = command.Email
        };

        var expectedGetCandidateRequest = new GetCandidateApiRequest(govUkId);
        mockApiClient.Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(
                It.Is<GetCandidateApiRequest>(r => r.GetUrl == expectedGetCandidateRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(null, HttpStatusCode.NotFound, string.Empty));
        
        var expectedGetCandidateByEmailAddressRequest = new GetCandidateByEmailAddressApiRequest(command.Email);
        mockApiClient.Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(
                It.Is<GetCandidateByEmailAddressApiRequest>(r => r.GetUrl == expectedGetCandidateByEmailAddressRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(null!, HttpStatusCode.NotFound, string.Empty));

        var expectedRequest = new PostCandidateApiRequest(command.GovUkIdentifier, expectedPostData);

        mockApiClient
                .Setup(client => client.PostWithResponseCode<PostCandidateApiResponse>(
                    It.Is<PostCandidateApiRequest>(r => r.PostUrl == expectedRequest.PostUrl), true))
                .ReturnsAsync(() => null);

        Func<Task> result = () => handler.Handle(command, CancellationToken.None);

        // act/assert
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
        // arrange
        command.GovUkIdentifier = govUkId;

        response.FirstName = null;
        response.LastName = null;
        response.PhoneNumber = null;
        var expectedPostData = new PostCandidateApiRequestData
        {
            Email = command.Email
        };

        var expectedGetCandidateRequest = new GetCandidateApiRequest(govUkId);
        var expectedGetCandidateByEmailAddressRequest = new GetCandidateByEmailAddressApiRequest(command.Email);
        mockApiClient.Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(
                It.Is<GetCandidateApiRequest>(r => r.GetUrl == expectedGetCandidateRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(null!, HttpStatusCode.NotFound, string.Empty));
        
        mockApiClient.Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(
                It.Is<GetCandidateByEmailAddressApiRequest>(r => r.GetUrl == expectedGetCandidateByEmailAddressRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(null!, HttpStatusCode.NotFound, string.Empty));

        var expectedRequest = new PostCandidateApiRequest(command.GovUkIdentifier, expectedPostData);

        mockApiClient
            .Setup(client => client.PostWithResponseCode<PostCandidateApiResponse>(
                It.Is<PostCandidateApiRequest>(r => r.PostUrl == expectedRequest.PostUrl), true))
            .ReturnsAsync(new ApiResponse<PostCandidateApiResponse>(response, HttpStatusCode.OK, string.Empty));

        // act
        var result = await handler.Handle(command, CancellationToken.None);

        // assert
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
    
    [Test, MoqAutoData]
    public async Task Then_If_The_Candidate_Is_Not_Found_By_Their_Gov_Id_Then_The_Candidate_Is_Looked_Up_By_Email_Address_And_The_Gov_Id_Updated_When_Null(
        CreateCandidateCommand command,
        GetCandidateApiResponse candidate,
        PostCandidateApiResponse response,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        CreateCandidateCommandHandler handler)
    {
        // arrange
        candidate.Email = command.Email;
        candidate.Status = UserStatus.Completed;
        candidate.GovUkIdentifier = null;

        mockApiClient
            .Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(It.IsAny<GetCandidateApiRequest>()))
            .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(null!, HttpStatusCode.NotFound, string.Empty));
        
        mockApiClient
            .Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(It.IsAny<GetCandidateByEmailAddressApiRequest>()))
            .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(candidate, HttpStatusCode.OK, string.Empty));

        mockApiClient
            .Setup(client => client.PostWithResponseCode<PostCandidateApiResponse>(It.IsAny<PostCandidateApiRequest>(), true))
            .ReturnsAsync(new ApiResponse<PostCandidateApiResponse>(response, HttpStatusCode.OK, string.Empty));
        
        PatchCandidateApiRequest? capturedPatchRequest = null;
        mockApiClient
            .Setup(client => client.PatchWithResponseCode(It.IsAny<IPatchApiRequest<JsonPatchDocument<PatchableCandidate>>>()))
            .Callback<IPatchApiRequest<JsonPatchDocument<PatchableCandidate>>>(x => capturedPatchRequest = x as PatchCandidateApiRequest)
            .ReturnsAsync(new ApiResponse<string>(string.Empty, HttpStatusCode.OK, string.Empty));

        // act
        var result = await handler.Handle(command, CancellationToken.None);

        // assert
        result.GovUkIdentifier.Should().BeEquivalentTo(command.GovUkIdentifier);
        
        capturedPatchRequest.Should().NotBeNull();
        capturedPatchRequest.Data.Operations.Should().HaveCount(2);
        capturedPatchRequest.Data.Operations[0].op.Should().Be("replace");
        capturedPatchRequest.Data.Operations[0].path.Should().Be("/GovUkIdentifier");
        capturedPatchRequest.Data.Operations[0].value.Should().Be(command.GovUkIdentifier);
        capturedPatchRequest.Data.Operations[1].op.Should().Be("replace");
        capturedPatchRequest.Data.Operations[1].path.Should().Be("/UpdatedOn");
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_The_Candidate_Is_Not_Found_By_Their_Gov_Id_Then_The_Candidate_Is_Looked_Up_By_Email_Address_And_The_Gov_Id_Update_Will_Fail_When_It_Is_Not_Null(
        CreateCandidateCommand command,
        GetCandidateApiResponse candidate,
        PostCandidateApiResponse response,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        CreateCandidateCommandHandler handler)
    {
        // arrange
        candidate.Email = command.Email;
        candidate.Status = UserStatus.Completed;

        mockApiClient
            .Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(It.IsAny<GetCandidateApiRequest>()))
            .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(null!, HttpStatusCode.NotFound, string.Empty));
        
        mockApiClient
            .Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(It.IsAny<GetCandidateByEmailAddressApiRequest>()))
            .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(candidate, HttpStatusCode.OK, string.Empty));

        var action = async () => await handler.Handle(command, CancellationToken.None);

        // act/assert
        await action.Should().ThrowAsync<NotSupportedException>();
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_The_Candidate_Matches_Multiple_Accounts_By_Their_Email_Address_Then_An_Exception_Is_Raised(
        CreateCandidateCommand command,
        GetCandidateApiResponse candidate,
        PostCandidateApiResponse response,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        CreateCandidateCommandHandler handler)
    {
        // arrange
        candidate.Email = command.Email;
        candidate.Status = UserStatus.Completed;

        mockApiClient
            .Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(It.IsAny<GetCandidateApiRequest>()))
            .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(null!, HttpStatusCode.NotFound, string.Empty));
        
        mockApiClient
            .Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(It.IsAny<GetCandidateByEmailAddressApiRequest>()))
            .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(candidate, HttpStatusCode.BadRequest, string.Empty));

        var action = async () => await handler.Handle(command, CancellationToken.None);

        // act/assert
        await action.Should().ThrowAsync<NotSupportedException>();
    }
}
