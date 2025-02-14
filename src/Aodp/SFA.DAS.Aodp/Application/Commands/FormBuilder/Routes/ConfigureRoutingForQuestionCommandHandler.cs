using MediatR;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Routes;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Routes
{
    public class ConfigureRoutingForQuestionCommandHandler : IRequestHandler<ConfigureRoutingForQuestionCommand, BaseMediatrResponse<ConfigureRoutingForQuestionCommandResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


        public ConfigureRoutingForQuestionCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;

        }

        public async Task<BaseMediatrResponse<ConfigureRoutingForQuestionCommandResponse>> Handle(ConfigureRoutingForQuestionCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<ConfigureRoutingForQuestionCommandResponse>()
            {
                Success = false
            };

            try
            {
                var apiRequest = new ConfigureRoutingForQuestionApiRequest(request.QuestionId, request.PageId, request.FormVersionId, request.SectionId)
                {
                    Data = request
                };
                await _apiClient.Put(apiRequest);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.Success = false;
            }

            return response;
        }
    }
}
