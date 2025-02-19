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
                response.Value = result.Value;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }
}
