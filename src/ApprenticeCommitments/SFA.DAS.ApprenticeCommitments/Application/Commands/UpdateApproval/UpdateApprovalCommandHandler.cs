using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService;
using SFA.DAS.SharedOuterApi.Services;
using static System.String;
using ApprenticeshipResponse = SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi.ApprenticeshipResponse;

#nullable enable

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.UpdateApproval
{
    public class UpdateApprovalCommandHandler : IRequestHandler<UpdateApprovalCommand>
    {
        private readonly ApprenticeCommitmentsService _apprenticeCommitmentsService;
        private readonly CommitmentsV2Service _commitmentsService;
        private readonly TrainingProviderService _trainingProviderService;
        private readonly CoursesService _coursesService;
        private readonly ILogger<UpdateApprovalCommandHandler> _logger;

        public UpdateApprovalCommandHandler(
            ApprenticeCommitmentsService apprenticeCommitmentsService,
            CommitmentsV2Service commitmentsV2Service,
            TrainingProviderService trainingProviderService,
            CoursesService coursesService,
            ILogger<UpdateApprovalCommandHandler> logger)
        {
            _apprenticeCommitmentsService = apprenticeCommitmentsService;
            _commitmentsService = commitmentsV2Service;
            _trainingProviderService = trainingProviderService;
            _coursesService = coursesService;
            _logger = logger;
        }

        public async Task<Unit> Handle(
            UpdateApprovalCommand command,
            CancellationToken cancellationToken)
        {
            var (apprenticeship, provider, course) = await GetExternalData(command) ?? default;

            if (apprenticeship == null) return default;

            await _apprenticeCommitmentsService.ChangeApproval(new ChangeApprovalRequestData
            {
                CommitmentsContinuedApprenticeshipId = command.CommitmentsContinuedApprenticeshipId,
                CommitmentsApprenticeshipId = command.CommitmentsApprenticeshipId,
                FirstName = apprenticeship.FirstName,
                LastName = apprenticeship.LastName,
                DateOfBirth = apprenticeship.DateOfBirth,
                Email = apprenticeship.Email,
                EmployerName = apprenticeship.EmployerName,
                EmployerAccountLegalEntityId = apprenticeship.AccountLegalEntityId,
                TrainingProviderId = apprenticeship.ProviderId,
                TrainingProviderName = IsNullOrWhiteSpace(provider.TradingName) ? provider.LegalName : provider.TradingName,
                DeliveryModel = apprenticeship.DeliveryModel,
                CourseName = course.Title,
                CourseOption = apprenticeship.Option,
                CourseLevel = course.Level,
                CourseDuration = course.TypicalDuration,
                PlannedStartDate = apprenticeship.StartDate,
                PlannedEndDate = apprenticeship.EndDate,
                CommitmentsApprovedOn = command.CommitmentsApprovedOn,
                EmploymentEndDate = apprenticeship.EmploymentEndDate
            });

            return default;
        }

        private async Task<(ApprenticeshipResponse, TrainingProviderResponse, StandardApiResponse)?>
            GetExternalData(UpdateApprovalCommand command)
        {
            var apprenticeship = await _commitmentsService.GetApprenticeshipDetails(
                command.CommitmentsApprenticeshipId);

            if (IsNullOrEmpty(apprenticeship.Email))
            {
                _logger.LogInformation("Apprenticeship {apprenticeshipId} does not have an email, no point in continuing", apprenticeship.Id);
                return default;
            }

            var courseCode = apprenticeship.GetCourseCode(_logger);
            if (courseCode is null) return default;

            var course = _coursesService.GetCourse(courseCode);
            var provider = _trainingProviderService.GetTrainingProviderDetails(apprenticeship.ProviderId);

            return (apprenticeship, await provider, await course);
        }
    }
}