using System;
using System.Text.Json;
using AutoFixture;
using SFA.DAS.Recruit.Domain.Vacancy;
using SFA.DAS.Recruit.GraphQL;
using SFA.DAS.SharedOuterApi.Models;
using ApplicationMethod = SFA.DAS.Recruit.GraphQL.ApplicationMethod;
using ClosureReason = SFA.DAS.Recruit.GraphQL.ClosureReason;
using DurationUnit = SFA.DAS.Recruit.GraphQL.DurationUnit;
using EmployerNameOption = SFA.DAS.Recruit.GraphQL.EmployerNameOption;
using GeoCodeMethod = SFA.DAS.Recruit.GraphQL.GeoCodeMethod;
using OwnerType = SFA.DAS.Recruit.GraphQL.OwnerType;
using SourceOrigin = SFA.DAS.Recruit.GraphQL.SourceOrigin;
using SourceType = SFA.DAS.Recruit.GraphQL.SourceType;
using VacancyStatus = SFA.DAS.Recruit.GraphQL.VacancyStatus;
using WageType = SFA.DAS.Recruit.GraphQL.WageType;

namespace SFA.DAS.Recruit.UnitTests;

public class AllVacancyFieldsFake: IAllVacancyFields
{
    private static readonly RandomNumericSequenceGenerator VacancyReferenceGenerator = new (20000000, 21000000);
    private static readonly RandomNumericSequenceGenerator AccountIdGenerator = new (1000, 9999);
    
    public static AllVacancyFieldsFake Create()
    {
        var fixture = new Fixture();
        var result = fixture.Create<AllVacancyFieldsFake>();
        result.AccountId = (long)AccountIdGenerator.Create(typeof(long), null);
        result.EmployerLocations = JsonSerializer.Serialize(fixture.CreateMany<Address>(), Global.JsonSerializerOptions);
        result.EmployerReviewFieldIndicators = JsonSerializer.Serialize(fixture.CreateMany<ReviewFieldIndicator>(), Global.JsonSerializerOptions);
        result.ProviderReviewFieldIndicators = JsonSerializer.Serialize(fixture.CreateMany<ReviewFieldIndicator>(), Global.JsonSerializerOptions);
        result.Qualifications = JsonSerializer.Serialize(fixture.CreateMany<Qualification>(), Global.JsonSerializerOptions);
        result.Skills = JsonSerializer.Serialize(fixture.CreateMany<string>(), Global.JsonSerializerOptions);
        result.SourceVacancyReference = (long)VacancyReferenceGenerator.Create(typeof(long), null);
        result.TrainingProvider_Address = JsonSerializer.Serialize(fixture.Create<Address>(), Global.JsonSerializerOptions);
        result.TransferInfo = JsonSerializer.Serialize(fixture.Create<TransferInfo>(), Global.JsonSerializerOptions);
        result.VacancyReference = (long)VacancyReferenceGenerator.Create(typeof(long), null);
        
        return result;
    }

    public Guid Id { get; set; }
    public long? VacancyReference { get; set; }
    public long? AccountId { get; set; }
    public VacancyStatus Status { get; set; }
    public ApprenticeshipTypes? ApprenticeshipType { get; set; }
    public string? Title { get; set; }
    public OwnerType? OwnerType { get; set; }
    public SourceOrigin? SourceOrigin { get; set; }
    public SourceType? SourceType { get; set; }
    public long? SourceVacancyReference { get; set; }
    public DateTimeOffset? ApprovedDate { get; set; }
    public DateTimeOffset? CreatedDate { get; set; }
    public DateTimeOffset? LastUpdatedDate { get; set; }
    public DateTimeOffset? SubmittedDate { get; set; }
    public DateTimeOffset? ReviewRequestedDate { get; set; }
    public DateTimeOffset? ClosedDate { get; set; }
    public DateTimeOffset? DeletedDate { get; set; }
    public DateTimeOffset? LiveDate { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? ClosingDate { get; set; }
    public int ReviewCount { get; set; }
    public string? ApplicationUrl { get; set; }
    public ApplicationMethod? ApplicationMethod { get; set; }
    public string? ApplicationInstructions { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public string? AnonymousReason { get; set; }
    public bool? DisabilityConfident { get; set; }
    public string? ContactName { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public string? EmployerDescription { get; set; }
    public string? EmployerLocations { get; set; }
    public AvailableWhere? EmployerLocationOption { get; set; }
    public string? EmployerLocationInformation { get; set; }
    public string? EmployerName { get; set; }
    public EmployerNameOption? EmployerNameOption { get; set; }
    public string? EmployerRejectedReason { get; set; }
    public string? LegalEntityName { get; set; }
    public string? EmployerWebsiteUrl { get; set; }
    public GeoCodeMethod? GeoCodeMethod { get; set; }
    public long? AccountLegalEntityId { get; set; }
    public int? NumberOfPositions { get; set; }
    public string? OutcomeDescription { get; set; }
    public string? ProgrammeId { get; set; }
    public string? Skills { get; set; }
    public string? Qualifications { get; set; }
    public string? ThingsToConsider { get; set; }
    public string? TrainingDescription { get; set; }
    public string? AdditionalTrainingDescription { get; set; }
    public int? Ukprn { get; set; }
    public string? TrainingProvider_Name { get; set; }
    public string? TrainingProvider_Address { get; set; }
    public int? Wage_Duration { get; set; }
    public DurationUnit? Wage_DurationUnit { get; set; }
    public string? Wage_WorkingWeekDescription { get; set; }
    public decimal? Wage_WeeklyHours { get; set; }
    public WageType? Wage_WageType { get; set; }
    public decimal? Wage_FixedWageYearlyAmount { get; set; }
    public string? Wage_WageAdditionalInformation { get; set; }
    public string? Wage_CompanyBenefitsInformation { get; set; }
    public ClosureReason? ClosureReason { get; set; }
    public string? TransferInfo { get; set; }
    public string? AdditionalQuestion1 { get; set; }
    public string? AdditionalQuestion2 { get; set; }
    public bool? HasSubmittedAdditionalQuestions { get; set; }
    public bool? HasChosenProviderContactDetails { get; set; }
    public bool? HasOptedToAddQualifications { get; set; }
    public string? EmployerReviewFieldIndicators { get; set; }
    public string? ProviderReviewFieldIndicators { get; set; }
    public Guid? SubmittedByUserId { get; set; }
    public Guid? ReviewRequestedByUserId { get; set; }
}