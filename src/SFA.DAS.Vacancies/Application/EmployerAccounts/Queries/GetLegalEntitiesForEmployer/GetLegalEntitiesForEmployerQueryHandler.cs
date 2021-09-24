using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.Vacancies.Application.EmployerAccounts.Queries.GetLegalEntitiesForEmployer
{
    public class GetLegalEntitiesForEmployerQueryHandler : IRequestHandler<GetLegalEntitiesForEmployerQuery, GetLegalEntitiesForEmployerResult>
    {
        public async Task<GetLegalEntitiesForEmployerResult> Handle(GetLegalEntitiesForEmployerQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}