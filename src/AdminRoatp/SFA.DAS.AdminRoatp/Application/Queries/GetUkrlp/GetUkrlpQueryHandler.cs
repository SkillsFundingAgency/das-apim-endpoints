using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetUkrlp;
public class GetUkrlpQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> _apiClient) : IRequestHandler<GetUkrlpQuery, GetUkrlpQueryResult?>
{
    public async Task<GetUkrlpQueryResult?> Handle(GetUkrlpQuery request, CancellationToken cancellationToken)
    {
        var response = await _apiClient.GetWithResponseCode<UkprnLookupResponse>(new GetUkrlpRequest(request.Ukprn));

        if (response.Body?.Results == null)
        {
            return null;
        }

        return response.Body.Results.Select(r => (GetUkrlpQueryResult)r).FirstOrDefault();
    }
}