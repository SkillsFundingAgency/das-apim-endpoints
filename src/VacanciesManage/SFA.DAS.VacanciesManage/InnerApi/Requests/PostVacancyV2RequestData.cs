using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.VacanciesManage.InnerApi.Requests;

public class PostVacancyV2RequestData
{
    [JsonPropertyName("accountId")]
    public long AccountId { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("apprenticeshipType")]
    public string ApprenticeshipType { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("ownerType")]
    public string OwnerType { get; set; }

    [JsonPropertyName("sourceOrigin")]
    public string SourceOrigin { get; set; }

    [JsonPropertyName("sourceType")]
    public string SourceType { get; set; }

    [JsonPropertyName("submittedDate")]
    public DateTime SubmittedDate { get; set; }

    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("closingDate")]
    public DateTime ClosingDate { get; set; }

    [JsonPropertyName("applicationUrl")]
    public string ApplicationUrl { get; set; }

    [JsonPropertyName("applicationMethod")]
    public string ApplicationMethod { get; set; }

    [JsonPropertyName("applicationInstructions")]
    public string ApplicationInstructions { get; set; }

    [JsonPropertyName("shortDescription")]
    public string ShortDescription { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("anonymousReason")]
    public string AnonymousReason { get; set; }

    [JsonPropertyName("disabilityConfident")]
    public bool DisabilityConfident { get; set; }

    [JsonPropertyName("contact")]
    public PostVacancyV2Contact Contact { get; set; }

    [JsonPropertyName("employerDescription")]
    public string EmployerDescription { get; set; }

    [JsonPropertyName("employerLocations")]
    public List<PostVacancyV2EmployerLocation> EmployerLocations { get; set; }

    [JsonPropertyName("employerLocationOption")]
    public string EmployerLocationOption { get; set; }

    [JsonPropertyName("employerLocationInformation")]
    public string EmployerLocationInformation { get; set; }

    [JsonPropertyName("employerName")]
    public string EmployerName { get; set; }

    [JsonPropertyName("employerNameOption")]
    public string EmployerNameOption { get; set; }

    [JsonPropertyName("employerRejectedReason")]
    public string EmployerRejectedReason { get; set; }

    [JsonPropertyName("legalEntityName")]
    public string LegalEntityName { get; set; }

    [JsonPropertyName("employerWebsiteUrl")]
    public string EmployerWebsiteUrl { get; set; }

    [JsonPropertyName("geoCodeMethod")]
    public string GeoCodeMethod { get; set; }

    [JsonPropertyName("accountLegalEntityId")]
    public long AccountLegalEntityId { get; set; }

    [JsonPropertyName("numberOfPositions")]
    public long NumberOfPositions { get; set; }

    [JsonPropertyName("outcomeDescription")]
    public string OutcomeDescription { get; set; }

    [JsonPropertyName("programmeId")]
    public string ProgrammeId { get; set; }

    [JsonPropertyName("skills")]
    public List<string> Skills { get; set; }

    [JsonPropertyName("qualifications")]
    public List<PostVacancyV2Qualification> Qualifications { get; set; }

    [JsonPropertyName("thingsToConsider")]
    public string ThingsToConsider { get; set; }

    [JsonPropertyName("trainingDescription")]
    public string TrainingDescription { get; set; }

    [JsonPropertyName("additionalTrainingDescription")]
    public string AdditionalTrainingDescription { get; set; }

    [JsonPropertyName("trainingProvider")]
    public PostVacancyV2TrainingProvider TrainingProvider { get; set; }

    [JsonPropertyName("wage")]
    public PostVacancyV2Wage Wage { get; set; }

    [JsonPropertyName("additionalQuestion1")]
    public string AdditionalQuestion1 { get; set; }

    [JsonPropertyName("additionalQuestion2")]
    public string AdditionalQuestion2 { get; set; }

    [JsonPropertyName("hasSubmittedAdditionalQuestions")]
    public bool HasSubmittedAdditionalQuestions { get; set; }

    [JsonPropertyName("hasChosenProviderContactDetails")]
    public bool HasChosenProviderContactDetails { get; set; }

    [JsonPropertyName("hasOptedToAddQualifications")]
    public bool HasOptedToAddQualifications { get; set; }

    [JsonPropertyName("submittedByUserId")]
    public string SubmittedByUserId { get; set; }
}

public class PostVacancyV2Contact
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("phone")]
    public string Phone { get; set; }
}

public class PostVacancyV2Qualification
{
    [JsonPropertyName("qualificationType")]
    public string QualificationType { get; set; }

    [JsonPropertyName("subject")]
    public string Subject { get; set; }

    [JsonPropertyName("grade")]
    public string Grade { get; set; }

    [JsonPropertyName("level")]
    public int? Level { get; set; }

    [JsonPropertyName("weighting")]
    public string Weighting { get; set; }

    [JsonPropertyName("otherQualificationName")]
    public string OtherQualificationName { get; set; }
}

public class PostVacancyV2TrainingProvider
{
    [JsonPropertyName("ukprn")]
    public long Ukprn { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("address")]
    public PostVacancyV2EmployerLocation Address { get; set; }
}

public class PostVacancyV2EmployerLocation
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

    [JsonPropertyName("latitude")]
    public long Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public long Longitude { get; set; }
}

public class PostVacancyV2Wage
{
    [JsonPropertyName("duration")] public long Duration { get; set; }

    [JsonPropertyName("durationUnit")] public string DurationUnit { get; set; }

    [JsonPropertyName("workingWeekDescription")]
    public string WorkingWeekDescription { get; set; }

    [JsonPropertyName("weeklyHours")] 
    public decimal WeeklyHours { get; set; }

    [JsonPropertyName("wageType")] public string WageType { get; set; }

    [JsonPropertyName("fixedWageYearlyAmount")]
    public decimal? FixedWageYearlyAmount { get; set; }

    [JsonPropertyName("wageAdditionalInformation")]
    public string WageAdditionalInformation { get; set; }

    [JsonPropertyName("companyBenefitsInformation")]
    public string CompanyBenefitsInformation { get; set; }

    [JsonPropertyName("apprenticeMinimumWage")]
    public long ApprenticeMinimumWage { get; set; }

    [JsonPropertyName("under18NationalMinimumWage")]
    public long Under18NationalMinimumWage { get; set; }

    [JsonPropertyName("between18AndUnder21NationalMinimumWage")]
    public long Between18AndUnder21NationalMinimumWage { get; set; }

    [JsonPropertyName("between21AndUnder25NationalMinimumWage")]
    public long Between21AndUnder25NationalMinimumWage { get; set; }

    [JsonPropertyName("over25NationalMinimumWage")]
    public long Over25NationalMinimumWage { get; set; }

    [JsonPropertyName("wageText")] public string WageText { get; set; }
}