using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetUkrlp;
public class GetUkrlpQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> _apiClient, ILogger<GetUkrlpQueryHandler> _logger) : IRequestHandler<GetUkrlpQuery, GetUkrlpQueryResult?>
{
    public async Task<GetUkrlpQueryResult?> Handle(GetUkrlpQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handle GetUkrlp request for Ukprn {Ukprn}", request.Ukprn);
        var response = await _apiClient.GetWithResponseCode<UkrlpLookupResponse>(new GetUkrlpRequest(request.Ukprn));

        response.EnsureSuccessStatusCode();

        if (response.Body.Results == null)
        {
            return null;
        }

        return response.Body.Results.Select(r => (GetUkrlpQueryResult)r).FirstOrDefault();
    }
}