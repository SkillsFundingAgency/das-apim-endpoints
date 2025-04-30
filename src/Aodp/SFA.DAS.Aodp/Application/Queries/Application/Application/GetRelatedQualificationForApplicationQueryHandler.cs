using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

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