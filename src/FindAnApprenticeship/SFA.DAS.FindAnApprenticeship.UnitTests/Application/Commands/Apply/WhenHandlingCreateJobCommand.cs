using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateJob;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply
{
    [TestFixture]
    public class WhenHandlingCreateJobCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(
            CreateJobCommand command,
            PostWorkHistoryApiResponse apiResponse,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> faaApiClient,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            CreateJobCommandHandler handler)
        {
            var expectedRequest = new PostWorkHistoryApiRequest(command.ApplicationId, command.CandidateId, new PostWorkHistoryApiRequest.PostWorkHistoryApiRequestData());
            candidateApiClient
                .Setup(client => client.PostWithResponseCode<PostWorkHistoryApiResponse>(
                    It.Is<PostWorkHistoryApiRequest>(r => r.PostUrl == expectedRequest.PostUrl), true))
                .ReturnsAsync(new ApiResponse<PostWorkHistoryApiResponse>(apiResponse, HttpStatusCode.Created, string.Empty));
         
            var result = await handler.Handle(command, CancellationToken.None);

            result.Id.Should().Be(apiResponse.Id);
        }
    }
}
