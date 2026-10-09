using MediatR;
using SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;
using SFA.DAS.EmployerFinance.InnerApi.Responses.Finance;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetLastLevyDeclarationDate;

public class GetLastLevyDeclarationDateQueryHandler(IFinanceApiClient<FinanceApiConfiguration> financeApiClient)
    : IRequestHandler<GetLastLevyDeclarationDateQuery, GetLastLevyDeclarationDateQueryResult>
{
    public async Task<GetLastLevyDeclarationDateQueryResult> Handle(GetLastLevyDeclarationDateQuery request, CancellationToken cancellationToken)
    {
        var response = await financeApiClient.Get<GetLevyLastSubmissionDateResponse>(
            new GetLevyLastSubmissionDateRequest(request.AccountId));

        return new GetLastLevyDeclarationDateQueryResult(response.LastSubmissionDate);
    }
}