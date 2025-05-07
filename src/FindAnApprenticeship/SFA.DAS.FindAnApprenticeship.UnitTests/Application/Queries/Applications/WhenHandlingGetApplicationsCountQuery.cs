using SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplicationsCount;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Applications
{
    [TestFixture]
    public class WhenHandlingGetApplicationsCountQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(
            GetApplicationsCountQuery query,
            PostApplicationsCountApiResponse applicationsCountApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetApplicationsCountQueryHandler handler)
        {
            var expectedPutData = new PostApplicationsCountApiRequest.PostApplicationsCountApiRequestData(query.Statuses);

            var expectedRequest = new PostApplicationsCountApiRequest(query.CandidateId, expectedPutData);

            candidateApiClient
                .Setup(client => client.PostWithResponseCode<PostApplicationsCountApiResponse>(
                    It.Is<PostApplicationsCountApiRequest>(r => r.PostUrl == expectedRequest.PostUrl
                    && r.Payload.Statuses == query.Statuses), true))
                .ReturnsAsync(new ApiResponse<PostApplicationsCountApiResponse>(applicationsCountApiResponse, HttpStatusCode.OK, string.Empty));
            
            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();
            result.Stats.Should().BeEquivalentTo(applicationsCountApiResponse.Stats);
        }
    }
}
