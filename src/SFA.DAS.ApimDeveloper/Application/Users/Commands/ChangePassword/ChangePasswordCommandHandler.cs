using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.Interfaces;

namespace SFA.DAS.ApimDeveloper.Application.Users.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        private readonly IApimDeveloperApiClient<ApimDeveloperApiConfiguration> _apimDeveloperApiClient;

        public ChangePasswordCommandHandler(IApimDeveloperApiClient<ApimDeveloperApiConfiguration> apimDeveloperApiClient)
        {
            _apimDeveloperApiClient = apimDeveloperApiClient;
        }
        
        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            await _apimDeveloperApiClient.Put(new PutUpdateUserRequest(request.Id, new UserRequestData
            {
                Password = request.Password
            }));
            
            return Unit.Value;
        }
    }
}