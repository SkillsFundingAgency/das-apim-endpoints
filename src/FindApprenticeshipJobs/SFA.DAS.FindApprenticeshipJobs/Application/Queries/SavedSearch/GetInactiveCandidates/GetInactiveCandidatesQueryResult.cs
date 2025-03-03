using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetInactiveCandidates
{
    public record GetInactiveCandidatesQueryResult
    {
        public List<Candidate> Candidates { get; set; } = [];
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public static implicit operator GetInactiveCandidatesQueryResult(GetInactiveCandidatesApiResponse source)
        {
            return new GetInactiveCandidatesQueryResult
            {
                Candidates = source.Candidates.Select(candidate => (Candidate)candidate).ToList()
            };
        }

        public class Candidate
        {
            public Address Address { get; set; }
            public string? GovUkIdentifier { get; set; }
            public string? LastName { get; set; }
            public string? FirstName { get; set; }
            public string? MiddleNames { get; set; }
            public string? PhoneNumber { get; set; }
            public DateTime DateOfBirth { get; set; }
            public DateTime CreatedOn { get; set; }
            public DateTime UpdatedOn { get; set; }
            public DateTime? TermsOfUseAcceptedOn { get; set; }
            public UserStatus Status { get; set; }
            public string? MigratedEmail { get; set; }
            public string? Email { get; set; }
            public Guid Id { get; set; }
            public Guid? MigratedCandidateId { get; set; }

            public static implicit operator Candidate(GetInactiveCandidatesApiResponse.Candidate source)
            {
                return new Candidate
                {
                    Address = source.Address,
                    GovUkIdentifier = source.GovUkIdentifier,
                    LastName = source.LastName,
                    FirstName = source.FirstName,
                    MiddleNames = source.MiddleNames,
                    PhoneNumber = source.PhoneNumber,
                    DateOfBirth = source.DateOfBirth,
                    CreatedOn = source.CreatedOn,
                    UpdatedOn = source.UpdatedOn,
                    TermsOfUseAcceptedOn = source.TermsOfUseAcceptedOn,
                    Status = source.Status,
                    MigratedEmail = source.MigratedEmail,
                    Email = source.Email,
                    Id = source.Id,
                    MigratedCandidateId = source.MigratedCandidateId
                };
            }
        }

        public class Address
        {
            public Guid Id { get; set; }
            public string? Uprn { get; set; }
            public string? AddressLine1 { get; set; }
            public string? AddressLine2 { get; set; }
            public string? Town { get; set; }
            public string? County { get; set; }
            public string? Postcode { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public Guid CandidateId { get; set; }

            public static implicit operator Address(GetInactiveCandidatesApiResponse.Address source)
            {
                return new Address
                {
                    Id = source.Id,
                    Uprn = source.Uprn,
                    AddressLine1 = source.AddressLine1,
                    AddressLine2 = source.AddressLine2,
                    Town = source.Town,
                    County = source.County,
                    Postcode = source.Postcode,
                    Latitude = source.Latitude,
                    Longitude = source.Longitude,
                    CandidateId = source.CandidateId
                };
            }
        }
    }
}
