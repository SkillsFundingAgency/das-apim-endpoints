using System;
using System.Collections.Generic;
using System.Text;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships
{
    public class GetApprenticeshipPriceRequest : IGetApiRequest
    {
        public Guid ApprenticeshipKey { get; set; }
        public string GetUrl => $"{ApprenticeshipKey}/price";
    }
}
