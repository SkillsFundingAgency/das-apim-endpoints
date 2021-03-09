using MediatR;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApprenticeCommitments.Application.Services.ApprenticeLogin;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipCommandHandler : IRequestHandler<CreateApprenticeshipCommand>
    {
        private readonly ApprenticeCommitmentsService _apprenticeCommitmentsService;
        private readonly CommitmentsV2Service _commitmentsService;
        private readonly ApprenticeLoginService _apprenticeLoginService;

        public CreateApprenticeshipCommandHandler(
            ApprenticeCommitmentsService apprenticeCommitmentsService,
            ApprenticeLoginService apprenticeLoginService,
            CommitmentsV2Service commitmentsV2Service)
        {
            _apprenticeCommitmentsService = apprenticeCommitmentsService;
            _apprenticeLoginService = apprenticeLoginService;
            _commitmentsService = commitmentsV2Service;
        }

        public async Task<Unit> Handle(
            CreateApprenticeshipCommand command,
            CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();

            var apprentice = await _commitmentsService.GetApprenticeshipDetails(
                command.EmployerAccountId,
                command.ApprenticeshipId);

            await _apprenticeCommitmentsService.CreateApprenticeship(
                id,
                command.ApprenticeshipId,
                command.Email,
                command.Organisation);

            await _apprenticeLoginService.SendInvitation(new SendInvitationModel
            {
                SourceId = id,
                Email = command.Email,
                GivenName = apprentice.FirstName,
                FamilyName = apprentice.LastName,
                OrganisationName = command.Organisation,
                ApprenticeshipName = apprentice.CourseName,
            });

            return Unit.Value;
        }
    }
}