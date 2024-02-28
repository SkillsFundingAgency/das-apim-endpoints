using MediatR;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetLegalEntities
{
    public class RefreshLegalEntitiesCommandHandler : IRequestHandler<RefreshLegalEntitiesCommand, Unit>
    {
        private readonly IAccountsService _accountsService;
        private readonly ILegalEntitiesService _legalEntitiesService;

        public RefreshLegalEntitiesCommandHandler(IAccountsService accountsService, ILegalEntitiesService legalEntitiesService)
        {
            _accountsService = accountsService;
            _legalEntitiesService = legalEntitiesService;
        }

        public async Task<Unit> Handle(RefreshLegalEntitiesCommand request, CancellationToken cancellationToken)
        {
            var response = await _accountsService.GetLegalEntitiesByPage(request.PageNumber, request.PageSize);

            await _legalEntitiesService.RefreshLegalEntities(response.Data, response.Page, request.PageSize, response.TotalPages);

            return Unit.Value;
        }
    }
}
