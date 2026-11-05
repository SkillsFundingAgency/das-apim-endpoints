namespace SFA.DAS.LearnerData.Application.Fm36.Common;

public static class DateTimeExtensions
{
    public static DateOnly? ToDateOnly(this DateTime? value) => value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    public static DateOnly ToDateOnly(this DateTime value) => DateOnly.FromDateTime(value);
}