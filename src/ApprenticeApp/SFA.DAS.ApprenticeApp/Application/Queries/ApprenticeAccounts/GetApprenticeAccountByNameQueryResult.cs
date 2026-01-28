using SFA.DAS.ApprenticeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeAccounts
{
    public class GetApprenticeAccountByNameQueryResult
    {
        public List<Apprentice> Apprentices { get; set; }
    }
}
