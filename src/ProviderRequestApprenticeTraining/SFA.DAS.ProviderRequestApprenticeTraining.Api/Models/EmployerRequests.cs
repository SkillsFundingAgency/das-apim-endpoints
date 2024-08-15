using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.Models
{
    public class EmployerRequests
    {
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public List<EmployerRequest> Requests { get; set; }

        public static implicit operator EmployerRequests(GetEmployerRequestsByIdsResult source)
        {
            if (source.EmployerRequests.Any())
            {
                return new EmployerRequests
                {
                    StandardReference = source.EmployerRequests.First().StandardReference,
                    StandardTitle = source.EmployerRequests.First().StandardTitle,
                    StandardLevel = source.EmployerRequests.First().StandardLevel,
                    Requests = source.EmployerRequests.Select(request => (EmployerRequest)request).ToList()
                };
            }
            else
            {
                return new EmployerRequests 
                {
                    Requests = new List<EmployerRequest>()
                };
            }
        }
    }

    public class EmployerRequest 
    {
        public Guid EmployerRequestId { get; set; }
        public List<string> Locations { get; set; }
        public bool DayRelease { get; set; }
        public bool AtApprenticesWorkplace { get; set; }
        public bool BlockRelease { get; set; }
        public string DateOfRequest { get; set; }
        public int NumberOfApprentices { get; set; }

        public static implicit operator EmployerRequest(GetEmployerRequestsByIdsResponse source)
        {
            return new EmployerRequest
            {
                EmployerRequestId = source.EmployerRequestId,
                DateOfRequest = source.DateOfRequest.ToString("d MMMM yyyy"),
                NumberOfApprentices = source.NumberOfApprentices,
                Locations = source.Locations,
                AtApprenticesWorkplace = source.AtApprenticesWorkplace,
                BlockRelease = source.BlockRelease,
                DayRelease = source.DayRelease,
            };
        } 
    }
}
