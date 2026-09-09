using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.Aodp.Services;

namespace SFA.DAS.Aodp.Application.Queries.Rollover;

public class GetSectorSubjectAreasForRolloverQueryBuilderQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    : IRequestHandler<GetSectorSubjectAreaForRolloverQueryBuilderQuery, BaseMediatrResponse<GetSectorSubjectAreaForRolloverQueryBuilderQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient = apiClient;

    public async Task<BaseMediatrResponse<GetSectorSubjectAreaForRolloverQueryBuilderQueryResponse>> Handle(
        GetSectorSubjectAreaForRolloverQueryBuilderQuery request,
        CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetSectorSubjectAreaForRolloverQueryBuilderQueryResponse>();

        try
        {
            var result = await _apiClient.PostWithResponseCode<GetSectorSubjectAreaForRolloverQueryBuilderQueryResponse>(new GetSectorSubjectAreaForRolloverQueryBuilderApiRequest(request.Filters));

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