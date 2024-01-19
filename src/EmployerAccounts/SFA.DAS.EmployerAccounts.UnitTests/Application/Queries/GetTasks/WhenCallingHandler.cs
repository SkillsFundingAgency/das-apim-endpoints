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
        private GetApprenticeshipUpdatesResponse _pendingApprenticeshipChanges;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _comtApiClient;
        private Mock<ILogger<GetTasksQueryHandler>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<GetTasksQueryHandler>>();

            var fixture = new Fixture();
            _request = fixture.Create<GetTasksQuery>();

            _pendingApprenticeshipChanges = fixture.Create<GetApprenticeshipUpdatesResponse>();

            _comtApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _comtApiClient.Setup(x =>
                    x.Get<GetApprenticeshipUpdatesResponse>(It.IsAny<GetPendingApprenticeChangesRequest>()))
                .ReturnsAsync(_pendingApprenticeshipChanges);

            _handler = new GetTasksQueryHandler(_loggerMock.Object, _comtApiClient.Object);
        }

        [Test]
        public async Task Then_Gets_Tasks_Returns_GetTasksQueryResult()
        {
            var result = await _handler.Handle(_request, CancellationToken.None);

            result.Should().BeEquivalentTo(new GetTasksQueryResult
            {
                NumberOfApprenticesToReview = _pendingApprenticeshipChanges.ApprenticeshipUpdates.Count
            });
        }
    }
}