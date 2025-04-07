namespace SFA.DAS.Aodp.Application.Queries.Application.Form;

using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

public class GetApplicationFormsQueryHandler : IRequestHandler<GetApplicationFormsQuery, BaseMediatrResponse<GetApplicationFormsQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public GetApplicationFormsQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<GetApplicationFormsQueryResponse>> Handle(GetApplicationFormsQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetApplicationFormsQueryResponse>();
        response.Success = false;
        try
        {
            var result = await _apiClient.Get<GetApplicationFormsQueryResponse>(new GetApplicationFormsApiRequest());
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