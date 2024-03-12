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
    public class UpdateApplicationCommandHandler : IRequestHandler<UpdateApplicationCommand, Unit>
    {
        private readonly ICommitmentsService _commitmentsService;
        private readonly IApplicationService _applicationService;

        public UpdateApplicationCommandHandler(ICommitmentsService commitmentsService, IApplicationService applicationService)
        {
            _commitmentsService = commitmentsService;
            _applicationService = applicationService;
        }

        public async Task<Unit> Handle(UpdateApplicationCommand command, CancellationToken cancellationToken)
        {
            var apprenticeships = await _commitmentsService.GetApprenticeshipDetails(command.AccountId, command.ApprenticeshipIds);

            var request = CreateIncentiveApplicationRequest(command, apprenticeships);

            await _applicationService.Update(request);

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