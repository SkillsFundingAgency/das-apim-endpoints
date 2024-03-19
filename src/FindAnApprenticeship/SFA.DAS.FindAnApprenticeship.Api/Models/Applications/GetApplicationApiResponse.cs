using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplication;
using System;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class GetApplicationApiResponse
{
    public CandidateDetailsSection Candidate { get; set; }
   
    public static implicit operator GetApplicationApiResponse(GetApplicationQueryResult source)
    {
        return new GetApplicationApiResponse
        {
            Candidate = source.CandidateDetails
        };
    }

    public class CandidateDetailsSection
    {
        public Guid Id { get; set; }
        public string GovUkIdentifier { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public AddressDetailsSection Address { get; set; }

        public static implicit operator CandidateDetailsSection(GetApplicationQueryResult.Candidate source)
        {
            if (source is null) return null;

            return new CandidateDetailsSection
            {
                Id = source.Id,
                GovUkIdentifier = source.GovUkIdentifier,
                Email = source.Email,
                FirstName = source.FirstName,
                MiddleName = source.MiddleName,
                LastName = source.LastName,
                PhoneNumber = source.PhoneNumber,
                DateOfBirth = source.DateOfBirth,
                Address = source.Address
            };
        }
    }

    public class AddressDetailsSection
    {
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public string? Town { get; set; }
        public string? County { get; set; }
        public string Postcode { get; set; } = null!;

        public static implicit operator AddressDetailsSection(GetApplicationQueryResult.CandidateAddress source)
        {
            if (source is null) return null;

            return new AddressDetailsSection
            {
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                Town = source.Town,
                County = source.County,
                Postcode = source.Postcode,
            };
        }
    }
}