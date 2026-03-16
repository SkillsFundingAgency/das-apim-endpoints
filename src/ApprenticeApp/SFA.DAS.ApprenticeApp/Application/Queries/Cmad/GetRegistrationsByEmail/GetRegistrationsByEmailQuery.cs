using MediatR;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Cmad.GetRegistrationsByEmail
{    
    public class GetRegistrationsByEmailQuery : IRequest<GetRegistrationsByEmailQueryResult>
    {
        public string Email { get; set; }
    }
}
