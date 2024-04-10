using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.CreateAccount.PhoneNumber
{
    public class GetPhoneNumberQuery : IRequest<GetPhoneNumberQueryResult>
    {
        public Guid CandidateId { get; set; }
    }
}
