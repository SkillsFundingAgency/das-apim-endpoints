using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Routes;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Routes
{
    public class GetAvailableSectionsAndPagesForRoutingQueryHandler : IRequestHandler<GetAvailableSectionsAndPagesForRoutingQuery, BaseMediatrResponse<GetAvailableSectionsAndPagesForRoutingQueryResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


        public GetAvailableSectionsAndPagesForRoutingQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;

        }

        public async Task<BaseMediatrResponse<GetAvailableSectionsAndPagesForRoutingQueryResponse>> Handle(GetAvailableSectionsAndPagesForRoutingQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetAvailableSectionsAndPagesForRoutingQueryResponse>();
            response.Success = false;
            try
            {
                var sections = await _apiClient.Get<GetAvailableSectionsAndPagesForRoutingQueryResponse>(new GetAvailableSectionsAndPagesForRoutingApiRequest()
                {
                    FormVersionId = request.FormVersionId,
                });
                response.Value = sections;
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
