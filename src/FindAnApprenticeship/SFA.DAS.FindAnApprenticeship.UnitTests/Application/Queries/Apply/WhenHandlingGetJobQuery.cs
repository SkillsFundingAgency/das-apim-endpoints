using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetJob;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply
{
    [TestFixture]
    public class WhenHandlingGetJobQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(
            GetJobQuery query,
            GetWorkHistoryItemApiResponse workHistoriesApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetJobQueryHandler handler)
        {
            var expectedGetWorkHistoriesRequest = new GetWorkHistoryItemApiRequest(query.ApplicationId, query.CandidateId, query.JobId, WorkHistoryType.Job);
            candidateApiClient
                .Setup(client => client.Get<GetWorkHistoryItemApiResponse>(
                    It.Is<GetWorkHistoryItemApiRequest>(r => r.GetUrl == expectedGetWorkHistoriesRequest.GetUrl)))
                .ReturnsAsync(workHistoriesApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();
            result.Should().BeEquivalentTo((GetJobQueryResult)workHistoriesApiResponse);
        }
    }
}
