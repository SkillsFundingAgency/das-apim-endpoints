using SFA.DAS.Recruit.Application.Candidates.Queries.GetApplicationsById;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.UnitTests.Application.Candidates.Queries
{
    [TestFixture]
    public class WhenHandlingGetApplicationsByIdQuery
    {
        [Test, MoqAutoData]
        public async Task Handle_ValidRequest_ReturnsExpectedResult(
            GetApplicationsByIdQuery query,
            GetAllApplicationsByIdApiResponse response,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            [Greedy] GetApplicationsByIdQueryHandler handler)
        {
            // Arrange
            var expectedPostUrl = new GetAllApplicationsByIdApiRequest(new GetAllApplicationsByIdApiRequestData
            {
                ApplicationIds = query.ApplicationIds,
                IncludeDetails = query.IncludeDetails
            });

            var apiResponse =
                new ApiResponse<GetAllApplicationsByIdApiResponse>(response, HttpStatusCode.OK, string.Empty);

            candidateApiClient.Setup(x => x.PostWithResponseCode<GetAllApplicationsByIdApiResponse>(
                    It.Is<GetAllApplicationsByIdApiRequest>(r =>
                        r.PostUrl == expectedPostUrl.PostUrl && r.Payload == expectedPostUrl.Payload),
                    true))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            
            // Assert
            result.Should().BeEquivalentTo(response);
        }
    }
}
