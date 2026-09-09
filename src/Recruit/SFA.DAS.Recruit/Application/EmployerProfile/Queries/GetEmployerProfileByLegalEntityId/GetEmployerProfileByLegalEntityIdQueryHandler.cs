using MediatR;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.EmployerProfiles;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Recruit.Application.EmployerProfile.Queries.GetEmployerProfileByLegalEntityId;

public class GetEmployerProfileByLegalEntityIdQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient)
    : IRequestHandler<GetEmployerProfileByLegalEntityIdQuery,
        GetEmployerProfileByLegalEntityIdQueryResult>
{
    public async Task<GetEmployerProfileByLegalEntityIdQueryResult> Handle(
        GetEmployerProfileByLegalEntityIdQuery request, CancellationToken cancellationToken)
    {
        var response =
            await recruitApiClient.Get<InnerApi.Models.EmployerProfile>(
                new GetEmployerProfileByLegalEntityIdApiRequest(request.AccountLegalEntityId));

        return new GetEmployerProfileByLegalEntityIdQueryResult(
            EmployerProfile: response);
    }
}
