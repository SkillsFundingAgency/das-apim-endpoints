using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetEmploymentLocations;
using SFA.DAS.FindAnApprenticeship.Domain;
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
            bool status,
            GetApplicationApiResponse applicationApiResponse,
            GetEmploymentLocationsQuery query,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetEmploymentLocationsQueryHandler handler)
        {
            applicationApiResponse.EmploymentLocationStatus = status 
                ? Constants.SectionStatus.Completed 
                : Constants.SectionStatus.InProgress;

            var expectedGetApplicationApiRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId, false);

            candidateApiClient.Setup(x => x.Get<GetApplicationApiResponse>(It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationApiRequest.GetUrl)))
                .ReturnsAsync(applicationApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().BeOfType<GetEmploymentLocationsQueryResult>();
                result.EmploymentLocation.Should().NotBeNull();
                result.EmploymentLocation.Should().BeEquivalentTo(applicationApiResponse.EmploymentLocation);
                result.IsSectionCompleted.Should().Be(status);
            }
        }
    }
}
