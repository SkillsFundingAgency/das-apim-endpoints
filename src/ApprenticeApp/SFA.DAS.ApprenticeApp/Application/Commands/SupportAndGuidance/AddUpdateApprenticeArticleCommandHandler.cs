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
    public class AddUpdateApprenticeArticleCommandHandler : IRequestHandler<AddUpdateApprenticeArticleCommand, Unit>
    {
        private readonly ILogger<AddUpdateApprenticeArticleCommandHandler> _logger;
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _accountsApiClient;

        public AddUpdateApprenticeArticleCommandHandler
        (
            ILogger<AddUpdateApprenticeArticleCommandHandler> logger,
            IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> accountsApiClient

        )
        {
            _logger = logger;
            _accountsApiClient = accountsApiClient;
        }

        public async Task<Unit> Handle(AddUpdateApprenticeArticleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "AddUpdateApprenticeArticleCommand Apprentice:{ApprenticeId}",
                request.Id);

            // send to apprentice accounts
            ApprenticeArticle apprenticeArticleData = new ApprenticeArticle()
            {
                LikeStatus = request.LikeStatus,
                IsSaved = request.IsSaved,
                EntryTitle = request.EntryTitle
            };

            await _accountsApiClient.Post(new PostApprenticeArticlesRequest(request.Id, request.EntryId, request.EntryTitle){ Data = apprenticeArticleData });

            return Unit.Value;
        }
    }
}