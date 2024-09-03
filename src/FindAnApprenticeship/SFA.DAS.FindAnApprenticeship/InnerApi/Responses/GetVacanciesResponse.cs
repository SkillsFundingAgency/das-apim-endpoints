﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Responses
{
    public class GetVacanciesResponse
    {
        [JsonPropertyName("total")]
        public long Total { get; set; }

        [JsonPropertyName("totalFound")]
        public long TotalFound { get; set; }

        [JsonPropertyName("apprenticeshipVacancies")]
        public IEnumerable<GetVacanciesListItem> ApprenticeshipVacancies { get; set; }
    }

    public class GetVacanciesListItem
    {
        [JsonPropertyName("id")] 
        public string Id { get; set; }

        [JsonPropertyName("anonymousEmployerName")]
        public string AnonymousEmployerName { get; set; }

        [JsonPropertyName("apprenticeshipLevel")]
        public string ApprenticeshipLevel { get; set; }

        [JsonPropertyName("closingDate")] 
        public DateTime ClosingDate { get; set; }

        [JsonPropertyName("employerName")] 
        public string EmployerName { get; set; }

        [JsonPropertyName("isEmployerAnonymous")]
        public bool IsEmployerAnonymous { get; set; }

        [JsonPropertyName("postedDate")] 
        public DateTime PostedDate { get; set; }

        [JsonPropertyName("title")] 
        public string Title { get; set; }

        [JsonPropertyName("vacancyReference")]
        public string VacancyReference { get; set; }

        [JsonPropertyName("standardTitle")]
        public string CourseTitle { get; set; }
        [JsonPropertyName("standardLarsCode")]
        public int CourseId { get; set; }
        [JsonPropertyName("wageAmount")]
        public string WageAmount { get; set; }
        [JsonPropertyName("wageType")]
        public int WageType { get; set; }
        [JsonPropertyName("wageText")]
        public string WageText { get; set; }
        [JsonPropertyName("address")] 
        public Address Address { get; set; }

        [JsonPropertyName("distance")] 
        public decimal? Distance { get; set; }

        [JsonPropertyName("courseRoute")]
        public string CourseRoute { get; set; }

        [JsonPropertyName("standardLevel")]
        public string CourseLevel { get; set; }

        [JsonPropertyName("isDisabilityConfident")]
        public bool IsDisabilityConfident { get; set; }

        [JsonPropertyName("applicationUrl")]
        public string ApplicationUrl { get; set; }
        public CandidateApplication? Application { get; set; } = null;
        [JsonPropertyName("location")]
        public Location Location { get; set; }
        public string? CompanyBenefitsInformation { get; set; }
        public string? AdditionalTrainingDescription { get; set; }

        public class CandidateApplication
        {
            public string Status { get; set; }
        }
    }

    public class Address
    {
        [JsonPropertyName("addressLine1")]
        public string AddressLine1 { get; set; }
        [JsonPropertyName("addressLine2")]
        public string AddressLine2 { get; set; }
        [JsonPropertyName("addressLine3")]
        public string AddressLine3 { get; set; }
        [JsonPropertyName("addressLine4")]
        public string AddressLine4 { get; set; }
        [JsonPropertyName("postcode")]
        public string Postcode { get; set; }
    }

    public class Location
    {
        [JsonPropertyName("lat")]
        public double? Lat { get; set; }
        [JsonPropertyName("lon")]
        public double? Lon { get; set; }
    }

}
