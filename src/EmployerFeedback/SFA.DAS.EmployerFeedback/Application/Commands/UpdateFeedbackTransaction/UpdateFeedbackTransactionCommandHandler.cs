using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFeedback.Application.Commands.UpdateFeedbackTransaction
{
    public class UpdateFeedbackTransactionCommandHandler : IRequestHandler<UpdateFeedbackTransactionCommand>
    {
        private readonly IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> _apiClient;
        private readonly ILogger<UpdateFeedbackTransactionCommandHandler> _logger;

        public UpdateFeedbackTransactionCommandHandler(
            IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> apiClient,
            ILogger<UpdateFeedbackTransactionCommandHandler> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task Handle(UpdateFeedbackTransactionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing UpdateFeedbackTransaction for ID: {FeedbackTransactionId}", request.Id);

            var data = new UpdateFeedbackTransactionData
            {
                TemplateId = request.Request.TemplateId,
                SentCount = request.Request.SentCount,
                SentDate = request.Request.SentDate
            };

            var apiRequest = new UpdateFeedbackTransactionRequest(request.Id, data);
            await _apiClient.Put(apiRequest);

            _logger.LogInformation("Successfully updated feedback transaction {FeedbackTransactionId}", request.Id);
        }
    }
}
