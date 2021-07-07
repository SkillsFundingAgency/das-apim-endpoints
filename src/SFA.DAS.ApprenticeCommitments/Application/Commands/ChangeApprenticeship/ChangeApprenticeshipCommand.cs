using MediatR;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Apis.TrainingProviderApi;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using SFA.DAS.ApprenticeCommitments.Application.Services.ApprenticeLogin;
using System;
using System.Threading;
using System.Threading.Tasks;
using CommitmentsApprenticeshipResponse = SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi.ApprenticeshipResponse;

#nullable enable

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.ChangeApprenticeship
{
    public class ChangeApprenticeshipCommand : IRequest
    {
        public long CommitmentsApprenticeshipId { get; set; }
        public DateTime CommitmentsApprovedOn { get; set; }
    }

    public class UpdateApprenticeshipCommandHandler : IRequestHandler<ChangeApprenticeshipCommand>
    {
        private readonly ApprenticeCommitmentsService _apprenticeCommitmentsService;
        private readonly CommitmentsV2Service _commitmentsService;
        private readonly TrainingProviderService _trainingProviderService;
        private readonly ApprenticeLoginService _apprenticeLoginService;
        private readonly CoursesService _coursesService;

        public UpdateApprenticeshipCommandHandler(
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
            ChangeApprenticeshipCommand command,
            CancellationToken cancellationToken)
        {
            var (provider, apprenticeship, course) = await GetExternalData(command);

            await _apprenticeCommitmentsService.ChangeApprenticeship(new ChangeApprenticeshipRequestData
            {
                CommitmentsApprenticeshipId = command.CommitmentsApprenticeshipId,
                EmployerName = apprenticeship.EmployerName,
                EmployerAccountLegalEntityId = apprenticeship.AccountLegalEntityId,
                TrainingProviderId = apprenticeship.ProviderId,
                TrainingProviderName = string.IsNullOrWhiteSpace(provider.TradingName) ? provider.LegalName : provider.TradingName,
                CourseName = course.Title,
                CourseLevel = course.Level,
                PlannedStartDate = apprenticeship.StartDate,
                PlannedEndDate = apprenticeship.EndDate,
                CommitmentsApprovedOn = command.CommitmentsApprovedOn,
            });

            return Unit.Value;
        }

        private async Task<(TrainingProviderResponse, CommitmentsApprenticeshipResponse apprenticeship, StandardApiResponse)>
            GetExternalData(ChangeApprenticeshipCommand command)
        {
            var apprenticeship = await _commitmentsService.GetApprenticeshipDetails(
                command.CommitmentsApprenticeshipId);

            var course = _coursesService.GetCourse(apprenticeship.CourseCode);
            var provider = _trainingProviderService.GetTrainingProviderDetails(apprenticeship.ProviderId);

            await Task.WhenAll(course, provider);

            return (provider.Result, apprenticeship, course.Result);
        }
    }
}