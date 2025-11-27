using NUnit.Framework;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerFeedback.Api.TaskQueue;

namespace SFA.DAS.EmployerFeedback.Api.UnitTests.TaskQueue
{
    [TestFixture]
    public class TaskQueueHostedServiceTests
    {
        private TaskQueueHostedService _service;
        private Mock<IBackgroundTaskQueue> _backgroundTaskQueueMock;
        private Mock<ILogger<TaskQueueHostedService>> _loggerMock;
        private Mock<IServiceProvider> _serviceProviderMock;
        private Mock<IServiceScopeFactory> _serviceScopeFactoryMock;
        private Mock<IServiceScope> _serviceScopeMock;
        private Mock<IMediator> _mediatorMock;

        [SetUp]
        public void SetUp()
        {
            _backgroundTaskQueueMock = new Mock<IBackgroundTaskQueue>();
            _loggerMock = new Mock<ILogger<TaskQueueHostedService>>();
            _serviceProviderMock = new Mock<IServiceProvider>();
            _serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            _serviceScopeMock = new Mock<IServiceScope>();
            _mediatorMock = new Mock<IMediator>();

            _serviceProviderMock
                .Setup(x => x.GetService(typeof(IServiceScopeFactory)))
                .Returns(_serviceScopeFactoryMock.Object);

            _serviceScopeFactoryMock
                .Setup(x => x.CreateScope())
                .Returns(_serviceScopeMock.Object);

            _serviceScopeMock
                .Setup(x => x.ServiceProvider)
                .Returns(_serviceProviderMock.Object);

            _serviceProviderMock
                .Setup(x => x.GetService(typeof(IMediator)))
                .Returns(_mediatorMock.Object);

            _serviceProviderMock
                .Setup(x => x.GetService(typeof(ILogger<TaskQueueHostedService>)))
                .Returns(_loggerMock.Object);

            _service = new TaskQueueHostedService(
                _backgroundTaskQueueMock.Object,
                _loggerMock.Object,
                _serviceProviderMock.Object);
        }

        [Test]
        public async Task ExecuteAsync_DequeuesAndExecutesTasks()
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            var mockRequest = new Mock<IBaseRequest>().Object;

            _backgroundTaskQueueMock
                .Setup(x => x.DequeueAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync((mockRequest, "TestRequest", (response, duration, logger) => { }
            ))
                .Callback(() => cancellationTokenSource.Cancel());

            await _service.StartAsync(cancellationTokenSource.Token);

            _mediatorMock.Verify(x => x.Send(It.IsAny<IBaseRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            _serviceScopeFactoryMock.Verify(x => x.CreateScope(), Times.Once);
        }

        [Test]
        public async Task ExecuteAsync_HandlesExceptionsGracefully()
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            _backgroundTaskQueueMock
                .Setup(x => x.DequeueAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync((null, "FailingRequest", (response, duration, logger) => throw new Exception("Simulated failure")))
                .Callback(() => cancellationTokenSource.Cancel()); // Simulate a task that fails

            Func<Task> act = async () => await _service.StartAsync(cancellationTokenSource.Token);

            await act.Should().NotThrowAsync();
        }
    }
}
