using System;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.InnerApi.Responses
{
    public class GetAllApplicationsByIdApiResponse
    {
        public List<Domain.Application> Applications { get; set; }

        public class Application
        {
            public Guid Id { get; set; }
            public Guid CandidateId { get; set; }
            public string? VacancyReference { get; set; }
            public Location? EmploymentLocation { get; set; }
        }
        public class Location
        {
            public List<Address>? Addresses { get; set; }
            public short EmployerLocationOption { get; set; }

        }
        public class Address
        {
            public string FullAddress { get; init; }
            public bool IsSelected { get; init; }
            public short AddressOrder { get; init; }
        }
    }
}