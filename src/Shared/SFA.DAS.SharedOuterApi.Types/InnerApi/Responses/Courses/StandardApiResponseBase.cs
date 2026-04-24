using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses
{
    public abstract class StandardApiResponseBase
    {
        public CourseDate CourseDates { get; set; }
        public List<ApprenticeshipFunding> ApprenticeshipFunding { get; set; }

        [JsonIgnore]
        public int MaxFunding => GetFundingDetails(nameof(MaxFunding));
        [JsonIgnore]
        public int TypicalDuration => GetFundingDetails(nameof(TypicalDuration));
        [JsonIgnore]
        public bool IsActive => IsStandardActive();

        public int MaxFundingOn(DateTime effectiveDate)
        {
            return GetFundingDetails(nameof(MaxFunding), effectiveDate);
        }

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

        private bool IsStandardActive()
        {
            if (CourseDates == null) return false;

            return CourseDates.EffectiveFrom.Date <= DateTime.UtcNow.Date
                   && (!CourseDates.EffectiveTo.HasValue ||
                       CourseDates.EffectiveTo.Value.Date >= DateTime.UtcNow.Date)
                   && (!CourseDates.LastDateStarts.HasValue ||
                       CourseDates.LastDateStarts.Value.Date >= DateTime.UtcNow.Date);
        }
    }

    public class CourseDate
    {
        public DateTime? LastDateStarts { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public DateTime EffectiveFrom { get; set; }
    }

    public class ApprenticeshipFunding
    {
        public int MaxEmployerLevyCap { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public DateTime EffectiveFrom { get; set; }
        public int Duration { get; set; }
        public int? FoundationAppFirstEmpPayment { get; set; }
        public int? FoundationAppSecondEmpPayment { get; set; }
        public int? FoundationAppThirdEmpPayment { get; set; }
    }
}