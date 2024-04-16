using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Reservations.Application.Rules.Queries.GetAvailableDates;
using SFA.DAS.Reservations.InnerApi.Responses;

namespace SFA.DAS.Reservations.Api.Models
{
    public class GetAvailableDatesApiResponse
    {
        public IEnumerable<AvailableDateStartWindowResponseItem> AvailableDates { get; set; }

        public static explicit operator GetAvailableDatesApiResponse(GetAvailableDatesResult source)
        {
            if (source == null)
            {
                return null;
            }

            return new GetAvailableDatesApiResponse()
            {
                AvailableDates = source.AvailableDates.Select(x => (AvailableDateStartWindowResponseItem)x)
            };
        }
    }
    
    public class AvailableDateStartWindowResponseItem()
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public static implicit operator AvailableDateStartWindowResponseItem(AvailableDateStartWindow source)
        {

            if (source == null)
            {
                return null;
            }

            return new AvailableDateStartWindowResponseItem
            {
                StartDate = source.StartDate,
                EndDate = source.EndDate
            };
        }
    }
}
