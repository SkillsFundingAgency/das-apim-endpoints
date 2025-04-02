using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.Application.Review
{
    public class SaveSkillsEnglandReviewOutcomeCommandHandler : IRequestHandler<SaveSkillsEnglandReviewOutcomeCommand, BaseMediatrResponse<EmptyResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;


        public SaveSkillsEnglandReviewOutcomeCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<BaseMediatrResponse<EmptyResponse>> Handle(SaveSkillsEnglandReviewOutcomeCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<EmptyResponse>()
            {
                Success = false
            };

            try
            {
                var apiRequest = new SaveSkillsEnglandReviewOutcomeApiRequest(request.ApplicationReviewId)
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