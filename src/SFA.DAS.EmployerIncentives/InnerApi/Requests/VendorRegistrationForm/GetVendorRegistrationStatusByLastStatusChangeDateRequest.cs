using SFA.DAS.EmployerIncentives.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm
{
    public class GetVendorRegistrationStatusByLastStatusChangeDateRequest : IGetApiRequest
    {
        private readonly DateTime _dateTimeFrom;
        private readonly DateTime _dateTimeTo;

        public GetVendorRegistrationStatusByLastStatusChangeDateRequest(DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            _dateTimeFrom = dateTimeFrom;
            _dateTimeTo = dateTimeTo;
        }

        public string BaseUrl { get; set; }
        public string GetUrl => $"{BaseUrl}Finance/Registrations?DateTimeFrom={_dateTimeFrom.ToIsoDateTime()}&DateTimeTo={_dateTimeTo.ToIsoDateTime()}&api-version=2019-06-01";
    }
}