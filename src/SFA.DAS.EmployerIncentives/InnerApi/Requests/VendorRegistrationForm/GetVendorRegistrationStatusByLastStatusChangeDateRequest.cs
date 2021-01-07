using SFA.DAS.EmployerIncentives.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm
{
    public class GetVendorRegistrationStatusByLastStatusChangeDateRequest : IGetApiRequest
    {
        private readonly DateTime _dateTimeFrom;
        private readonly string _skipCodeParameter;

        public GetVendorRegistrationStatusByLastStatusChangeDateRequest(DateTime dateTimeFrom, string skipCode)
        {
            _dateTimeFrom = dateTimeFrom;
            _skipCodeParameter = string.IsNullOrEmpty(skipCode) ? string.Empty : $"&SkipToken={skipCode}";
        }

        public string BaseUrl { get; set; }
        public string GetUrl => $"{BaseUrl}Finance/Registrations?DateTimeFrom={_dateTimeFrom.ToIsoDateTime()}&VendorType=EMPLOYER{_skipCodeParameter}&api-version=2019-06-01";
    }
}