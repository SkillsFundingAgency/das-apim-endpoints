#nullable enable
using System;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Domain
{
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
    }
    public class Address
    {
        public string FullAddress { get; init; }
        public short AddressOrder { get; init; }
    }
}
