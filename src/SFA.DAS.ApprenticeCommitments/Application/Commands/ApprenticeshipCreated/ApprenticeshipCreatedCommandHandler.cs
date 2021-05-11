using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Apis.TrainingProviderApi;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using SFA.DAS.ApprenticeCommitments.Application.Services.ApprenticeLogin;
using ApprenticeshipResponse = SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi.ApprenticeshipResponse;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.ApprenticeshipCreated
{
    public class ApprenticeshipCreatedCommandHandler : IRequestHandler<ApprenticeshipCreatedCommand>
    {
        private readonly ApprenticeCommitmentsService _apprenticeCommitmentsService;
        private readonly CommitmentsV2Service _commitmentsService;
        private readonly TrainingProviderService _trainingProviderService;
        private readonly ApprenticeLoginService _apprenticeLoginService;
        private readonly CoursesService _coursesService;

        public ApprenticeshipCreatedCommandHandler(
            ApprenticeCommitmentsService apprenticeCommitmentsService,
            ApprenticeLoginService apprenticeLoginService,
            CommitmentsV2Service commitmentsV2Service,
            TrainingProviderService trainingProviderService,
            CoursesService coursesService)
        {
            _apprenticeCommitmentsService = apprenticeCommitmentsService;
            _apprenticeLoginService = apprenticeLoginService;
            _commitmentsService = commitmentsV2Service;
            _trainingProviderService = trainingProviderService;
            _coursesService = coursesService;
        }

        public async Task<Unit> Handle(
            ApprenticeshipCreatedCommand command,
            CancellationToken cancellationToken)
        {
            var (trainingProvider, apprenticeship, course) = await GetExternalData(command);

            if (apprenticeship.ContinuationOfId == null)
            {
                await CreateApprenticeshipAndSendInvitation(command, trainingProvider, course, apprenticeship);
            }
            else
            {
                await ChangeApprenticeship(command, trainingProvider, course, apprenticeship);
            }
            return Unit.Value;
        }

        private async Task ChangeApprenticeship(ApprenticeshipCreatedCommand command, TrainingProviderResponse trainingProvider,
            StandardApiResponse course, ApprenticeshipResponse apprenticeship)
        {
            await _apprenticeCommitmentsService.ChangeApprenticeship(new ChangeApprenticeshipRequestData
            {
                ContinuationOfApprenticeshipId = apprenticeship.ContinuationOfId,
                ApprenticeshipId = command.ApprenticeshipId,
                Email = command.Email,
                EmployerName = command.EmployerName,
                EmployerAccountLegalEntityId = command.EmployerAccountLegalEntityId,
                TrainingProviderId = command.TrainingProviderId,
                TrainingProviderName = GetTrainingProviderName(trainingProvider),
                CourseName = course.Title,
                CourseLevel = course.Level,
                PlannedStartDate = apprenticeship.StartDate,
                PlannedEndDate = apprenticeship.EndDate,
                ApprovedOn = command.AgreedOn,
            });
        }

        private static string GetTrainingProviderName(TrainingProviderResponse trainingProvider)
        {
            return string.IsNullOrWhiteSpace(trainingProvider.TradingName)
                ? trainingProvider.LegalName
                : trainingProvider.TradingName;
        }

        private async Task CreateApprenticeshipAndSendInvitation(ApprenticeshipCreatedCommand command,
            TrainingProviderResponse trainingProvider, StandardApiResponse course, ApprenticeshipResponse apprenticeship)
        {
            var id = Guid.NewGuid();

            await _apprenticeCommitmentsService.CreateApprenticeship(new CreateApprenticeshipRequestData
            {
                ApprenticeId = id,
                ApprenticeshipId = command.ApprenticeshipId,
                Email = command.Email,
                EmployerName = command.EmployerName,
                EmployerAccountLegalEntityId = command.EmployerAccountLegalEntityId,
                TrainingProviderId = command.TrainingProviderId,
                TrainingProviderName = GetTrainingProviderName(trainingProvider),
                CourseName = course.Title,
                CourseLevel = course.Level,
                PlannedStartDate = apprenticeship.StartDate,
                PlannedEndDate = apprenticeship.EndDate,
                ApprovedOn = command.AgreedOn,
            });

            await _apprenticeLoginService.SendInvitation(new SendInvitationModel
            {
                SourceId = id,
                Email = command.Email,
                GivenName = apprenticeship.FirstName,
                FamilyName = apprenticeship.LastName,
                OrganisationName = command.EmployerName,
                ApprenticeshipName = apprenticeship.CourseName,
            });
        }

        private async Task<(TrainingProviderResponse, Apis.CommitmentsV2InnerApi.ApprenticeshipResponse, StandardApiResponse course)> GetExternalData(ApprenticeshipCreatedCommand command)
        {
            var trainingProviderTask = _trainingProviderService.GetTrainingProviderDetails(command.TrainingProviderId);

            var apprenticeTask = _commitmentsService.GetApprenticeshipDetails(
                command.EmployerAccountId,
                command.ApprenticeshipId);

            await Task.WhenAll(trainingProviderTask, apprenticeTask);

            var course = await _coursesService.GetCourse(apprenticeTask.Result.CourseCode);

            return (trainingProviderTask.Result, apprenticeTask.Result, course);
        }
    }
}