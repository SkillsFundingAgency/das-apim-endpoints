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
using CreateApprenticeshipRequestData = SFA.DAS.ApprenticeCommitments.Apis.InnerApi.CreateApprenticeshipRequestData;

#nullable enable

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.CreateRegistration
{
    public class CreateRegistrationCommandHandler : IRequestHandler<CreateRegistrationCommand, CreateRegistrationResponse?>
    {
        private readonly ApprenticeCommitmentsService _apprenticeCommitmentsService;
        private readonly CommitmentsV2Service _commitmentsService;
        private readonly TrainingProviderService _trainingProviderService;
        private readonly ApprenticeLoginConfiguration _loginConfiguration;
        private readonly CoursesService _coursesService;
        private readonly ILogger<CreateRegistrationCommandHandler> _logger;

        public CreateRegistrationCommandHandler(
            ApprenticeCommitmentsService apprenticeCommitmentsService,
            ApprenticeLoginConfiguration loginConfiguration,
            CommitmentsV2Service commitmentsV2Service,
            TrainingProviderService trainingProviderService,
            CoursesService coursesService,
            ILogger<CreateRegistrationCommandHandler> logger)
        {
            _apprenticeCommitmentsService = apprenticeCommitmentsService;
            _loginConfiguration = loginConfiguration;
            _commitmentsService = commitmentsV2Service;
            _trainingProviderService = trainingProviderService;
            _coursesService = coursesService;
            _logger = logger;
        }

        public async Task<CreateRegistrationResponse?> Handle(
            CreateRegistrationCommand command,
            CancellationToken cancellationToken)
        {
            var (apprentice, trainingProvider, course) = await GetExternalData(command);

            if (apprentice == null) return default;

            var id = Guid.NewGuid();

            await _apprenticeCommitmentsService.CreateApprenticeship(new CreateApprenticeshipRequestData
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
                CourseName = course.Title,
                CourseLevel = course.Level,
                CourseDuration = course.TypicalDuration,
                PlannedStartDate = apprentice.StartDate,
                PlannedEndDate = apprentice.EndDate,
                CommitmentsApprovedOn = command.CommitmentsApprovedOn,
            });

            var res = new CreateRegistrationResponse
            {
                RegistrationId = id,
                Email = apprentice.Email,
                GivenName = apprentice.FirstName,
                FamilyName = apprentice.LastName,
                ApprenticeshipName = apprentice.CourseName,
            };

            _logger.LogInformation($"Create Apprenticeship response: {JsonConvert.SerializeObject(res)}");

            return res;
        }

        private async Task<(ApprenticeshipResponse, TrainingProviderResponse, StandardApiResponse)>
            GetExternalData(CreateRegistrationCommand command)
        {
            var provider = _trainingProviderService.GetTrainingProviderDetails(
                    command.TrainingProviderId);

            var apprentice = _commitmentsService.GetApprenticeshipDetails(
                command.EmployerAccountId,
                command.CommitmentsApprenticeshipId);

            var courseCode = (await apprentice).GetCourseCode(_logger);
            if (courseCode is null) return default;

            var course = _coursesService.GetCourse(courseCode);

            return (await apprentice, await provider, await course);
        }

        private static string ProviderName(TrainingProviderResponse trainingProvider)
            => string.IsNullOrWhiteSpace(trainingProvider.TradingName)
                ? trainingProvider.LegalName
                : trainingProvider.TradingName;
    }
}