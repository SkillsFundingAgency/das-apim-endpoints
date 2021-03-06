using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Commands.UpdateApplication
{
    public class UpdateApplicationCommandHandler : IRequestHandler<UpdateApplicationCommand>
    {
        private readonly ICommitmentsService _commitmentsService;
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public UpdateApplicationCommandHandler(ICommitmentsService commitmentsService, IEmployerIncentivesService employerIncentivesService)
        {
            _commitmentsService = commitmentsService;
            _employerIncentivesService = employerIncentivesService;
        }

        public async Task<Unit> Handle(UpdateApplicationCommand command, CancellationToken cancellationToken)
        {
            var apprenticeships = await _commitmentsService.GetApprenticeshipDetails(command.AccountId, command.ApprenticeshipIds);

            var request = CreateIncentiveApplicationRequest(command, apprenticeships);

            await _employerIncentivesService.UpdateIncentiveApplication(request);

            return Unit.Value;
        }

        private static UpdateIncentiveApplicationRequestData CreateIncentiveApplicationRequest(UpdateApplicationCommand command, IEnumerable<ApprenticeshipResponse> apprenticeships)
        {
            return new UpdateIncentiveApplicationRequestData
            {
                IncentiveApplicationId = command.ApplicationId,
                AccountId = command.AccountId,
                Apprenticeships = apprenticeships.Select(x => (IncentiveClaimApprenticeshipDto)x).ToArray()
            };
        }
    }
}