using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.Application.ChangeHistory.Queries.GetAll;

public class GetAllChangeHistoryForEmployerQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient)
        : IRequestHandler<GetAllChangeHistoryForEmployerQuery, GetAllChangeHistoryForEmployerQueryResult>
{
    public async Task<GetAllChangeHistoryForEmployerQueryResult> Handle(GetAllChangeHistoryForEmployerQuery query, CancellationToken cancellationToken)
    {
        var changeHistory = await commitmentsV2ApiClient.Get<GetAllChangeHistoryForEmployerResponse>(new GetAllChangeHistoryForEmployerRequest(query.AccountId));
        return new GetAllChangeHistoryForEmployerQueryResult
        {
            ChangeHistory = changeHistory?.ChangeHistory ?? []
        };
    }
}