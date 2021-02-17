using MediatR;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Application.Commands.AddJobRequest;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetLegalEntities
{
    public class RefreshLegalEntitiesCommandHandler : IRequestHandler<RefreshLegalEntitiesCommand>
    {
        private readonly IAccountsService _accountsService;
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public RefreshLegalEntitiesCommandHandler(IAccountsService accountsService, IEmployerIncentivesService employerIncentivesService)
        {
            _accountsService = accountsService;
            _employerIncentivesService = employerIncentivesService;
        }

        public async Task<Unit> Handle(RefreshLegalEntitiesCommand request, CancellationToken cancellationToken)
        {
            var response = await _accountsService.GetLegalEntitiesByPage(request.PageNumber, request.PageSize);

            await _employerIncentivesService.RefreshLegalEntities(response.Data, response.Page, request.PageSize, response.TotalPages);

            return Unit.Value;
        }
    }
}
