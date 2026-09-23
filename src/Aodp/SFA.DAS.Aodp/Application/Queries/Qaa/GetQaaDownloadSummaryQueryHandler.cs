using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qaa;
using SFA.DAS.Aodp.Services;

namespace SFA.DAS.Aodp.Application.Queries.Qaa
{
    public class GetQaaDownloadSummaryQueryHandler : IRequestHandler<GetQaaDownloadSummaryQuery, BaseMediatrResponse<GetQaaDownloadSummaryQueryResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public GetQaaDownloadSummaryQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BaseMediatrResponse<GetQaaDownloadSummaryQueryResponse>> Handle(GetQaaDownloadSummaryQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetQaaDownloadSummaryQueryResponse>();

            try
            {
                var result = await _apiClient.Get<GetQaaDownloadSummaryQueryResponse>(new GetQaaDownloadSummaryApiRequest());

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
