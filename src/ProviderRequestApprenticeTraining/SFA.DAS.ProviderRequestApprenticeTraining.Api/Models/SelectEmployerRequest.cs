using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.Models
{
    public class SelectEmployerRequest 
    {
        public Guid EmployerRequestId { get; set; }
        public List<string> Locations { get; set; }
        public string DateOfRequest { get; set; }
        public int NumberOfApprentices { get; set; }
        public bool AtApprenticesWorkplace { get; set; }
        public bool BlockRelease { get; set; }
        public bool DayRelease { get; set; }
        public bool IsNew { get; set; }
        public bool IsContacted { get; set; }

        public static implicit operator SelectEmployerRequest(GetSelectEmployerRequestsResponse source)
        {
            return new SelectEmployerRequest
            {
                EmployerRequestId = source.EmployerRequestId,
                DateOfRequest = source.DateOfRequest.ToString("d MMMM yyyy"),
                IsContacted = source.IsContacted,
                IsNew = source.IsNew,
                NumberOfApprentices = source.NumberOfApprentices,   
                Locations = source.Locations,
                AtApprenticesWorkplace = source.AtApprenticesWorkplace,
                BlockRelease = source.BlockRelease,
                DayRelease = source.DayRelease,
            };
        }
    }
}
