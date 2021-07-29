using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Apis.TrainingProviderApi;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using SFA.DAS.ApprenticeCommitments.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using static System.String;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipCommandHandler : IRequestHandler<CreateApprenticeshipCommand, CreateApprenticeshipResponse>
    {
        private readonly ApprenticeCommitmentsService _apprenticeCommitmentsService;
        private readonly CommitmentsV2Service _commitmentsService;
        private readonly TrainingProviderService _trainingProviderService;
        private readonly ApprenticeLoginConfiguration _loginConfiguration;
        private readonly CoursesService _coursesService;
        private readonly ILogger<CreateApprenticeshipCommandHandler> _logger;

        public CreateApprenticeshipCommandHandler(
            ApprenticeCommitmentsService apprenticeCommitmentsService,
            ApprenticeLoginConfiguration loginConfiguration,
            CommitmentsV2Service commitmentsV2Service,
            TrainingProviderService trainingProviderService,
            CoursesService coursesService,
            ILogger<CreateApprenticeshipCommandHandler> logger)
        {
            _apprenticeCommitmentsService = apprenticeCommitmentsService;
            _loginConfiguration = loginConfiguration;
            _commitmentsService = commitmentsV2Service;
            _trainingProviderService = trainingProviderService;
            _coursesService = coursesService;
            _logger = logger;
        }

        public async Task<CreateApprenticeshipResponse> Handle(
            CreateApprenticeshipCommand command,
            CancellationToken cancellationToken)
        {
            var (apprentice, trainingProvider, course) = await GetExternalData(command) ?? default;

            if (apprentice == null) return default;

            var id = Guid.NewGuid();

            await _apprenticeCommitmentsService.CreateApprenticeship(new CreateApprenticeshipRequestData
            {
                ApprenticeId = id,
                CommitmentsApprenticeshipId = command.CommitmentsApprenticeshipId,
                FirstName = apprentice.FirstName,
                LastName = apprentice.LastName,
                DateOfBirth = apprentice.DateOfBirth,
                Email = apprentice.Email,
                EmployerName = command.EmployerName,
                EmployerAccountLegalEntityId = command.EmployerAccountLegalEntityId,
                TrainingProviderId = command.TrainingProviderId,
                TrainingProviderName = string.IsNullOrWhiteSpace(trainingProvider.TradingName) ? trainingProvider.LegalName : trainingProvider.TradingName,
                CourseName = course.Title,
                CourseLevel = course.Level,
                CourseDuration = course.TypicalDuration,
                PlannedStartDate = apprentice.StartDate,
                PlannedEndDate = apprentice.EndDate,
                CommitmentsApprovedOn = command.CommitmentsApprovedOn,
            });

            // return parameters for the invitation
            var res = new CreateApprenticeshipResponse
            {
                SourceId = id,
                Email = apprentice.Email,
                GivenName = apprentice.FirstName,
                FamilyName = apprentice.LastName,
                ApprenticeshipName = apprentice.CourseName,
            };

            _logger.LogInformation($"Create Apprenticeship response: {JsonConvert.SerializeObject(res)}");

            return res;
        }

        private async Task<(Apis.CommitmentsV2InnerApi.ApprenticeshipResponse, TrainingProviderResponse, StandardApiResponse)?>
            GetExternalData(CreateApprenticeshipCommand command)
        {
            var trainingProviderTask = _trainingProviderService.GetTrainingProviderDetails(command.TrainingProviderId);

            var apprenticeTask = _commitmentsService.GetApprenticeshipDetails(
                command.EmployerAccountId,
                command.CommitmentsApprenticeshipId);

            await Task.WhenAll(trainingProviderTask, apprenticeTask);

            if (IsNullOrEmpty(apprenticeTask.Result.Email))
            {
                _logger.LogInformation("Apprenticeship {apprenticeshipId} does not have an email, no point in calling Apprentice Commitments", apprenticeTask.Result.Id);
                return default;
            }

            if (apprenticeTask.Result.CourseCode.Contains("-"))
            {
                _logger.LogWarning("Apprenticeship {apprenticeshipId} is for a framework, no point in calling Apprentice Commitments", apprenticeTask.Result.Id);
                return default;
            }

            var courseCode = ApprenticeCourseCode(apprenticeTask.Result);

            var course = await _coursesService.GetCourse(courseCode);

            return (apprenticeTask.Result, trainingProviderTask.Result, course);
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