using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications;

public class GetMatchingQualificationsQueryHandler : IRequestHandler<GetMatchingQualificationsQuery, BaseMediatrResponse<GetMatchingQualificationsQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

    public GetMatchingQualificationsQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BaseMediatrResponse<GetMatchingQualificationsQueryResponse>> Handle(GetMatchingQualificationsQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetMatchingQualificationsQueryResponse>();
        response.Success = false;

        try
        {
            var result = await _apiClient.Get<GetMatchingQualificationsQueryResponse>(new GetMatchingQualificationsQueryApiRequest(request.SearchTerm, request.Skip, request.Take));

            if (result?.Qualifications != null)
            {
                response.Value.Qualifications = result.Qualifications;
                response.Value.TotalRecords = result.TotalRecords;
                response.Value.Skip = result.Skip;
                response.Value.Take = result.Take;
                response.Success = true;
                return response;
            }
            response.Success = true;
            response.ErrorMessage = "No matching qualifications found.";
            response.Value.Qualifications = new List<GetMatchingQualificationsQueryItem>();
            return response;
        }
        catch (Exception ex)
        {

            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}
