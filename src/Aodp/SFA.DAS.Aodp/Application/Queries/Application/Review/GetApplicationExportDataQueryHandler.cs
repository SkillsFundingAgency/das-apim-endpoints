using MediatR;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;

namespace SFA.DAS.Aodp.Application.Queries.Application.Review
{
    public class GetApplicationExportDataQueryHandler :IRequestHandler<GetApplicationExportDataQuery, BaseMediatrResponse<GetApplicationExportDataQueryResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public GetApplicationExportDataQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BaseMediatrResponse<GetApplicationExportDataQueryResponse>> Handle(GetApplicationExportDataQuery request, CancellationToken cancellationToken)
        {


            var response = new BaseMediatrResponse<GetApplicationExportDataQueryResponse>();
            response.Success = false;
            try
            {
                var result = await _apiClient.Get<GetApplicationExportDataQueryResponse>(
                    new GetApplicationExportDetailsApiRequest(request.ApplicationReviewId));

                response.Value = result;
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
