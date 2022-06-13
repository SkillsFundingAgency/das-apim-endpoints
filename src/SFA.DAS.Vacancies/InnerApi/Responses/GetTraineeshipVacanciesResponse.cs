﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.Vacancies.InnerApi.Responses
{
    public class GetTraineeshipVacanciesResponse
    {
        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("totalFound")]
        public long TotalFound { get; set; }

        [JsonProperty("traineeshipVacancies")]
        public IEnumerable<GetTraineeshipVacanciesListItem> TraineeshipVacancies { get; set; }
    }

    public class GetTraineeshipVacanciesListItem
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("anonymousEmployerName")]
        public string AnonymousEmployerName { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("categoryCode")]
        public string CategoryCode { get; set; }

        [JsonProperty("closingDate")]
        public DateTime ClosingDate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("employerName")]
        public string EmployerName { get; set; }

        [JsonProperty("hoursPerWeek")]
        public decimal? HoursPerWeek { get; set; }

        [JsonProperty("isDisabilityConfident")]
        public bool IsDisabilityConfident { get; set; }

        [JsonProperty("isEmployerAnonymous")]
        public bool IsEmployerAnonymous { get; set; }

        [JsonProperty("isPositiveAboutDisability")]
        public bool IsPositiveAboutDisability { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("numberOfPositions")]
        public long NumberOfPositions { get; set; }

        [JsonProperty("postedDate")]
        public DateTime PostedDate { get; set; }

        [JsonProperty("providerName")]
        public string ProviderName { get; set; }

        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("ukprn")]
        public int Ukprn { get; set; }

        [JsonProperty("vacancyLocationType")]
        public string VacancyLocationType { get; set; }

        [JsonProperty("vacancyReference")]
        public string VacancyReference { get; set; }

        [JsonProperty("workingWeek")]
        public string WorkingWeek { get; set; }

        [JsonProperty("expectedDuration")]
        public string ExpectedDuration { get; set; }

        [JsonProperty("employerWebsiteUrl")]
        public string EmployerWebsiteUrl { get; set; }

        [JsonProperty("employerContactPhone")]
        public string EmployerContactPhone { get; set; }

        [JsonProperty("employerContactEmail")]
        public string EmployerContactEmail { get; set; }

        [JsonProperty("employerContactName")]
        public string EmployerContactName { get; set; }

        [JsonProperty("address")]
        public TraineeshipAddress Address { get; set; }

        [JsonProperty("distance")]
        public decimal? Distance { get; set; }

        [JsonProperty("score")]
        public long Score { get; set; }

        [JsonIgnore]
        public string VacancyUrl { get; set; }

        [JsonProperty("routeId")]
        public int? RouteId { get; set; }

        [JsonProperty("routeName")]
        public string RouteName { get; set; }

        [JsonProperty("employerDescription")]
        public string EmployerDescription { get; set; }
    }

    public class TraineeshipAddress
    {
        [JsonProperty("addressLine1")]
        public string AddressLine1 { get; set; }
        [JsonProperty("addressLine2")]
        public string AddressLine2 { get; set; }
        [JsonProperty("addressLine3")]
        public string AddressLine3 { get; set; }
        [JsonProperty("addressLine4")]
        public string AddressLine4 { get; set; }
        [JsonProperty("postcode")]
        public string Postcode { get; set; }
    }
}
