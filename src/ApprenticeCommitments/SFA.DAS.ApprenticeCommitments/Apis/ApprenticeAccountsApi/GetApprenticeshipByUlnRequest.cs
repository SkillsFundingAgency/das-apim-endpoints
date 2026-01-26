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
        public int Uln { get; set; }

        public GetApprenticeshipByUlnRequest(int uln)
        {
            Uln = uln;
        }

        public string GetUrl => $"apprentice/{Uln}";
    }
}
