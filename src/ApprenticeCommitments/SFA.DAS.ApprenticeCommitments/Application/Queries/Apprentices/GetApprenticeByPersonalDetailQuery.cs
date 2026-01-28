using MediatR;
using SFA.DAS.ApprenticeCommitments.Apis.ApprenticeAccountsApi;
using System;
using System.Collections.Generic;


namespace SFA.DAS.ApprenticeCommitments.Application.Queries.Apprentices
{
    public class GetApprenticeByPersonalDetailQuery : IRequest<GetApprenticeByPersonalDetailQueryResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
