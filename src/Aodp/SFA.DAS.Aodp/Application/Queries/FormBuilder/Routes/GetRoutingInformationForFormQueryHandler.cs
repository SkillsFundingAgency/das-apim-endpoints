using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Routes;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Routes
{
    public class GetRoutingInformationForFormQueryHandler : IRequestHandler<GetRoutingInformationForFormQuery, BaseMediatrResponse<GetRoutingInformationForFormQueryResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;

        public GetRoutingInformationForFormQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;

        }

        public async Task<BaseMediatrResponse<GetRoutingInformationForFormQueryResponse>> Handle(GetRoutingInformationForFormQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetRoutingInformationForFormQueryResponse>
            {
                Success = false
            };
            try
            {
                var info = await _apiClient.Get<GetRoutingInformationForFormQueryResponse>(new GetRoutesForFormVersionApiRequest()
                {
                    FormVersionId = request.FormVersionId,
                });

                response.Value = info;
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