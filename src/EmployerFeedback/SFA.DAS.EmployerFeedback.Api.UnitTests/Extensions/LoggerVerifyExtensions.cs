using Microsoft.Extensions.Logging;
using Moq;
using System;

namespace SFA.DAS.EmployerFeedback.Api.UnitTests.Extensions
{
    internal static class LoggerVerifyExtensions
    {
        public static void VerifyLogErrorContains<TLogger>(this Mock<ILogger<TLogger>> logger,
            string messageSubstring,
            Exception? exception = null,
            Times? times = null)
        {
            logger.Verify(x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(messageSubstring, StringComparison.OrdinalIgnoreCase)),
                    exception ?? It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                (times ?? Times.AtLeastOnce()));
        }
    }
}
