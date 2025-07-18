using System;
using System.Collections.Generic;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.Enums;

namespace SFA.DAS.Recruit.InnerApi.Responses
{
    public record GetApplicationsByVacancyReferenceApiResponse
    {
        public List<Domain.Application> Applications { get; set; } = [];

        public record Application
        {
            public Guid Id { get; set; }
            public Guid CandidateId { get; set; }
            public string? VacancyReference { get; set; }
            public Location? EmploymentLocation { get; set; }
            public Candidate? Candidate { get; set; }
            public DateTime? SubmittedDate { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? WithdrawnDate { get; set; }
            public ApplicationStatus Status { get; set; }
        }
        public record Location
        {
            public List<Address>? Addresses { get; set; }
            public short EmployerLocationOption { get; set; }

        }
        public record Address
        {
            public string FullAddress { get; init; }
            public bool IsSelected { get; init; }
            public short AddressOrder { get; init; }
        }
    }
}