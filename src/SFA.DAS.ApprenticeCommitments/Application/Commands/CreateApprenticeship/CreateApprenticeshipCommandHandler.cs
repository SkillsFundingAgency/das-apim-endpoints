using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Apis.TrainingProviderApi;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using SFA.DAS.ApprenticeCommitments.Application.Services.ApprenticeLogin;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipCommandHandler : IRequestHandler<CreateApprenticeshipCommand>
    {
        private readonly ApprenticeCommitmentsService _apprenticeCommitmentsService;
        private readonly CommitmentsV2Service _commitmentsService;
        private readonly TrainingProviderService _trainingProviderService;
        private readonly ApprenticeLoginService _apprenticeLoginService;
        private readonly CoursesService _coursesService;
        private readonly ILogger<CreateApprenticeshipCommandHandler> _logger;

        public CreateApprenticeshipCommandHandler(
            ApprenticeCommitmentsService apprenticeCommitmentsService,
            ApprenticeLoginService apprenticeLoginService,
            CommitmentsV2Service commitmentsV2Service,
            TrainingProviderService trainingProviderService,
            CoursesService coursesService,
            ILogger<CreateApprenticeshipCommandHandler> logger)
        {
            _apprenticeCommitmentsService = apprenticeCommitmentsService;
            _apprenticeLoginService = apprenticeLoginService;
            _commitmentsService = commitmentsV2Service;
            _trainingProviderService = trainingProviderService;
            _coursesService = coursesService;
            _logger = logger;
        }

        public async Task<Unit> Handle(
            CreateApprenticeshipCommand command,
            CancellationToken cancellationToken)
        {
            var (trainingProvider, apprentice, course) = await GetExternalData(command);

            if (string.IsNullOrEmpty(apprentice.Email)) return Unit.Value;

            var id = Guid.NewGuid();

            _logger.LogInformation($"Creating registraion for `{apprentice.FirstName} {apprentice.LastName}`");

            await _apprenticeCommitmentsService.CreateApprenticeship(new CreateApprenticeshipRequestData
            {
                ApprenticeId = id,
                CommitmentsApprenticeshipId = command.CommitmentsApprenticeshipId,
                FirstName = apprentice.FirstName,
                LastName = apprentice.LastName,
                Email = apprentice.Email,
                EmployerName = command.EmployerName,
                EmployerAccountLegalEntityId = command.EmployerAccountLegalEntityId,
                TrainingProviderId = command.TrainingProviderId,
                TrainingProviderName = string.IsNullOrWhiteSpace(trainingProvider.TradingName) ? trainingProvider.LegalName : trainingProvider.TradingName,
                CourseName = course.Title,
                CourseLevel = course.Level,
                PlannedStartDate = apprentice.StartDate,
                PlannedEndDate = apprentice.EndDate,
                CommitmentsApprovedOn = command.CommitmentsApprovedOn,
            });

            await _apprenticeLoginService.SendInvitation(new SendInvitationModel
            {
                SourceId = id,
                Email = apprentice.Email,
                GivenName = apprentice.FirstName,
                FamilyName = apprentice.LastName,
                OrganisationName = command.EmployerName,
                ApprenticeshipName = apprentice.CourseName,
            });

            return Unit.Value;
        }

        private async Task<(TrainingProviderResponse, Apis.CommitmentsV2InnerApi.ApprenticeshipResponse, StandardApiResponse course)> GetExternalData(CreateApprenticeshipCommand command)
        {
            var trainingProviderTask = _trainingProviderService.GetTrainingProviderDetails(command.TrainingProviderId);

            var apprenticeTask = _commitmentsService.GetApprenticeshipDetails(
                command.EmployerAccountId,
                command.CommitmentsApprenticeshipId);

            await Task.WhenAll(trainingProviderTask, apprenticeTask);

            var courseCode = ApprenticeCourseCode(apprenticeTask.Result);

            var course = await _coursesService.GetCourse(courseCode);

            return (trainingProviderTask.Result, apprenticeTask.Result, course);
        }

        private static string ApprenticeCourseCode(Apis.CommitmentsV2InnerApi.ApprenticeshipResponse apprenticeTask)
        {
            // Remove after Standards Versioning goes live
            // Revert to just StandardUId
            return string.IsNullOrWhiteSpace(apprenticeTask.StandardUId)
                ? apprenticeTask.CourseCode : apprenticeTask.StandardUId;
        }
    }
}