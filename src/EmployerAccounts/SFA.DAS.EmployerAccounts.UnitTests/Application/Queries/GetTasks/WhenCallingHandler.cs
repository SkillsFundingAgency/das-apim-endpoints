using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.GetTasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.GetTasks
{
    [TestFixture]
    public class WhenCallingHandler
    {
        private GetTasksQueryHandler _handler;
        private GetTasksQuery _request;
        private GetApplicationsResponse _ltmApplicationsResponse;
        private Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>> _ltmApiClient;
        private Mock<ILogger<GetTasksQueryHandler>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<GetTasksQueryHandler>>();

            var fixture = new Fixture();
            _request = fixture.Create<GetTasksQuery>();

            _ltmApplicationsResponse = fixture.Create<GetApplicationsResponse>();

            _ltmApiClient = new Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>>();

            _ltmApiClient.Setup(x =>
                    x.Get<GetApplicationsResponse>(It.IsAny<GetApplicationsRequest>()))
                .ReturnsAsync(_ltmApplicationsResponse);

            _handler = new GetTasksQueryHandler(_loggerMock.Object, _ltmApiClient.Object);
        }

        [Test]
        public async Task Then_Gets_Tasks_Returns_GetTasksQueryResult()
        {
            var result = await _handler.Handle(_request, CancellationToken.None);

            result.Should().BeEquivalentTo(new GetTasksQueryResult
            {
                NumberTransferPledgeApplicationsToReview = _ltmApplicationsResponse.TotalItems
            });
        }
    }
}