namespace SFA.DAS.RecruitQa.Domain;

public static class WageExtensions
{
    public static string GetDuration(this Wage? wage)
    {
        if (wage is null) return string.Empty;

        return wage.DurationUnit switch
        {
            DurationUnit.Year => Pluralize(wage.Duration, "year"),
            DurationUnit.Week => Pluralize(wage.Duration, "week"),
            DurationUnit.Month => FormatMonths(wage.Duration),
            _ => string.Empty
        };
    }

    private static string Pluralize(int? value, string unit)
    {
        return value is 1 ? $"1 {unit}" : $"{value} {unit}s";
    }

    private static string FormatMonths(int? totalMonths)
    {
        if (totalMonths == null) return string.Empty;

        var years = totalMonths / 12;
        var months = totalMonths % 12;

        var parts = new List<string>();

        if (years > 0) parts.Add(Pluralize(years, "year"));
        if (months > 0) parts.Add(Pluralize(months, "month"));

        return parts.Count > 0 ? string.Join(" ", parts) : string.Empty;
    }
}