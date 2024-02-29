using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WorkHistory;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Models;
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
            GetWorkHistoriesApiResponse workHistoriesApiResponse,
            GetApplicationApiResponse applicationApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetJobsQueryHandler handler)
        {
            var expectedGetWorkHistoriesRequest = new GetWorkHistoriesApiRequest(query.ApplicationId, query.CandidateId, WorkHistoryType.Job);
            candidateApiClient
                .Setup(client => client.Get<GetWorkHistoriesApiResponse>(
                    It.Is<GetWorkHistoriesApiRequest>(r => r.GetUrl == expectedGetWorkHistoriesRequest.GetUrl)))
                .ReturnsAsync(workHistoriesApiResponse);

            var expectedApplicationRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId);
            applicationApiResponse.JobsStatus = Constants.SectionStatus.InProgress;
            candidateApiClient.Setup(client =>
                client.Get<GetApplicationApiResponse>(It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedApplicationRequest.GetUrl)))
                .ReturnsAsync(applicationApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();

            result.Should().BeEquivalentTo(new GetJobsQueryResult
            {
                IsSectionCompleted = false,
                Jobs = workHistoriesApiResponse.WorkHistories.Select(x => new GetJobsQueryResult.Job
                {
                    Id = x.Id,
                    ApplicationId = x.ApplicationId,
                    Description = x.Description,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Employer = x.Employer,
                    JobTitle = x.JobTitle
                }).ToList()
            });
        }
    }
}
