using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Approvals.Types;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Approvals.InnerApi.Responses;

public class GetCoursesListItem
{
    public string LarsCode { get; set; }
    public string Title { get; set; }
    public int Level { get; set; }
    public LearningType LearningType { get; set; }
    public CourseDates CourseDates { get; set; }
    public List<ApprenticeshipFunding> ApprenticeshipFunding { get; set; }
    public int MaxFunding => GetMaxFunding();

    private int GetMaxFunding()
    {
        if (ApprenticeshipFunding == null || !ApprenticeshipFunding.Any()) return 0;

        var funding = ApprenticeshipFunding
            .FirstOrDefault(c =>
                c.EffectiveFrom <= DateTime.UtcNow
                && (c.EffectiveTo == null || c.EffectiveTo >= DateTime.UtcNow));

        if (funding == null)
        {
            funding = ApprenticeshipFunding.FirstOrDefault(c => c.EffectiveTo == null);
        }

        return funding?.MaxEmployerLevyCap
               ?? ApprenticeshipFunding.FirstOrDefault()?.MaxEmployerLevyCap
               ?? 0;
    }
}

public class CourseDates
{
    public DateTime? LastDateStarts { get; set; }

    public DateTime? EffectiveTo { get; set; }

    public DateTime EffectiveFrom { get; set; }
}