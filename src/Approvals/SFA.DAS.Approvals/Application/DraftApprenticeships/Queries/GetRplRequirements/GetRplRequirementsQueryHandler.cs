using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.Approvals.InnerApi.TrainingTypesApi.Requests;
using SFA.DAS.Approvals.InnerApi.TrainingTypesApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetRplRequirements
{
    public class GetRplRequirementsQueryHandler(
        ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
        ITrainingTypesApiClient trainingTypesApiClient,
        ILogger<GetRplRequirementsQueryHandler> logger)
        : IRequestHandler<GetRplRequirementsQuery, GetRplRequirementsResult>
    {
        public async Task<GetRplRequirementsResult> Handle(GetRplRequirementsQuery request, CancellationToken cancellationToken)
        {
            var standard = await coursesApiClient.Get<GetStandardsListItem>(new GetStandardDetailsByIdRequest(request.CourseId));
            
            if (standard == null)
            {
                logger.LogError("Standard not found for course ID {CourseId}", request.CourseId);
                return null;
            }
            
            var rplRequirements = await trainingTypesApiClient.Get<GetRecognitionOfPriorLearningResponse>(new GetRecognitionOfPriorLearningRequest(standard.ApprenticeshipType));

            if (rplRequirements == null)
            {
                logger.LogError("RPL requirements not found for apprenticeship type {ApprenticeshipType}", standard.ApprenticeshipType);
                return null;
            }

            return new GetRplRequirementsResult
            {
                ApprenticeshipType = standard.ApprenticeshipType,
                IsRequired = rplRequirements.IsRequired,
                OffTheJobTrainingMinimumHours = rplRequirements.OffTheJobTrainingMinimumHours
            };
        }
    }
} 