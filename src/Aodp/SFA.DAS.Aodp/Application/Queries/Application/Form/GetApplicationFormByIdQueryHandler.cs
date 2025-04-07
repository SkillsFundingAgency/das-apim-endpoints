using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Application.Form;

public class GetApplicationFormByIdQueryHandler : IRequestHandler<GetApplicationFormByIdQuery, BaseMediatrResponse<GetApplicationFormByIdQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public GetApplicationFormByIdQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<GetApplicationFormByIdQueryResponse>> Handle(GetApplicationFormByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetApplicationFormByIdQueryResponse>();
        response.Success = false;
        try
        {
            var result = await _apiClient.Get<GetApplicationFormByIdQueryResponse>(new GetApplicationFormByIdApiRequest()
            {
                FormVersionId = request.FormVersionId,
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