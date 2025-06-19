#nullable enable
using System;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Domain
{
    public record Application
    {
        public Candidate? Candidate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? WithdrawnDate { get; set; }
        public Guid CandidateId { get; set; }
        public Guid Id { get; set; }
        public Location? EmploymentLocation { get; set; }
        public string? VacancyReference { get; set; }
    }
    public record Location
    {
        public List<Address>? Addresses { get; set; }
    }
    public record Address
    {
        public bool IsSelected { get; init; }
        public short AddressOrder { get; init; }
        public string? FullAddress { get; init; }
    }
}