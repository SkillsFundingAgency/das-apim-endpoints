using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.Models
{
    public class CheckYourAnswers
    {
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public List<EmployerRequest> Requests { get; set; }
        public string Website { get; set; }

        public static implicit operator CheckYourAnswers(GetEmployerRequestsByIdsResult source)
        {
            if (source.EmployerRequests.Any())
            {
                return new CheckYourAnswers
                {
                    StandardReference = source.EmployerRequests.First().StandardReference,
                    StandardTitle = source.EmployerRequests.First().StandardTitle,
                    StandardLevel = source.EmployerRequests.First().StandardLevel,
                    Requests = source.EmployerRequests.Select(request => (EmployerRequest)request).ToList()
                };
            }
            else
            {
                return new CheckYourAnswers 
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
