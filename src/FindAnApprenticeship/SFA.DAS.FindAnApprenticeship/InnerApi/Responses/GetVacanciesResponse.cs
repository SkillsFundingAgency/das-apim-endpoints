using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using SFA.DAS.SharedOuterApi.Domain;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Responses;

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

    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }

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
    [JsonPropertyName("over25NationalMinimumWage")]
    public decimal? Over25NationalMinimumWage { get; set; }
    [JsonPropertyName("between21AndUnder25NationalMinimumWage")]
    public decimal? Between21AndUnder25NationalMinimumWage { get; set; }
    [JsonPropertyName("between18AndUnder21NationalMinimumWage")]
    public decimal? Between18AndUnder21NationalMinimumWage { get; set; }
    [JsonPropertyName("under18NationalMinimumWage")]
    public decimal? Under18NationalMinimumWage { get; set; }
    [JsonPropertyName("apprenticeMinimumWage")]
    public decimal? ApprenticeMinimumWage { get; set; }
    [JsonPropertyName("address")]
    public Address Address { get; set; }

    [JsonPropertyName("otherAddresses")] 
    public List<Address>? OtherAddresses { get; set; } = [];
    [JsonPropertyName("isPrimaryLocation")]
    public bool IsPrimaryLocation { get; set; }

    [JsonPropertyName("employmentLocationInformation")]
    public string? EmploymentLocationInformation { get; set; }
        
    [JsonPropertyName("availableWhere"), JsonConverter(typeof(JsonStringEnumConverter<AvailableWhere>))]
    public AvailableWhere? EmployerLocationOption { get; set; }

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
    public bool IsSavedVacancy { get; set; } = false;

    [JsonPropertyName("vacancySource")] 
    public VacancyDataSource VacancySource { get; set; }
    
    [JsonPropertyName("apprenticeshipType")]
    public ApprenticeshipTypes ApprenticeshipType { get; set; }

    public class CandidateApplication
    {
        public string Status { get; set; }
    }
}
public class Location
{
    [JsonPropertyName("lat")]
    public double? Lat { get; set; }
    [JsonPropertyName("lon")]
    public double? Lon { get; set; }
}