using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Cmad.GetRegistrationsByEmail
{
    public class GetRegistrationsByEmailQuery : IRequest<GetRegistrationsByEmailQueryResult>
    {
        public string Email { get; set; }
    }
}
