using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Api.Models
{
    public class GetTraineeshipVacanciesListResponse
    {
        public long Total { get; set; }
        public long TotalFiltered { get; set; }
        public int TotalPages { get; set; }
        public List<GetTraineeshipVacanciesListResponseItem> Vacancies { get; set; }

        public static implicit operator GetTraineeshipVacanciesListResponse(GetTraineeshipVacanciesQueryResult source)
        {
            return new GetTraineeshipVacanciesListResponse()
            {
                Vacancies = source.Vacancies.Select(c => (GetTraineeshipVacanciesListResponseItem)c).ToList(),
                Total = source.Total,
                TotalFiltered = source.TotalFiltered,
                TotalPages = source.TotalPages
            };
        }

    }
    public class GetTraineeshipVacanciesListResponseItem
    {
        public long Id { get; set; }
        public string AnonymousEmployerName { get; set; }
        public DateTime ClosingDate { get; set; }
        public string Description { get; set; }
        public string EmployerName { get; set; }
        public decimal HoursPerWeek { get; set; }
        public bool IsDisabilityConfident { get; set; }
        public bool IsNationalVacancy { get; set; }
        public long NumberOfPositions { get; set; }
        public DateTime PostedDate { get; set; }
        public string ProviderName { get; set; }
        public DateTime StartDate { get; set; }
        public string Title { get; set; }
        public int Ukprn { get; set; }
        public string VacancyReference { get; set; }
        public string VacancyUrl { get; set; }
        public TraineeshipVacancyLocation Location { get; set; }
        public GetTraineeshipVacancyAddressItem Address { get; set; }
        public decimal? Distance { get; set; }
        public string EmployerContactPhone { get; set; }
        public string EmployerContactName { get; set; }
        public string EmployerContactEmail { get; set; }
        public string EmployerWebsiteUrl { get; set; }
        public string ExpectedDuration { get; set; }
        public int? RouteId { get; set; }
        public string RouteName { get; set; }
        public bool IsEmployerAnonymous { get; set; }
        public bool IsPositiveAboutDisability { get; set; }
        public string WorkingWeek { get; set; }
        public double Score { get; set; }
        public string EmployerDescription { get; set; }
        public string Category { get; set; }
        public string CategoryCode { get; set; }

        public static implicit operator GetTraineeshipVacanciesListResponseItem(GetTraineeshipVacanciesListItem source)
        {
            return new GetTraineeshipVacanciesListResponseItem
            {
                Id = source.Id,
                AnonymousEmployerName = source.AnonymousEmployerName,
                ClosingDate = source.ClosingDate,
                Description = source.Description,
                EmployerName = source.IsEmployerAnonymous ? source.AnonymousEmployerName : source.EmployerName,
                HoursPerWeek = source.HoursPerWeek,
                IsDisabilityConfident = source.IsDisabilityConfident,
                IsNationalVacancy = source.VacancyLocationType.Equals("National", StringComparison.CurrentCultureIgnoreCase),
                NumberOfPositions = source.NumberOfPositions,
                PostedDate = source.PostedDate,
                ProviderName = source.ProviderName,
                StartDate = source.StartDate,
                Title = source.Title,
                Ukprn = source.Ukprn,
                VacancyReference = source.VacancyReference,
                VacancyUrl = source.VacancyUrl,
                Distance = source.Distance,
                Address = source,
                EmployerWebsiteUrl = source.EmployerWebsiteUrl,
                EmployerContactEmail = source.EmployerContactEmail,
                EmployerContactName = source.EmployerContactName,
                EmployerContactPhone = source.EmployerContactPhone,
                ExpectedDuration = source.ExpectedDuration,
                Location = new TraineeshipVacancyLocation
                {
                    Lat = source.Location.Lat,
                    Lon = source.Location.Lon
                },
                RouteId = source.RouteId,
                RouteName = source.RouteName,
                IsEmployerAnonymous = source.IsEmployerAnonymous,
                IsPositiveAboutDisability = source.IsPositiveAboutDisability,
                WorkingWeek = source.WorkingWeek,
                Score = source.Score,
                EmployerDescription = source.EmployerDescription,
                Category = source.Category,
                CategoryCode = source.CategoryCode
            };
        }
    }
    public class TraineeshipVacancyLocation
    {
        public double? Lat { get; set; }
        public double? Lon { get; set; }
    }
}
