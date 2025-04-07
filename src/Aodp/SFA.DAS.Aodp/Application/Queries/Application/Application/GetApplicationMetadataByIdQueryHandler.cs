using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Application.Application;

public class GetApplicationMetadataByIdQueryHandler : IRequestHandler<GetApplicationMetadataByIdQuery, BaseMediatrResponse<GetApplicationMetadataByIdQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public GetApplicationMetadataByIdQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<GetApplicationMetadataByIdQueryResponse>> Handle(GetApplicationMetadataByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetApplicationMetadataByIdQueryResponse>();
        response.Success = false;
        try
        {
            var result = await _apiClient.Get<GetApplicationMetadataByIdQueryResponse>(new GetApplicationMetadataByIdRequest()
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
