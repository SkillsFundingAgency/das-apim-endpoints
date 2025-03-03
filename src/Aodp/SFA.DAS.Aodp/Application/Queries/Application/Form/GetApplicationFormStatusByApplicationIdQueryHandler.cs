using MediatR;
using SFA.DAS.Aodp.Application;
using SFA.DAS.SharedOuterApi.Configuration;using SFA.DAS.SharedOuterApi.Interfaces;

public class GetApplicationFormStatusByApplicationIdQueryHandler : IRequestHandler<GetApplicationFormStatusByApplicationIdQuery, BaseMediatrResponse<GetApplicationFormStatusByApplicationIdQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public GetApplicationFormStatusByApplicationIdQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<GetApplicationFormStatusByApplicationIdQueryResponse>> Handle(GetApplicationFormStatusByApplicationIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetApplicationFormStatusByApplicationIdQueryResponse>();
        response.Success = false;
        try
        {
            var result = await _apiClient.Get<GetApplicationFormStatusByApplicationIdQueryResponse>(new GetApplicationFormStatusByApplicationIdApiRequest()
            {
                FormVersionId = request.FormVersionId,
                ApplicationId = request.ApplicationId
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