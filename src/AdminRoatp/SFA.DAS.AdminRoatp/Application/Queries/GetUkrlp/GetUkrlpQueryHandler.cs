using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetUkrlp;

public class GetUkrlpQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> _apiClient, ILogger<GetUkrlpQueryHandler> _logger) : IRequestHandler<GetUkrlpQuery, GetUkrlpQueryResult?>
{
    public async Task<GetUkrlpQueryResult?> Handle(GetUkrlpQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handle GetUkrlp request for Ukprn {Ukprn}", request.Ukprn);
        var response = await _apiClient.GetWithResponseCode<UkrlpProvidersResponse>(new GetUkrlpRequest(request.Ukprn));

        response.EnsureSuccessStatusCode();

        if (response.Body.Providers == null)
        {
            return null;
        }

        return response.Body.Providers.Select(r => (GetUkrlpQueryResult)r).FirstOrDefault();
    }
}