using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses;
public abstract class CourseApiResponseBase
{
    public CourseDates CourseDates { get; set; }
    public List<ApprenticeshipFunding> ApprenticeshipFunding { get; set; }

    [JsonIgnore]
    public int MaxFunding => GetFundingDetails(nameof(MaxFunding));

    protected virtual int GetFundingDetails(string prop, DateTime? effectiveDate = null)
    {
        if (ApprenticeshipFunding == null || !ApprenticeshipFunding.Any()) return 0;

        var effDate = effectiveDate ?? DateTime.UtcNow;

        var funding = ApprenticeshipFunding
            .FirstOrDefault(c =>
                c.EffectiveFrom <= effDate
                && (c.EffectiveTo == null || c.EffectiveTo >= effDate));

        if (funding == null)
        {
            funding = ApprenticeshipFunding.FirstOrDefault(c => c.EffectiveTo == null);
        }

        if (prop == nameof(MaxFunding))
        {
            return funding?.MaxEmployerLevyCap
                   ?? ApprenticeshipFunding.FirstOrDefault()?.MaxEmployerLevyCap
                   ?? 0;
        }

        return funding?.Duration
               ?? ApprenticeshipFunding.FirstOrDefault()?.Duration
               ?? 0;
    }
}

public class CourseDates
{
    public DateTime? LastDateStarts { get; set; }

    public DateTime? EffectiveTo { get; set; }

    public DateTime EffectiveFrom { get; set; }
}