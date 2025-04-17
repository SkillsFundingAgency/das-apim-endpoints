using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Application.Messages;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Application.Application;

public class GetApplicationMessageByIdQueryHandler : IRequestHandler<GetApplicationMessageByIdQuery, BaseMediatrResponse<GetApplicationMessageByIdQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public GetApplicationMessageByIdQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<GetApplicationMessageByIdQueryResponse>> Handle(GetApplicationMessageByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetApplicationMessageByIdQueryResponse>();
        response.Success = false;
        try
        {
            var result = await _apiClient.Get<GetApplicationMessageByIdQueryResponse>(new GetApplicationMessageByIdApiRequest()
            {
                MessageId = request.MessageId,

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
