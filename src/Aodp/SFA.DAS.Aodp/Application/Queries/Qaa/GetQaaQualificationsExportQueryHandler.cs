using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qaa;
using SFA.DAS.Aodp.Services;

namespace SFA.DAS.Aodp.Application.Queries.Qaa
{
    public class GetQaaQualificationsExportQueryHandler : IRequestHandler<GetQaaQualificationsExportQuery, BaseMediatrResponse<GetQaaQualificationsExportQueryResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public GetQaaQualificationsExportQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BaseMediatrResponse<GetQaaQualificationsExportQueryResponse>> Handle(GetQaaQualificationsExportQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetQaaQualificationsExportQueryResponse>();

            try
            {
                var result = await _apiClient.Get<GetQaaQualificationsExportQueryResponse>(new GetQaaQualificationsExportApiRequest
                {
                    CurrentUsername = request.CurrentUsername
                });

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
}
