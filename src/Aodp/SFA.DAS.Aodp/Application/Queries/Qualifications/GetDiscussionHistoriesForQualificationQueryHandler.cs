using MediatR;
using SFA.DAS.AODP.Domain.Qualifications.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications;

public class GetDiscussionHistoriesForQualificationQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient) : IRequestHandler<GetDiscussionHistoriesForQualificationQuery, BaseMediatrResponse<GetDiscussionHistoriesForQualificationQueryResponse>>
{
    private readonly IAodpApiClient<AodpApiConfiguration> _apiClient = apiClient;

    public async Task<BaseMediatrResponse<GetDiscussionHistoriesForQualificationQueryResponse>> Handle(GetDiscussionHistoriesForQualificationQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseMediatrResponse<GetDiscussionHistoriesForQualificationQueryResponse>();
        try
        {
            var result = await _apiClient.Get<GetDiscussionHistoriesForQualificationQueryResponse>(new GetDiscussionHistoryForQualificationApiRequest(request.QualificationReference));
            if (result != null)
            {
                response.Value = result;
                response.Success = true;
            }
            else
            {
                response.Success = false;
                response.ErrorMessage = $"Failed to get qualification discussion history for qualification ref: {request.QualificationReference}";
            }
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = ex.Message;
        }
        return response;
    }
}
