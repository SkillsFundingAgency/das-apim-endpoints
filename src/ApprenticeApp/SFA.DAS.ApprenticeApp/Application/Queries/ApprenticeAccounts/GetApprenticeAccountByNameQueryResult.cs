using SFA.DAS.ApprenticeApp.Models;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeAccounts
{
    public class GetApprenticeAccountByNameQueryResult
    {
        public List<Apprentice> Apprentices { get; set; }
    }
}
