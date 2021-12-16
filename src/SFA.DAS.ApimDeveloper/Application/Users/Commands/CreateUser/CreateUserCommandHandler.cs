using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.Interfaces;
using SFA.DAS.ApimDeveloper.Models;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApimDeveloper.Application.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Unit>
    {
        private readonly IApimDeveloperApiClient<ApimDeveloperApiConfiguration> _apimDeveloperApiClient;
        private readonly INotificationService _notificationService;
        private readonly ApimDeveloperMessagingConfiguration _config;

        public CreateUserCommandHandler (
            IApimDeveloperApiClient<ApimDeveloperApiConfiguration> apimDeveloperApiClient, 
            IOptions<ApimDeveloperMessagingConfiguration> options,
            INotificationService notificationService)
        {
            _apimDeveloperApiClient = apimDeveloperApiClient;
            _notificationService = notificationService;
            _config = options.Value;
        }
        public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var actual = await _apimDeveloperApiClient.PostWithResponseCode<object>(new PostCreateUserRequest(request.Id, new UserRequestData
            {
                Email = request.Email,
                Password = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName,
                ConfirmationEmailLink = request.ConfirmationEmailLink,
                State = 0
            }));

            if (actual.StatusCode != HttpStatusCode.Created)
            {
                throw new HttpRequestContentException("Error creating user", actual.StatusCode, actual.ErrorContent);
            }

            var email = new VerifyThirdyPartyAccountEmail(request, _config);
            var command = new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens);
            await _notificationService.Send(command);
            
            return Unit.Value;
        }
    }
}