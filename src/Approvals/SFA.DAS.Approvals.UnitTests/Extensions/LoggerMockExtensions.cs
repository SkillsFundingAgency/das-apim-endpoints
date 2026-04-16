using System;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.Approvals.UnitTests.Extensions;

public static class LoggerMockExtensions
{
    public static void VerifyLogged<T>(this Mock<ILogger<T>> logger, string messageContained, LogLevel logLevel, Times times)
    {
        logger.Verify(
            l => l.Log(
                It.Is<LogLevel>(level => level == logLevel),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((@object, type) => MatchPartialMatches(@object, new[] { messageContained })),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            times);
    }

    private static bool MatchPartialMatches(object @object, string[] messageContained)
    {
        var generatedString = @object.ToString()!;
        return Array.TrueForAll(messageContained, generatedString.Contains);
    }
}