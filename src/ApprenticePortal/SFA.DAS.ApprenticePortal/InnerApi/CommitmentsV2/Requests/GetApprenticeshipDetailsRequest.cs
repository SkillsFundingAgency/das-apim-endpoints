using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticePortal.InnerApi.CommitmentsV2.Requests
{
    public class GetApprenticeshipDetailsRequest : IGetApiRequest
    {
        private readonly long _apprenticeshipId;

        public GetApprenticeshipDetailsRequest(long apprenticeshipId)
        {
            _apprenticeshipId = apprenticeshipId;
        }

        public string GetUrl => $"api/apprenticeships/{_apprenticeshipId}";
    }
}