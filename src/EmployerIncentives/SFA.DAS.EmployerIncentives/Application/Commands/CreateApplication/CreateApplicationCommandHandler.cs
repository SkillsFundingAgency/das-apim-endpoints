using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Commands.CreateApplication
{
    public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand>
    {
        private readonly ICommitmentsService _commitmentsService;
        private readonly IApplicationService _applicationService;

        public CreateApplicationCommandHandler(ICommitmentsService commitmentsService, IApplicationService applicationService)
        {
            _commitmentsService = commitmentsService;
            _applicationService = applicationService;
        }

        public async Task<Unit> Handle(CreateApplicationCommand command, CancellationToken cancellationToken)
        {
            var apprenticeships = await _commitmentsService.GetApprenticeshipDetails(command.AccountId, command.ApprenticeshipIds);

            var request = CreateIncentiveApplicationRequest(command, apprenticeships);

            await _applicationService.Create(request);

            return Unit.Value;
        }

        private static CreateIncentiveApplicationRequestData CreateIncentiveApplicationRequest(CreateApplicationCommand command, IEnumerable<ApprenticeshipResponse> apprenticeships)
        {
            return new CreateIncentiveApplicationRequestData
            {
                IncentiveApplicationId = command.ApplicationId,
                AccountId = command.AccountId,
                AccountLegalEntityId = command.AccountLegalEntityId,
                Apprenticeships = apprenticeships.Select(x => (IncentiveClaimApprenticeshipDto)x).ToArray()
            };
        }
    }
}