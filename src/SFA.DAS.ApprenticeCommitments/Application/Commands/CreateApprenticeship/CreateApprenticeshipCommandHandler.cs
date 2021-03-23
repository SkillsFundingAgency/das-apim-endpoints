using MediatR;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApprenticeCommitments.Application.Services.ApprenticeLogin;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Apis.TrainingProviderApi;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipCommandHandler : IRequestHandler<CreateApprenticeshipCommand>
    {
        private readonly ApprenticeCommitmentsService _apprenticeCommitmentsService;
        private readonly CommitmentsV2Service _commitmentsService;
        private readonly TrainingProviderService _trainingProviderService;
        private readonly ApprenticeLoginService _apprenticeLoginService;

        public CreateApprenticeshipCommandHandler(
            ApprenticeCommitmentsService apprenticeCommitmentsService,
            ApprenticeLoginService apprenticeLoginService,
            CommitmentsV2Service commitmentsV2Service,
            TrainingProviderService trainingProviderService)
        {
            _apprenticeCommitmentsService = apprenticeCommitmentsService;
            _apprenticeLoginService = apprenticeLoginService;
            _commitmentsService = commitmentsV2Service;
            _trainingProviderService = trainingProviderService;
        }

        public async Task<Unit> Handle(
            CreateApprenticeshipCommand command,
            CancellationToken cancellationToken)
        {
            var (trainingProvider, apprentice) = await GetExternalData(command);
            var id = Guid.NewGuid();

            await _apprenticeCommitmentsService.CreateApprenticeship(new CreateApprenticeshipRequestData
            {
                ApprenticeId = id,
                ApprenticeshipId = command.ApprenticeshipId,
                Email = command.Email,
                EmployerName = command.EmployerName,
                EmployerAccountLegalEntityId = command.EmployerAccountLegalEntityId,
                TrainingProviderId = command.TrainingProviderId,
                TrainingProviderName = string.IsNullOrWhiteSpace(trainingProvider.TradingName) ? trainingProvider.LegalName : trainingProvider.TradingName,
            });

            await _apprenticeLoginService.SendInvitation(new SendInvitationModel
            {
                SourceId = id,
                Email = command.Email,
                GivenName = apprentice.FirstName,
                FamilyName = apprentice.LastName,
                OrganisationName = command.EmployerName,
                ApprenticeshipName = apprentice.CourseName,
            });

            return Unit.Value;
        }

        private async Task<(TrainingProviderResponse, Apis.CommitmentsV2InnerApi.ApprenticeshipResponse)> GetExternalData(CreateApprenticeshipCommand command)
        {
            var trainingProviderTask = _trainingProviderService.GetTrainingProviderDetails(command.TrainingProviderId);

            var apprenticeTask = _commitmentsService.GetApprenticeshipDetails(
                command.EmployerAccountId,
                command.ApprenticeshipId);

            await Task.WhenAll(trainingProviderTask, apprenticeTask);

            return (trainingProviderTask.Result, apprenticeTask.Result);
        }
    }
}