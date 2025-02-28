using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetNewQualificationsQueryHandler : IRequestHandler<GetNewQualificationsQuery, BaseMediatrResponse<GetNewQualificationsQueryResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public GetNewQualificationsQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BaseMediatrResponse<GetNewQualificationsQueryResponse>> Handle(GetNewQualificationsQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetNewQualificationsQueryResponse>();
            response.Success = false;
            try
            {
                var result = await _apiClient.Get<BaseMediatrResponse<GetNewQualificationsQueryResponse>>(new GetNewQualificationsApiRequest());
                if (result != null && result.Value != null)
                {
                    response.Value = result.Value;
                    response.Success = true;
                }
                else
                {
                    response.Success = false;
                    response.ErrorMessage = "No new qualifications found.";
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }
}
