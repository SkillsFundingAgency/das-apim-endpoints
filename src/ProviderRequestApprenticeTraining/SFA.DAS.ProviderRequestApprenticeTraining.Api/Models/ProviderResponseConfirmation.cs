using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderResponseConfirmation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.Models
{
    public class ProviderResponseConfirmation
    {
        public long Ukprn { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public List<SelectEmployerRequest> EmployerRequests { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }


        public static implicit operator ProviderResponseConfirmation(GetProviderResponseConfirmationResult source)
        {
            return new ProviderResponseConfirmation
            {
                Ukprn = source.Ukprn,
                StandardTitle = source.EmployerRequests.FirstOrDefault().StandardTitle,
                StandardLevel = source.EmployerRequests.FirstOrDefault().StandardLevel,
                Email = source.Email,
                Phone = source.Phone,
                Website = source.Website,
                EmployerRequests = source.EmployerRequests.Select(entity => (SelectEmployerRequest)entity).ToList(),
            };
        }
    }
}
