using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi;
using SFA.DAS.ApprenticeCommitments.Apis.TrainingProviderApi;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using SFA.DAS.ApprenticeCommitments.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using CreateApprenticeshipRequestData = SFA.DAS.ApprenticeCommitments.Apis.InnerApi.ApprovalCreatedRequestData;

#nullable enable

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApproval
{
    public class CreateApprovalCommandHandler : IRequestHandler<CreateApprovalCommand, CreateApprovalResponse?>
    {
        private readonly ApprenticeCommitmentsService _apprenticeCommitmentsService;
        private readonly CommitmentsV2Service _commitmentsService;
        private readonly TrainingProviderService _trainingProviderService;
        private readonly ApprenticeLoginConfiguration _loginConfiguration;
        private readonly CoursesService _coursesService;
        private readonly ILogger<CreateApprovalCommandHandler> _logger;

        public CreateApprovalCommandHandler(
            ApprenticeCommitmentsService apprenticeCommitmentsService,
            ApprenticeLoginConfiguration loginConfiguration,
            CommitmentsV2Service commitmentsV2Service,
            TrainingProviderService trainingProviderService,
            CoursesService coursesService,
            ILogger<CreateApprovalCommandHandler> logger)
        {
            _apprenticeCommitmentsService = apprenticeCommitmentsService;
            _loginConfiguration = loginConfiguration;
            _commitmentsService = commitmentsV2Service;
            _trainingProviderService = trainingProviderService;
            _coursesService = coursesService;
            _logger = logger;
        }

        public async Task<CreateApprovalResponse?> Handle(
            CreateApprovalCommand command,
            CancellationToken cancellationToken)
        {
            var (apprentice, trainingProvider, course) = await GetExternalData(command);

            if (apprentice == null) return default;

            var id = Guid.NewGuid();

            await _apprenticeCommitmentsService.CreateApproval(new CreateApprenticeshipRequestData
            {
                RegistrationId = id,
                CommitmentsApprenticeshipId = command.CommitmentsApprenticeshipId,
                FirstName = apprentice.FirstName,
                LastName = apprentice.LastName,
                DateOfBirth = apprentice.DateOfBirth,
                Email = apprentice.Email,
                EmployerName = command.EmployerName,
                EmployerAccountLegalEntityId = command.EmployerAccountLegalEntityId,
                TrainingProviderId = command.TrainingProviderId,
                TrainingProviderName = ProviderName(trainingProvider),
                DeliveryModel = apprentice.DeliveryModel,
                CourseName = course.Title,
                CourseLevel = course.Level,
                CourseOption = apprentice.Option,
                CourseDuration = course.TypicalDuration,
                PlannedStartDate = apprentice.StartDate,
                PlannedEndDate = apprentice.EndDate,
                EmploymentEndDate = apprentice.EmploymentEndDate,
                CommitmentsApprovedOn = command.CommitmentsApprovedOn,
            });

            var res = new CreateApprovalResponse
            {
                RegistrationId = id,
                Email = apprentice.Email,
                GivenName = apprentice.FirstName,
                FamilyName = apprentice.LastName,
                ApprenticeshipName = apprentice.CourseName,
            };

            _logger.LogInformation($"Create Apprenticeship response was successful: RegistrationId: {res.RegistrationId}, Apprenticeship: {res.ApprenticeshipName} for CommitmentsApprenticeshipId: {command.CommitmentsApprenticeshipId}");

            return res;
        }

        private async Task<(ApprenticeshipResponse, TrainingProviderResponse, StandardApiResponse)>
            GetExternalData(CreateApprovalCommand command)
        {
            var apprenticeship = await _commitmentsService.GetApprenticeshipDetails(
                command.EmployerAccountId,
                command.CommitmentsApprenticeshipId);

            if (string.IsNullOrEmpty(apprenticeship.Email))
            {
                _logger.LogInformation("Apprenticeship {apprenticeshipId} does not have an email, no point in continuing", apprenticeship.Id);
                return default;
            }

            var courseCode = apprenticeship.GetCourseCode(_logger);
            if (courseCode is null) return default;

            var course = _coursesService.GetCourse(courseCode);

            var provider = _trainingProviderService.GetTrainingProviderDetails(
                command.TrainingProviderId);


            return (apprenticeship, await provider, await course);
        }

        private static string ProviderName(TrainingProviderResponse trainingProvider)
            => string.IsNullOrWhiteSpace(trainingProvider.TradingName)
                ? trainingProvider.LegalName
                : trainingProvider.TradingName;
    }
}