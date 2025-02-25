using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Application.Form;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Application.Application;

public class GetApplicationFormPreviewByIdQueryHandler : IRequestHandler<GetApplicationFormPreviewByIdQuery, BaseMediatrResponse<GetApplicationFormPreviewByIdQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public GetApplicationFormPreviewByIdQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<GetApplicationFormPreviewByIdQueryResponse>> Handle(GetApplicationFormPreviewByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetApplicationFormPreviewByIdQueryResponse>();
        response.Success = false;
        try
        {
            var result = await _apiClient.Get<GetApplicationFormPreviewByIdQueryResponse>(new GetApplicationFormPreviewApiRequest(request.ApplicationId)
            {
                ApplicationId = request.ApplicationId,
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