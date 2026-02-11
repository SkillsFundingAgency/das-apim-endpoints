using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Apis.ApprenticeAccountsApi
{
    public class GetApprenticeshipByUlnRequest : IGetApiRequest
    {
        public string Uln { get; set; }

        public GetApprenticeshipByUlnRequest(string uln)
        {
            Uln = uln;
        }

        public string GetUrl => $"apprentice/{Uln}";
    }
}
