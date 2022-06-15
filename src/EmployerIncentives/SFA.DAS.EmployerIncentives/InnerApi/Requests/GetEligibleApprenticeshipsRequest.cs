using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class GetEligibleApprenticeshipsRequest : IGetApiRequest
    {
        private readonly long _uln;
        private readonly DateTime _startDate;

        public GetEligibleApprenticeshipsRequest(long uln, DateTime startDate)
        {
            _uln = uln;
            _startDate = startDate;
        }

        public string GetUrl => $"eligible-apprenticeships/{_uln}?startDate={_startDate:yyyy-MM-dd}&isApproved=true";
    }
}