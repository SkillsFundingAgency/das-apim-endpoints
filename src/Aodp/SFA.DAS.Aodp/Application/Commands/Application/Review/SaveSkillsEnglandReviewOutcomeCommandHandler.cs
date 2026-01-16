using MediatR;
using SFA.DAS.Aodp.Application.Commands.Application.Application;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.Application.Review
{
    public class SaveSkillsEnglandReviewOutcomeCommandHandler : IRequestHandler<SaveSkillsEnglandReviewOutcomeCommand, BaseMediatrResponse<EmptyResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
        private readonly IEmailService _emailService;


        public SaveSkillsEnglandReviewOutcomeCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient, IEmailService service)
        {
            _apiClient = apiClient;
            _emailService = service;
        }

        public async Task<BaseMediatrResponse<EmptyResponse>> Handle(SaveSkillsEnglandReviewOutcomeCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<EmptyResponse>()
            {
                Success = false
            };

            try
            {
                var result = await _apiClient.PutWithResponseCode<SaveSkillsEnglandReviewOutcomeCommandResponse>(
                    new SaveSkillsEnglandReviewOutcomeApiRequest(request.ApplicationReviewId)
                    {
                        Data = request
                    });

                await _emailService.SendAsync(result.Body?.Notifications);

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