using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeSubscriptions
{
    public class RemoveApprenticeArticleCommandHandler : IRequestHandler<RemoveApprenticeArticleCommand, Unit>
    {
        private readonly ILogger<RemoveApprenticeArticleCommandHandler> _logger;
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _accountsApiClient;

        public RemoveApprenticeArticleCommandHandler
        (
            ILogger<RemoveApprenticeArticleCommandHandler> logger,
            IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> accountsApiClient

        )
        {
            _logger = logger;
            _accountsApiClient = accountsApiClient;
        }

        public async Task<Unit> Handle(RemoveApprenticeArticleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "RemoveApprenticeArticleCommand Apprentice:{ApprenticeId}",
                request.Id);

            // send to apprentice accounts
            ApprenticeArticle apprenticeArticleData = new ApprenticeArticle()
            {
                LikeStatus = request.LikeStatus,
                IsSaved = request.IsSaved
            };

            await _accountsApiClient.Post(new PostRemoveApprenticeArticlesRequest(request.Id, request.EntryId){ Data = apprenticeArticleData });

            return Unit.Value;
        }
    }
}