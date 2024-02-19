using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetJob;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WorkHistory.DeleteJob;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply
{
    [TestFixture]
    public class WhenHandlingGetDeleteJobQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(
            GetDeleteJobQuery query,
            GetDeleteJobApiResponse deleteJobApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetDeleteJobQueryHandler handler)
        {
            var expectedGetDeleteJobRequest = new GetDeleteJobApiRequest(query.ApplicationId, query.CandidateId, query.JobId, WorkHistoryType.Job);
            candidateApiClient
                .Setup(client => client.Get<GetDeleteJobApiResponse>(
                    It.Is<GetDeleteJobApiRequest>(r => r.GetUrl == expectedGetDeleteJobRequest.GetUrl)))
                .ReturnsAsync(deleteJobApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();
            result.Should().BeEquivalentTo((GetDeleteJobQueryResult)deleteJobApiResponse);
        }
    }
}
