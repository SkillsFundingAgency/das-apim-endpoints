using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.Aodp.Services;

namespace SFA.DAS.Aodp.Application.Queries.Rollover;

public class GetTypesForRolloverQueryBuilderQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    : IRequestHandler<GetTypesForRolloverQueryBuilderQuery, BaseMediatrResponse<GetTypesForRolloverQueryBuilderQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient = apiClient;

    public async Task<BaseMediatrResponse<GetTypesForRolloverQueryBuilderQueryResponse>> Handle(
        GetTypesForRolloverQueryBuilderQuery request,
        CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetTypesForRolloverQueryBuilderQueryResponse>();

        try
        {
            var result = await _apiClient.PostWithResponseCode<GetTypesForRolloverQueryBuilderQueryResponse>(new GetTypesForRolloverQueryBuilderApiRequest(request.Filters));

            response.Value = result.Body;
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