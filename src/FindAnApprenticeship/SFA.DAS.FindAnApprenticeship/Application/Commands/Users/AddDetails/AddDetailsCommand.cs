using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.AddDetails
{
    public class AddDetailsCommand : IRequest<Unit>
    {
        public Guid CandidateId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
