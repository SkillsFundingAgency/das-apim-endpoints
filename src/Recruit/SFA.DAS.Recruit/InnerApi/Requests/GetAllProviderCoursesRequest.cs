using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.InnerApi.Requests
{
    public class GetAllProviderCoursesRequest : IGetApiRequest
    {
        private readonly int _ukprn;

        public GetAllProviderCoursesRequest(int ukprn)
        {
            _ukprn = ukprn;
        }

        public string GetUrl => $"api/providers/{_ukprn}/courses";
    }
}
