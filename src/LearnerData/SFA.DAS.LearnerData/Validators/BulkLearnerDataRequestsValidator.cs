using FluentValidation;
using SFA.DAS.LearnerData.Requests;
using Microsoft.AspNetCore.Http;

namespace SFA.DAS.LearnerData.Validators
{
    public class BulkLearnerDataRequestsValidator : AbstractValidator<IEnumerable<LearnerDataRequest>>
    {
        public BulkLearnerDataRequestsValidator(IHttpContextAccessor accessor)
        {
            int academicYear = 0;
            long ukprn = 0;
            var routeData = accessor.HttpContext?.Request?.RouteValues;

            if (routeData == null)
            {
                throw new ArgumentNullException(nameof(accessor), "Route data cannot be null");
            }

            if (routeData.TryGetValue("academicyear", out var academicYearObject))
            {
                int.TryParse(academicYearObject?.ToString(), out academicYear);
            }

            if (routeData.TryGetValue("ukprn", out var ukprnObject))
            {
                long.TryParse(ukprnObject?.ToString(), out ukprn);
            }
            RuleForEach(x => x).SetValidator(new LearnerDataRequestValidator(ukprn, academicYear));
        }
    }
}
