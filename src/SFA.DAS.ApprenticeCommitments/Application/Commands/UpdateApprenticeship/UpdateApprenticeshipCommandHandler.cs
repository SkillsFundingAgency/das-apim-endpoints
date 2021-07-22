using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Apis.TrainingProviderApi;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using System.Threading;
using System.Threading.Tasks;
using static System.String;
using ApprenticeshipResponse = SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi.ApprenticeshipResponse;

#nullable enable

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.UpdateApprenticeship
{
    public class UpdateApprenticeshipCommandHandler : IRequestHandler<UpdateApprenticeshipCommand>
    {
        private readonly ApprenticeCommitmentsService _apprenticeCommitmentsService;
        private readonly CommitmentsV2Service _commitmentsService;
        private readonly TrainingProviderService _trainingProviderService;
        private readonly CoursesService _coursesService;
        private readonly ILogger<UpdateApprenticeshipCommandHandler> _logger;

        public UpdateApprenticeshipCommandHandler(
            ApprenticeCommitmentsService apprenticeCommitmentsService,
            CommitmentsV2Service commitmentsV2Service,
            TrainingProviderService trainingProviderService,
            CoursesService coursesService,
            ILogger<UpdateApprenticeshipCommandHandler> logger)
        {
            _apprenticeCommitmentsService = apprenticeCommitmentsService;
            _commitmentsService = commitmentsV2Service;
            _trainingProviderService = trainingProviderService;
            _coursesService = coursesService;
            _logger = logger;
        }

        public async Task<Unit> Handle(
            UpdateApprenticeshipCommand command,
            CancellationToken cancellationToken)
        {
            var (apprenticeship, provider, course) = await GetExternalData(command) ?? default;

            if (apprenticeship == null) return default;

            await _apprenticeCommitmentsService.ChangeApprenticeship(new ChangeApprenticeshipRequestData
            {
                CommitmentsContinuedApprenticeshipId = command.CommitmentsContinuedApprenticeshipId,
                CommitmentsApprenticeshipId = command.CommitmentsApprenticeshipId,
                FirstName = apprenticeship.FirstName,
                LastName = apprenticeship.LastName,
                DateOfBirth = apprenticeship.DateOfBirth,
                EmployerName = apprenticeship.EmployerName,
                EmployerAccountLegalEntityId = apprenticeship.AccountLegalEntityId,
                TrainingProviderId = apprenticeship.ProviderId,
                TrainingProviderName = IsNullOrWhiteSpace(provider.TradingName) ? provider.LegalName : provider.TradingName,
                CourseName = course.Title,
                CourseLevel = course.Level,
                PlannedStartDate = apprenticeship.StartDate,
                PlannedEndDate = apprenticeship.EndDate,
                CommitmentsApprovedOn = command.CommitmentsApprovedOn,
            });

            return default;
        }

        private async Task<(ApprenticeshipResponse, TrainingProviderResponse, StandardApiResponse)?>
            GetExternalData(UpdateApprenticeshipCommand command)
        {
            var apprenticeship = await _commitmentsService.GetApprenticeshipDetails(
                command.CommitmentsApprenticeshipId);

            if (IsNullOrEmpty(apprenticeship.Email))
            {
                _logger.LogInformation("Apprenticeship {apprenticeshipId} does not have an email, no point in calling Apprentice Commitments", apprenticeship.Id);
                return default;
            }

            if (apprenticeship.CourseCode.Contains("-"))
            {
                _logger.LogWarning("Apprenticeship {apprenticeshipId} is for a framework, no point in calling Apprentice Commitments", apprenticeship.Id);
                return default;
            }

            var course = _coursesService.GetCourse(apprenticeship.CourseCode);
            var provider = _trainingProviderService.GetTrainingProviderDetails(apprenticeship.ProviderId);

            await Task.WhenAll(course, provider);

            return (apprenticeship, provider.Result, course.Result);
        }
    }
}