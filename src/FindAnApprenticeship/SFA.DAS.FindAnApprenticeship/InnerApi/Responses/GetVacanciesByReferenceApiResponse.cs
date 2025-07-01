using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Responses
{
    public class PostGetVacanciesByReferenceApiResponse
    {
        public IEnumerable<ApprenticeshipVacancy> ApprenticeshipVacancies { get; set; }

        public class ApprenticeshipVacancy : IVacancy
        {
            private List<Address> _otherAddresses;
            public string VacancyReference { get; set; }
            public string EmployerName { get; set; }
            public string Title { get; set; }
            public DateTime ClosingDate { get; set; }
            public DateTime? ClosedDate { get; set; }
            public int CourseId { get; set; }
            public string AdditionalQuestion1 { get; set; }
            public string AdditionalQuestion2 { get; set; }
            public bool IsDisabilityConfident { get; set; }
            public string City =>
                !string.IsNullOrEmpty(Address?.AddressLine4) ? Address.AddressLine4 :
                !string.IsNullOrEmpty(Address?.AddressLine3) ? Address.AddressLine3 :
                !string.IsNullOrEmpty(Address?.AddressLine2) ? Address.AddressLine2 :
                !string.IsNullOrEmpty(Address?.AddressLine1) ? Address.AddressLine1 :
                string.Empty;
            public string Postcode => Address?.Postcode ?? string.Empty;
            public string ApplicationUrl { get; set; }
            public string ExternalVacancyUrl => ApplicationUrl;
            public VacancyDataSource VacancySource { get; set; }
            public Address Address { get; set; }

            public List<Address>? OtherAddresses
            {
                get
                {
                    if (EmployerLocationOption is not AvailableWhere.MultipleLocations)
                    {
                        return null;
                    }
                    return _otherAddresses
                        .DistinctBy(x => x.ToSingleLineAddress())
                        .ToList();
                }
                set => _otherAddresses = value;
            }

            public string EmploymentLocationInformation { get; set; }
            [JsonPropertyName("availableWhere"), JsonConverter(typeof(JsonStringEnumConverter<AvailableWhere>))]
            public AvailableWhere? EmployerLocationOption { get; set; }

            public ApprenticeshipTypes ApprenticeshipType { get; set; }
            public bool IsExternalVacancy => !string.IsNullOrWhiteSpace(ApplicationUrl);
        }
    }
}