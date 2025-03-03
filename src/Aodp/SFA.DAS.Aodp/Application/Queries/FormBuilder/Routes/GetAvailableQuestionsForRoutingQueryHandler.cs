using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Routes;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Routes
{
    public class GetAvailableQuestionsForRoutingQueryHandler : IRequestHandler<GetAvailableQuestionsForRoutingQuery, BaseMediatrResponse<GetAvailableQuestionsForRoutingQueryResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


        public GetAvailableQuestionsForRoutingQueryHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;

        }

        public async Task<BaseMediatrResponse<GetAvailableQuestionsForRoutingQueryResponse>> Handle(GetAvailableQuestionsForRoutingQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<GetAvailableQuestionsForRoutingQueryResponse>();
            response.Success = false;
            try
            {
                var questions = await _apiClient.Get<GetAvailableQuestionsForRoutingQueryResponse>(new GetAvailableQuestionsForRoutingApiRequest()
                {
                    FormVersionId = request.FormVersionId,
                    PageId = request.PageId,
                    SectionId = request.SectionId,
                });

                response.Value = questions;
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
