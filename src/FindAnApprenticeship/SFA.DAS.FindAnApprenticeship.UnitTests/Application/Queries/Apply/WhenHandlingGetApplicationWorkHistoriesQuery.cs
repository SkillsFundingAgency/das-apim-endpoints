using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WorkHistory;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply
{
    [TestFixture]
    public class WhenHandlingGetApplicationWorkHistoriesQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(
            GetJobsQuery query,
            List<GetWorkHistoriesApiResponse> workHistoriesApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetJobsQueryHandler handler)
        {
            var expectedGetWorkHistoriesRequest = new GetWorkHistoriesApiRequest(query.CandidateId, query.ApplicationId);
            candidateApiClient
                .Setup(client => client.Get<List<GetWorkHistoriesApiResponse>>(
                    It.Is<GetWorkHistoriesApiRequest>(r => r.GetUrl == expectedGetWorkHistoriesRequest.GetUrl)))
                .ReturnsAsync(workHistoriesApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();
            result?.Jobs.Should().BeEquivalentTo(workHistoriesApiResponse);
        }
    }
}
