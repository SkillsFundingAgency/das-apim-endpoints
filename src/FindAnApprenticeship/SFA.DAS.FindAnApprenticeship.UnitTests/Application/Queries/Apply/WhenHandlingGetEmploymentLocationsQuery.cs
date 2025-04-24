using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetEmploymentLocations;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply
{
    [TestFixture]
    public class WhenHandlingGetEmploymentLocationsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(
            GetEmploymentLocationsQuery query,
        GetEmploymentLocationApiResponse apiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetEmploymentLocationsQueryHandler handler)
        {
            var expectedRequest = new GetEmploymentLocationsApiRequest(query.CandidateId, query.ApplicationId);

            candidateApiClient
                .Setup(client => client.Get<GetEmploymentLocationApiResponse>(
                    It.Is<GetEmploymentLocationsApiRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().BeOfType<GetEmploymentLocationsQueryResult>();
                candidateApiClient.Verify(p => p.Get<GetEmploymentLocationApiResponse>(It.Is<GetEmploymentLocationsApiRequest>(x => x.GetUrl == expectedRequest.GetUrl)), Times.Once);
                result.EmploymentLocation.Should().NotBeNull();
            }
        }
    }
}
