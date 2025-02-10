using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplications;

public class GetApplicationsQueryResult
{
    public List<Application> Applications { get; set; } = [];


    public class Application
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string VacancyReference { get; set; }
        public string EmployerName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public string? ResponseNotes { get; set; }
        public ApplicationStatus Status { get; set; }
        public DateTime? WithdrawnDate { get; set; }
        public Address Address { get; set; }
        public List<Address>? OtherAddresses { get; set; } = [];
        public string? EmploymentLocationInformation { get; set; }
        public AvailableWhere? EmploymentLocationOption { get; set; }
    }
    public class Address
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string Postcode { get; set; }

        public static implicit operator Address(PostGetVacanciesByReferenceApiResponse.Address source)
        {
            return new Address
            {
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                AddressLine3 = source.AddressLine3,
                AddressLine4 = source.AddressLine4,
                Postcode = source.Postcode
            };
        }

        public static implicit operator Address(GetClosedVacancyResponse.Address source)
        {
            return new Address
            {
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                AddressLine3 = source.AddressLine3,
                AddressLine4 = source.AddressLine4,
                Postcode = source.Postcode
            };
        }
    }
}