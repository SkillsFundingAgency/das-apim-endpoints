using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Commands.CreateApplication
{
    public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, Guid>
    {
        private readonly ICommitmentsService _commitmentsService;
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public CreateApplicationCommandHandler(ICommitmentsService commitmentsService, IEmployerIncentivesService employerIncentivesService)
        {
            _commitmentsService = commitmentsService;
            _employerIncentivesService = employerIncentivesService;
        }

        public async Task<Guid> Handle(CreateApplicationCommand command, CancellationToken cancellationToken)
        {
            var apprenticeships = await _commitmentsService.GetApprenticeshipDetails(command.AccountId, command.ApprenticeshipIds);

            var request = CreateIncentiveApplicationRequest(command, apprenticeships);

            await _employerIncentivesService.CreateIncentiveApplication(request);

            return command.ApplicationId;
        }

        private CreateIncentiveApplication CreateIncentiveApplicationRequest(CreateApplicationCommand command, IEnumerable<ApprenticeshipResponse> apprenticeships)
        {
            return new CreateIncentiveApplication
            {
                IncentiveApplicationId = command.ApplicationId,
                AccountId = command.AccountId,
                AccountLegalEntityId = command.AccountLegalEntityId,
                Apprenticeships = apprenticeships.Select(x=>(IncentiveClaimApprenticeshipDto)x).ToArray()
            };
        }
    }
}