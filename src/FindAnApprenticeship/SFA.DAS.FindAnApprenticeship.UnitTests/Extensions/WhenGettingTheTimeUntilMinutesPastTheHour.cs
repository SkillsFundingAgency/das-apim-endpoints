using SFA.DAS.FindAnApprenticeship.Extensions;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Extensions;

public class WhenGettingTheTimeUntilMinutesPastTheHour
{
    [Test, MoqAutoData]
    public void Then_The_Time_Span_Should_Be_Correct_Before_The_Minute_Mark()
    {
        // arrange
        var datetime = DateTime.UtcNow.Date.AddMinutes(4);

        // act
        var result = datetime.TimeUntilMinutesPastHour(5);

        // assert
        result.Should().Be(TimeSpan.FromMinutes(1));
    }
    
    [Test, MoqAutoData]
    public void Then_The_Time_Span_Should_Be_Correct_After_The_Minute_Mark()
    {
        // arrange
        var datetime = DateTime.UtcNow.Date.AddMinutes(6);

        // act
        var result = datetime.TimeUntilMinutesPastHour(5);

        // assert
        result.Should().Be(TimeSpan.FromMinutes(59));
    }
}