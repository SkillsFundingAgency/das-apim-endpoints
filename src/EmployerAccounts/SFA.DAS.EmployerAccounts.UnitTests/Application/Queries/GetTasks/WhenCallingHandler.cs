using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.GetTasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.GetTasks
{
    [TestFixture]
    public class WhenCallingHandler
    {
        private GetTasksQueryHandler _handler;
        private GetTasksQuery _request;
        private GetCohortsResponse _cohortsForReviewResponse;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _comtApiClient;
        private Mock<ILogger<GetTasksQueryHandler>> _loggerMock;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<GetTasksQueryHandler>>();

            _fixture = new Fixture();
            _request = _fixture.Create<GetTasksQuery>();

            _cohortsForReviewResponse = _fixture.Build<GetCohortsResponse>()
            .With(x => x.Cohorts, _fixture.Build<GetCohortsResponse.CohortSummary>()
            .With(summary => summary.IsDraft, false)
            .With(summary => summary.WithParty, Party.Employer)
            .CreateMany()
            .ToArray())
            .Create();

            _comtApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _comtApiClient.Setup(x =>
                    x.Get<GetCohortsResponse>(It.IsAny<GetCohortsRequest>()))
                .ReturnsAsync(_cohortsForReviewResponse);

            _handler = new GetTasksQueryHandler(_loggerMock.Object, _comtApiClient.Object);
        }

        [Test]
        public async Task Then_Gets_Tasks_Returns_GetTasksQueryResult()
        {
            var result = await _handler.Handle(_request, CancellationToken.None);

            result.Should().BeEquivalentTo(new GetTasksQueryResult
            {
                NumberOfCohortsReadyToReview = _cohortsForReviewResponse.Cohorts.Count(x => !x.IsDraft && x.WithParty == Party.Employer)
            });
        }
    }
}