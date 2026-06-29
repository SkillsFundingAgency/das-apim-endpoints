using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.Aodp.Services;

namespace SFA.DAS.Aodp.Application.Queries.Rollover;

public class GetQualificationVersionsForRolloverQueryBuilderQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    : IRequestHandler<GetQualificationVersionsForRolloverQueryBuilderQuery, BaseMediatrResponse<GetQualificationVersionsForRolloverQueryBuilderQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient = apiClient;

    public async Task<BaseMediatrResponse<GetQualificationVersionsForRolloverQueryBuilderQueryResponse>> Handle(
        GetQualificationVersionsForRolloverQueryBuilderQuery request,
        CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetQualificationVersionsForRolloverQueryBuilderQueryResponse>();

        try
        {
            var result = await _apiClient.PostWithResponseCode<GetQualificationVersionsForRolloverQueryBuilderQueryResponse>(
                new GetQualificationVersionsForRolloverQueryBuilderApiRequest(request.Filters));

            response.Value = result.Body ?? new GetQualificationVersionsForRolloverQueryBuilderQueryResponse();
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}
