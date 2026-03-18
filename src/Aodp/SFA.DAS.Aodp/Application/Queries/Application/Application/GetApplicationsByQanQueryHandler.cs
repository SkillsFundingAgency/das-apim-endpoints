using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Application.Applications;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Application.Application;

public class GetApplicationsByQanQueryHandler : IRequestHandler<GetApplicationsByQanQuery, BaseMediatrResponse<GetApplicationsByQanQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public GetApplicationsByQanQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<GetApplicationsByQanQueryResponse>> Handle(GetApplicationsByQanQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetApplicationsByQanQueryResponse>();
        response.Success = false;
        try
        {
            var result = await _apiClient.Get<GetApplicationsByQanQueryResponse>(new GetApplicationsByQanApiRequest()
            {
                Qan = request.Qan
            });
            response.Value = result;
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}