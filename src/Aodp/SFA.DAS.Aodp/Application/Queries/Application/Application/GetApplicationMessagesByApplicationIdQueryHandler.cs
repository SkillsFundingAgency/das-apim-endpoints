using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Application.Messages;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Application.Application;

public class GetApplicationMessagesByApplicationIdQueryHandler : IRequestHandler<GetApplicationMessagesByApplicationIdQuery, BaseMediatrResponse<GetApplicationMessagesByApplicationIdQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public GetApplicationMessagesByApplicationIdQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<GetApplicationMessagesByApplicationIdQueryResponse>> Handle(GetApplicationMessagesByApplicationIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetApplicationMessagesByApplicationIdQueryResponse>();
        response.Success = false;
        try
        {
            var result = await _apiClient.Get<GetApplicationMessagesByApplicationIdQueryResponse>(new GetApplicationMessagesByApplicationIdApiRequest()
            {
                ApplicationId = request.ApplicationId,
                UserType = request.UserType,
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
