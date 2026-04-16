using MediatR;
using SFA.DAS.Aodp.Application.Commands.Application.Application;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Commands.Application.Review
{
    public class SaveOfqualReviewOutcomeCommandHandler : IRequestHandler<SaveOfqualReviewOutcomeCommand, BaseMediatrResponse<EmptyResponse>>
    {
        private readonly IAodpApiClient<AodpApiConfiguration> _apiClient;
        private readonly IEmailService _emailService;


        public SaveOfqualReviewOutcomeCommandHandler(IAodpApiClient<AodpApiConfiguration> apiClient, IEmailService emailService)
        {
            _apiClient = apiClient;
            _emailService = emailService;
        }

        public async Task<BaseMediatrResponse<EmptyResponse>> Handle(SaveOfqualReviewOutcomeCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseMediatrResponse<EmptyResponse>()
            {
                Success = false
            };

            try
            {
                var result = await _apiClient.PutWithResponseCode<SaveOfqualReviewOutcomeCommandResponse>(
                    new SaveOfqualReviewOutcomeApiRequest(request.ApplicationReviewId)
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