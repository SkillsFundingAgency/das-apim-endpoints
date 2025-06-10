using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.Models
{
    public class GetVacanciesListResponse
    {
        public IEnumerable<GetVacanciesListResponseItem> Vacancies { get; set; }

        public static implicit operator GetVacanciesListResponse(SearchApprenticeshipsResult source)
        {
            return new GetVacanciesListResponse
            {
                Vacancies = source.Vacancies.Select(c => (GetVacanciesListResponseItem)c)
            };
        }

    }

    public class GetVacanciesListAddressItem
    {
        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string Postcode { get; set; }

        public static GetVacanciesListAddressItem From(Address address)
        {
            if (address is null)
            {
                return null;
            }

            return new GetVacanciesListAddressItem
            {
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                AddressLine3 = address.AddressLine3,
                AddressLine4 = address.AddressLine4,
                Postcode = address.Postcode
            };
        }
    }
    
    public class GetVacanciesListResponseItem
    {
        public string Id { get; set; }
        public DateTime ClosingDate { get; set; }
        public string EmployerName { get; set; }
        public DateTime PostedDate { get; set; }
        public DateTime StartDate { get; set; }
        public string Title { get; set; }
        public string VacancyReference { get; set; }
        public string CourseTitle { get; set; }
        public string WageAmount { get; set; }
        public int WageType { get; set; }
        public decimal? Over25NationalMinimumWage { get; set; }
        public decimal? Between21AndUnder25NationalMinimumWage { get; set; }
        public decimal? Between18AndUnder21NationalMinimumWage { get; set; }
        public decimal? Under18NationalMinimumWage { get; set; }
        public decimal? ApprenticeMinimumWage { get; set; }
        public string AddressLine1 { get; private set; }
        public string? AddressLine2 { get; private set; }
        public string AddressLine3 { get; private set; }
        public string? AddressLine4 { get; private set; }
        public string PostCode { get; private set; }
        public List<GetVacanciesListAddressItem> OtherAddresses { get; set; }
        public bool IsPrimaryLocation { get; set; }
        public string? EmploymentLocationInformation { get; private set; }
        public decimal? Distance { get; set; }
        public string CourseLevel { get; set; }
        public int CourseId { get; set; }
        public string CourseRoute { get; set; }

        public string ApprenticeshipLevel { get; set; }
        public ApprenticeshipTypes ApprenticeshipType { get; set; }
        public string WageText { get; set; }
        public bool IsDisabilityConfident { get; set; }
        public double? Lon { get; set; }

        public double? Lat { get; set; }
        public string? CompanyBenefitsInformation { get; set; }
        public string? AdditionalTrainingDescription { get; set; }
        public bool IsSavedVacancy { get; set; } = false;
        public VacancyDataSource VacancySource { get; set; }
        public CandidateApplicationDetails? Application { get; set; }
        public string ApplicationUrl { get; set; }
        public static implicit operator GetVacanciesListResponseItem(GetVacanciesListItem source)
        {
            return new GetVacanciesListResponseItem
            {
                AdditionalTrainingDescription = source.AdditionalTrainingDescription,
                AddressLine1 = source.Address?.AddressLine1,
                AddressLine2 = source.Address?.AddressLine2,
                AddressLine3 = source.Address?.AddressLine3,
                AddressLine4 = source.Address?.AddressLine4,
                Application = source.Application,
                ApplicationUrl = source.ApplicationUrl,
                ApprenticeMinimumWage = source.ApprenticeMinimumWage,
                ApprenticeshipLevel = source.ApprenticeshipLevel,
                ApprenticeshipType = source.ApprenticeshipType,
                Between18AndUnder21NationalMinimumWage = source.Between18AndUnder21NationalMinimumWage,
                Between21AndUnder25NationalMinimumWage = source.Between21AndUnder25NationalMinimumWage,
                ClosingDate = source.ClosingDate,
                CompanyBenefitsInformation = source.CompanyBenefitsInformation,
                CourseId = source.CourseId,
                CourseLevel = source.CourseLevel,
                CourseRoute = source.CourseRoute,
                CourseTitle = source.CourseTitle,
                Distance = source.Distance,
                EmployerName = source.IsEmployerAnonymous ? source.AnonymousEmployerName : source.EmployerName,
                EmploymentLocationInformation = source.EmploymentLocationInformation,
                Id = source.Id,
                IsDisabilityConfident = source.IsDisabilityConfident,
                IsPrimaryLocation = source.IsPrimaryLocation,
                IsSavedVacancy = source.IsSavedVacancy,
                Lat = source.Location?.Lat,
                Lon = source.Location?.Lon,
                OtherAddresses = source.OtherAddresses?.Select(GetVacanciesListAddressItem.From).ToList(),
                Over25NationalMinimumWage = source.Over25NationalMinimumWage,
                PostCode = source.Address?.Postcode,
                PostedDate = source.PostedDate,
                StartDate = source.StartDate,
                Title = source.Title,
                Under18NationalMinimumWage = source.Under18NationalMinimumWage,
                VacancyReference = source.VacancyReference,
                VacancySource = source.VacancySource,
                WageAmount = source.WageAmount,
                WageText = source.WageText,
                WageType = source.WageType,
            };
        }
    }

    public class CandidateApplicationDetails
    {
        public string Status { get; set; }

        public static implicit operator CandidateApplicationDetails(GetVacanciesListItem.CandidateApplication source)
        {
            if (source is null) return null;

            return new CandidateApplicationDetails
            {
                Status = source.Status
            };
        }
    }
}