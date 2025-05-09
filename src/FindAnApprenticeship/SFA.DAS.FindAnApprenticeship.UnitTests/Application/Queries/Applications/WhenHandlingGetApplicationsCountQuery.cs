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
            GetApplicationsCountApiResponse applicationsCountApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetApplicationsCountQueryHandler handler)
        {
            var expectedRequest = new GetApplicationsCountApiRequest(query.CandidateId, query.Status);

            candidateApiClient
                .Setup(client => client.GetWithResponseCode<GetApplicationsCountApiResponse>(
                    It.Is<GetApplicationsCountApiRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(new ApiResponse<GetApplicationsCountApiResponse>(applicationsCountApiResponse, HttpStatusCode.OK, string.Empty));
            
            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();
            result.Should().BeEquivalentTo(applicationsCountApiResponse);
        }
    }
}
