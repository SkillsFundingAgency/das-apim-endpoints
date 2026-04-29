using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;

using SFA.DAS.Apim.Shared.Extensions;

public class GetRelatedQualificationForApplicationQueryHandler : IRequestHandler<GetRelatedQualificationForApplicationQuery, BaseMediatrResponse<GetRelatedQualificationForApplicationQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public GetRelatedQualificationForApplicationQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<GetRelatedQualificationForApplicationQueryResponse>> Handle(GetRelatedQualificationForApplicationQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetRelatedQualificationForApplicationQueryResponse>();
        response.Success = false;
        try
        {
            var result = await _apiClient.GetWithResponseCode<GetRelatedQualificationForApplicationQueryResponse>(new GetRelatedQualificationForApplicationApiRequest()
            {
                ApplicationId = request.ApplicationId
            });
            result.EnsureSuccessStatusCode();
            response.Value = result.Body;
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}