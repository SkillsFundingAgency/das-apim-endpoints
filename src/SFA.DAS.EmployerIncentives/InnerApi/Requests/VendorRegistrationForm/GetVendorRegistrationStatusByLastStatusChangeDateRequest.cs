using SFA.DAS.EmployerIncentives.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm
{
    public class GetVendorRegistrationStatusByLastStatusChangeDateRequest : IGetApiRequest
    {
        private readonly DateTime _dateTimeFrom;

        public GetVendorRegistrationStatusByLastStatusChangeDateRequest(DateTime dateTimeFrom)
        {
            _dateTimeFrom = dateTimeFrom;
        }

        public string BaseUrl { get; set; }
        public string GetUrl => $"{BaseUrl}Finance/Registrations?DateTimeFrom={_dateTimeFrom.ToIsoDateTime()}&VendorType=EMPLOYER&api-version=2019-06-01";
    }
}