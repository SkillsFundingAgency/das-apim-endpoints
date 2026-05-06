using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Threading;
using System;
using FluentAssertions;
using SFA.DAS.EmployerFeedback.Api.TaskQueue;

namespace SFA.DAS.EmployerFeedback.Api.UnitTests.TaskQueue
{
    public class BackgroundTaskQueueTests
    {
        private readonly BackgroundTaskQueue _queue;
        private readonly Mock<ILogger<TaskQueueHostedService>> _loggerMock;

        public BackgroundTaskQueueTests()
        {
            _queue = new BackgroundTaskQueue();
            _loggerMock = new Mock<ILogger<TaskQueueHostedService>>();
        }

        [Test]
        public void QueueBackgroundRequest_EnqueuesRequest()
        {
            var mockRequest = new Mock<IBaseRequest>().Object;
            Action<object, TimeSpan, ILogger<TaskQueueHostedService>> response = (obj, timeSpan, logger) => { };

            _queue.QueueBackgroundRequest(mockRequest, "TestRequest", response);

            var dequeued = _queue.DequeueAsync(CancellationToken.None).Result;
            dequeued.Request.Should().BeEquivalentTo(mockRequest);
            dequeued.RequestName.Should().Be("TestRequest");
        }

        [Test]
        public async Task DequeueAsync_ReturnsRequestsInOrderEnqueued()
        {
            var request1 = new Mock<IBaseRequest>().Object;
            var request2 = new Mock<IBaseRequest>().Object;
            Action<object, TimeSpan, ILogger<TaskQueueHostedService>> response = (obj, timeSpan, logger) => { };

            _queue.QueueBackgroundRequest(request1, "Request1", response);
            _queue.QueueBackgroundRequest(request2, "Request2", response);

            var dequeued1 = await _queue.DequeueAsync(CancellationToken.None);
            dequeued1.Request.Should().BeEquivalentTo(request1);

            var dequeued2 = await _queue.DequeueAsync(CancellationToken.None);
            dequeued2.Request.Should().BeEquivalentTo(request2);
        }

        [Test]
        public void QueueBackgroundRequest_ThrowsArgumentNullException_WhenRequestIsNull()
        {
            Action act = () => _queue.QueueBackgroundRequest(null, "TestRequest", null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public async Task DequeueAsync_Cancellation()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                cancellationTokenSource.Cancel();

                Func<Task> act = async () => await _queue.DequeueAsync(cancellationTokenSource.Token);

                await act.Should().ThrowAsync<OperationCanceledException>();
            }
        }
    }
}
