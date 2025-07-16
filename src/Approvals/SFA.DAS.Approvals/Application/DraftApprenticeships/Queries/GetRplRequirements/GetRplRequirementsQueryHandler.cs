using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Services;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetRplRequirements
{
    public class GetRplRequirementsQueryHandler(
        ICourseTypeRulesService courseTypeRulesService,
        ILogger<GetRplRequirementsQueryHandler> logger)
        : IRequestHandler<GetRplRequirementsQuery, GetRplRequirementsResult>
    {
        public async Task<GetRplRequirementsResult> Handle(GetRplRequirementsQuery request, CancellationToken cancellationToken)
        {
            var rplRules = await courseTypeRulesService.GetRplRulesAsync(request.CourseId);

            return new GetRplRequirementsResult
            {
                ApprenticeshipType = rplRules.Standard.ApprenticeshipType,
                IsRequired = rplRules.RplRules.IsRequired,
                OffTheJobTrainingMinimumHours = rplRules.RplRules.OffTheJobTrainingMinimumHours
            };
        }
    }
} 