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
                    StandardReference = source.EmployerRequests.FirstOrDefault().StandardReference,
                    StandardTitle = source.EmployerRequests.FirstOrDefault().StandardTitle,
                    StandardLevel = source.EmployerRequests.FirstOrDefault().StandardLevel,
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
        public List<string> DeliveryMethods { get; set; }
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
                DeliveryMethods = GetDeliveryMethods(source),
            };
        }

        private static List<string> GetDeliveryMethods(GetEmployerRequestsByIdsResponse response)
        {
            var methods = new List<string>();

            if (response.AtApprenticesWorkplace)
            {
                methods.Add("At Apprentices Workplace");
            }
            if (response.BlockRelease)
            {
                methods.Add("Block Release");
            }
            if (response.DayRelease)
            {
                methods.Add("Day Release");
            }
            return methods;
        }
    }
}
