using MediatR;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Cmad.GetRegistrationsByAccountDetails
{
    public  class GetRegistrationsByAccountDetailsQuery : IRequest<GetRegistrationsByAccountDetailsQueryResult>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
