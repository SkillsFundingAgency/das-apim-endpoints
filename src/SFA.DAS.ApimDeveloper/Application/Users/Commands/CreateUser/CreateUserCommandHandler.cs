using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.Interfaces;

namespace SFA.DAS.ApimDeveloper.Application.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Unit>
    {
        private readonly IApimDeveloperApiClient<ApimDeveloperApiConfiguration> _apimDeveloperApiClient;

        public CreateUserCommandHandler (IApimDeveloperApiClient<ApimDeveloperApiConfiguration> apimDeveloperApiClient)
        {
            _apimDeveloperApiClient = apimDeveloperApiClient;
        }
        public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await _apimDeveloperApiClient.Put(new PutCreateUserRequest(request.Id, new PutCreateUserRequestData
            {
                Email = request.Email,
                Password = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName,
                ConfirmationEmailLink = request.ConfirmationEmailLink,
                State = 0
            }));
            
            return Unit.Value;
        }
    }
}