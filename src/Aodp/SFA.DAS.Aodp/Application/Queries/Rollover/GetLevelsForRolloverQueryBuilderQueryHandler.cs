using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.Aodp.Services;

namespace SFA.DAS.Aodp.Application.Queries.Rollover;

public class GetLevelsForRolloverQueryBuilderQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    : IRequestHandler<GetLevelsForRolloverQueryBuilderQuery, BaseMediatrResponse<GetLevelsForRolloverQueryBuilderQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient = apiClient;

    public async Task<BaseMediatrResponse<GetLevelsForRolloverQueryBuilderQueryResponse>> Handle(
        GetLevelsForRolloverQueryBuilderQuery request,
        CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetLevelsForRolloverQueryBuilderQueryResponse>();

        try
        {
            var result = await _apiClient.Get<GetLevelsForRolloverQueryBuilderQueryResponse>(new GetLevelsForRolloverQueryBuilderApiRequest());

            response.Value = result;
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