namespace SFA.DAS.ApprenticeAan.Application.Extensions;

public static class DateTimeExtensions
{
    public static string ToApiString(this DateTime date) => date.ToString("yyyy-MM-dd");
}
