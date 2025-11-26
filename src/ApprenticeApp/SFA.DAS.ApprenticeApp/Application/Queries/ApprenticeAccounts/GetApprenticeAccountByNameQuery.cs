using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeAccounts
{
    public class GetApprenticeAccountByNameQuery : IRequest<GetApprenticeAccountByNameQueryResult>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
