using MediatR;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.EmployerProfiles;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.EmployerProfile.Queries.GetEmployerProfilesByAccountId;

public class GetEmployerProfilesByAccountIdQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) 
    : IRequestHandler<GetEmployerProfilesByAccountIdQuery, GetEmployerProfilesByAccountIdQueryResult>
{
    public async Task<GetEmployerProfilesByAccountIdQueryResult> Handle(GetEmployerProfilesByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.Get<List<InnerApi.Models.EmployerProfile>>(
            new GetEmployerProfilesByAccountIdApiRequest(request.AccountId));

        return new GetEmployerProfilesByAccountIdQueryResult
        {
            EmployerProfiles = response ?? []
        };
    }
}