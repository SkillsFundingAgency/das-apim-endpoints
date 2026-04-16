using System;
using System.Collections.Generic;
using System.Text.Json;
using SFA.DAS.Recruit.Domain.Vacancy;
using SFA.DAS.Recruit.GraphQL.RecruitInner.Mappers;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Models;
using GqlApplicationMethod = SFA.DAS.Recruit.GraphQL.ApplicationMethod;
using GqlApprenticeshipTypes = SFA.DAS.Recruit.GraphQL.ApprenticeshipTypes;
using GqlClosureReason = SFA.DAS.Recruit.GraphQL.ClosureReason;
using GqlEmployerNameOption = SFA.DAS.Recruit.GraphQL.EmployerNameOption;
using GqlGeoCodeMethod = SFA.DAS.Recruit.GraphQL.GeoCodeMethod;
using GqlOwnerType = SFA.DAS.Recruit.GraphQL.OwnerType;
using GqlSourceOrigin = SFA.DAS.Recruit.GraphQL.SourceOrigin;
using GqlSourceType = SFA.DAS.Recruit.GraphQL.SourceType;
using GqlDurationUnit = SFA.DAS.Recruit.GraphQL.DurationUnit;
using GqlWageType = SFA.DAS.Recruit.GraphQL.WageType;
using GqlAvailableWhere = SFA.DAS.Recruit.GraphQL.AvailableWhere;
using VacancyStatus = SFA.DAS.Recruit.GraphQL.VacancyStatus;

namespace SFA.DAS.Recruit.UnitTests.GraphQL.Mappers;

public class WhenMappingAVacancy
{
    [Test, MoqAutoData]
    public void Then_Simple_Fields_Are_Mapped_Correctly() // value properties, except enums
    {
        // arrange
        var source = AllVacancyFieldsFake.Create();
        
        // act
        var result = GqlVacancyMapper.From(source);
    
        // assert
        result.AccountId.Should().Be(source.AccountId);
        result.AccountLegalEntityId.Should().Be(source.AccountLegalEntityId);
        result.AdditionalQuestion1.Should().Be(source.AdditionalQuestion1);
        result.AdditionalQuestion2.Should().Be(source.AdditionalQuestion2);
        result.AdditionalTrainingDescription.Should().Be(source.AdditionalTrainingDescription);
        result.AnonymousReason.Should().Be(source.AnonymousReason);
        result.ApplicationInstructions.Should().Be(source.ApplicationInstructions);
        result.ApplicationUrl.Should().Be(source.ApplicationUrl);
        result.ApprovedDate.Should().Be(source.ApprovedDate!.Value.UtcDateTime);
        result.ClosedDate.Should().Be(source.ClosedDate!.Value.UtcDateTime);
        result.ClosingDate.Should().Be(source.ClosingDate!.Value.UtcDateTime);
        result.DeletedDate.Should().Be(source.DeletedDate!.Value.UtcDateTime);
        result.Description.Should().Be(source.Description);
        result.DisabilityConfident.Should().Be(source.DisabilityConfident);
        result.EmployerDescription.Should().Be(source.EmployerDescription);
        result.EmployerLocationInformation.Should().Be(source.EmployerLocationInformation);
        result.HasChosenProviderContactDetails.Should().Be(source.HasChosenProviderContactDetails);
        result.HasOptedToAddQualifications.Should().Be(source.HasOptedToAddQualifications);
        result.HasSubmittedAdditionalQuestions.Should().Be(source.HasSubmittedAdditionalQuestions);
        result.Id.Should().Be(source.Id);
        result.LastUpdatedDate.Should().Be(source.LastUpdatedDate!.Value.UtcDateTime);
        result.LegalEntityName.Should().Be(source.LegalEntityName);
        result.LiveDate.Should().Be(source.LiveDate!.Value.UtcDateTime);
        result.NumberOfPositions.Should().Be(source.NumberOfPositions);
        result.OutcomeDescription.Should().Be(source.OutcomeDescription);
        result.ProgrammeId.Should().Be(source.ProgrammeId);
        result.ReviewCount.Should().Be(source.ReviewCount);
        result.ReviewRequestedByUserId.Should().Be(source.ReviewRequestedByUserId);
        result.ReviewRequestedDate.Should().Be(source.ReviewRequestedDate!.Value.UtcDateTime);
        result.ShortDescription.Should().Be(source.ShortDescription);
        result.SourceVacancyReference.Should().Be(source.SourceVacancyReference);
        result.StartDate.Should().Be(source.StartDate!.Value.UtcDateTime);
        result.SubmittedByUserId.Should().Be(source.SubmittedByUserId);
        result.SubmittedDate.Should().Be(source.SubmittedDate!.Value.UtcDateTime);
        result.ThingsToConsider.Should().Be(source.ThingsToConsider);
        result.Title.Should().Be(source.Title);
        result.TrainingDescription.Should().Be(source.TrainingDescription);
        result.VacancyReference.Should().Be(source.VacancyReference);
    }

    [Test]
    [MoqInlineAutoData("name", null, null)]
    [MoqInlineAutoData(null, "email", null)]
    [MoqInlineAutoData(null, null, "phone")]
    [MoqInlineAutoData("name", "email", "phone")]
    public void Then_The_Contact_Is_Mapped_If_Any_Field_Is_Specified(string contactName, string contactEmail, string contactPhone)
    {
        // arrange
        var source = AllVacancyFieldsFake.Create();
        source.ContactName = contactName;
        source.ContactEmail = contactEmail;
        source.ContactPhone = contactPhone;

        // act
        var result = GqlVacancyMapper.From(source);

        // assert
        result.Contact.Should().NotBeNull();
        result.Contact.Name.Should().Be(contactName);
        result.Contact.Email.Should().Be(contactEmail);
        result.Contact.Phone.Should().Be(contactPhone);
    }
    
    [Test]
    public void Contact_Is_Not_Mapped()
    {
        // arrange
        var source = AllVacancyFieldsFake.Create();
        source.ContactName = null;
        source.ContactEmail = null;
        source.ContactPhone = null;

        // act
        var result = GqlVacancyMapper.From(source);

        // assert
        result.Contact.Should().BeNull();
    }

    [Test]
    [MoqInlineAutoData(GqlApplicationMethod.ThroughExternalApplicationSite, ApplicationMethod.ThroughExternalApplicationSite)]
    [MoqInlineAutoData(GqlApplicationMethod.ThroughFindAnApprenticeship, ApplicationMethod.ThroughFindAnApprenticeship)]
    [MoqInlineAutoData(GqlApplicationMethod.ThroughFindATraineeship, ApplicationMethod.ThroughFindATraineeship)]
    [MoqInlineAutoData(GqlApplicationMethod.Unspecified, ApplicationMethod.Unspecified)]
    [MoqInlineAutoData(null, null)]
    public void ApplicationMethod_Is_Mapped(GqlApplicationMethod? srcValue, ApplicationMethod? dstValue)
    {
        // arrange
        var source = AllVacancyFieldsFake.Create();
        source.ApplicationMethod = srcValue;

        // act
        var result = GqlVacancyMapper.From(source);

        // assert
        result.ApplicationMethod.Should().Be(dstValue);
    }
    
    [Test]
    [MoqInlineAutoData(GqlApprenticeshipTypes.Foundation, ApprenticeshipTypes.Foundation)]
    [MoqInlineAutoData(GqlApprenticeshipTypes.Standard, ApprenticeshipTypes.Standard)]
    [MoqInlineAutoData(null, null)]
    public void ApprenticeshipType_Is_Mapped(GqlApprenticeshipTypes? srcValue, ApprenticeshipTypes? dstValue)
    {
        // arrange
        var source = AllVacancyFieldsFake.Create();
        source.ApprenticeshipType = srcValue;

        // act
        var result = GqlVacancyMapper.From(source);

        // assert
        result.ApprenticeshipType.Should().Be(dstValue);
    }
    
    [Test]
    [MoqInlineAutoData(GqlClosureReason.Auto, ClosureReason.Auto)]
    [MoqInlineAutoData(GqlClosureReason.BlockedByQa, ClosureReason.BlockedByQa)]
    [MoqInlineAutoData(GqlClosureReason.Manual, ClosureReason.Manual)]
    [MoqInlineAutoData(GqlClosureReason.TransferredByEmployer, ClosureReason.TransferredByEmployer)]
    [MoqInlineAutoData(GqlClosureReason.TransferredByQa, ClosureReason.TransferredByQa)]
    [MoqInlineAutoData(GqlClosureReason.WithdrawnByQa, ClosureReason.WithdrawnByQa)]
    [MoqInlineAutoData(null, null)]
    public void ClosureReason_Is_Mapped(GqlClosureReason? srcValue, ClosureReason? dstValue)
    {
        // arrange
        var source = AllVacancyFieldsFake.Create();
        source.ClosureReason = srcValue;

        // act
        var result = GqlVacancyMapper.From(source);

        // assert
        result.ClosureReason.Should().Be(dstValue);
    }
    
    [Test]
    [MoqInlineAutoData(GqlEmployerNameOption.Anonymous, EmployerNameOption.Anonymous)]
    [MoqInlineAutoData(GqlEmployerNameOption.RegisteredName, EmployerNameOption.RegisteredName)]
    [MoqInlineAutoData(GqlEmployerNameOption.TradingName, EmployerNameOption.TradingName)]
    [MoqInlineAutoData(null, null)]
    public void EmployerNameOption_Is_Mapped(GqlEmployerNameOption? srcValue, EmployerNameOption? dstValue)
    {
        // arrange
        var source = AllVacancyFieldsFake.Create();
        source.EmployerNameOption = srcValue;

        // act
        var result = GqlVacancyMapper.From(source);

        // assert
        result.EmployerNameOption.Should().Be(dstValue);
    }
    
    [Test]
    [MoqInlineAutoData(GqlGeoCodeMethod.ExistingVacancy, GeoCodeMethod.ExistingVacancy)]
    [MoqInlineAutoData(GqlGeoCodeMethod.FailedToGeoCode, GeoCodeMethod.FailedToGeoCode)]
    [MoqInlineAutoData(GqlGeoCodeMethod.Loqate, GeoCodeMethod.Loqate)]
    [MoqInlineAutoData(GqlGeoCodeMethod.OuterApi, GeoCodeMethod.OuterApi)]
    [MoqInlineAutoData(GqlGeoCodeMethod.PostcodesIo, GeoCodeMethod.PostcodesIo)]
    [MoqInlineAutoData(GqlGeoCodeMethod.PostcodesIoOutcode, GeoCodeMethod.PostcodesIoOutcode)]
    [MoqInlineAutoData(GqlGeoCodeMethod.Unspecified, GeoCodeMethod.Unspecified)]
    [MoqInlineAutoData(null, null)]
    public void GeoCodeMethod_Is_Mapped(GqlGeoCodeMethod? srcValue, GeoCodeMethod? dstValue)
    {
        // arrange
        var source = AllVacancyFieldsFake.Create();
        source.GeoCodeMethod = srcValue;

        // act
        var result = GqlVacancyMapper.From(source);

        // assert
        result.GeoCodeMethod.Should().Be(dstValue);
    }
    
    [Test]
    [MoqInlineAutoData(GqlOwnerType.Employer, OwnerType.Employer)]
    [MoqInlineAutoData(GqlOwnerType.External, OwnerType.External)]
    [MoqInlineAutoData(GqlOwnerType.Provider, OwnerType.Provider)]
    [MoqInlineAutoData(GqlOwnerType.Unknown, OwnerType.Unknown)]
    [MoqInlineAutoData(null, null)]
    public void OwnerType_Is_Mapped(GqlOwnerType? srcValue, OwnerType? dstValue)
    {
        // arrange
        var source = AllVacancyFieldsFake.Create();
        source.OwnerType = srcValue;

        // act
        var result = GqlVacancyMapper.From(source);

        // assert
        result.OwnerType.Should().Be(dstValue);
    }
    
    [Test]
    [MoqInlineAutoData(GqlSourceOrigin.Api, SourceOrigin.Api)]
    [MoqInlineAutoData(GqlSourceOrigin.EmployerWeb, SourceOrigin.EmployerWeb)]
    [MoqInlineAutoData(GqlSourceOrigin.ProviderWeb, SourceOrigin.ProviderWeb)]
    [MoqInlineAutoData(GqlSourceOrigin.WebComplaint, SourceOrigin.WebComplaint)]
    [MoqInlineAutoData(null, null)]
    public void SourceOrigin_Is_Mapped(GqlSourceOrigin? srcValue, SourceOrigin? dstValue)
    {
        // arrange
        var source = AllVacancyFieldsFake.Create();
        source.SourceOrigin = srcValue;

        // act
        var result = GqlVacancyMapper.From(source);

        // assert
        result.SourceOrigin.Should().Be(dstValue);
    }
    
    [Test]
    [MoqInlineAutoData(GqlSourceType.Clone, SourceType.Clone)]
    [MoqInlineAutoData(GqlSourceType.Extension, SourceType.Extension)]
    [MoqInlineAutoData(GqlSourceType.New, SourceType.New)]
    [MoqInlineAutoData(null, null)]
    public void SourceType_Is_Mapped(GqlSourceType? srcValue, SourceType? dstValue)
    {
        // arrange
        var source = AllVacancyFieldsFake.Create();
        source.SourceType = srcValue;

        // act
        var result = GqlVacancyMapper.From(source);

        // assert
        result.SourceType.Should().Be(dstValue);
    }
    
    [Test, MoqAutoData]
    public void EmployerReviewFieldIndicators_Are_Mapped(List<ReviewFieldIndicator> reviewFieldIndicators)
    {
        // arrange
        var source = AllVacancyFieldsFake.Create();
        source.EmployerReviewFieldIndicators = JsonSerializer.Serialize(reviewFieldIndicators, Global.JsonSerializerOptions);

        // act
        var result = GqlVacancyMapper.From(source);

        // assert
        result.EmployerReviewFieldIndicators.Should().BeEquivalentTo(reviewFieldIndicators);
    }
    
    [Test, MoqAutoData]
    public void ProviderReviewFieldIndicators_Are_Mapped(List<ReviewFieldIndicator> reviewFieldIndicators)
    {
        // arrange
        var source = AllVacancyFieldsFake.Create();
        source.ProviderReviewFieldIndicators = JsonSerializer.Serialize(reviewFieldIndicators, Global.JsonSerializerOptions);

        // act
        var result = GqlVacancyMapper.From(source);

        // assert
        result.ProviderReviewFieldIndicators.Should().BeEquivalentTo(reviewFieldIndicators);
    }
    
    [Test, MoqAutoData]
    public void Qualifications_Are_Mapped(List<Qualification> qualifications)
    {
        // arrange
        var source = AllVacancyFieldsFake.Create();
        source.Qualifications = JsonSerializer.Serialize(qualifications, Global.JsonSerializerOptions);

        // act
        var result = GqlVacancyMapper.From(source);

        // assert
        result.Qualifications.Should().BeEquivalentTo(qualifications);
    }
    
    
    [Test, MoqAutoData]
    public void Skills_Are_Mapped(List<string> skills)
    {
        // arrange
        var source = AllVacancyFieldsFake.Create();
        source.Skills = JsonSerializer.Serialize(skills, Global.JsonSerializerOptions);

        // act
        var result = GqlVacancyMapper.From(source);

        // assert
        result.Skills.Should().BeEquivalentTo(skills);
    }
    
    [Test, MoqAutoData]
    public void TrainingProvider_Is_Mapped(TrainingProvider trainingProvider)
    {
        // arrange
        var source = AllVacancyFieldsFake.Create();
        source.Ukprn = (int)trainingProvider.Ukprn!.Value;
        source.TrainingProvider_Name = trainingProvider.Name;
        source.TrainingProvider_Address = JsonSerializer.Serialize(trainingProvider.Address, Global.JsonSerializerOptions);

        // act
        var result = GqlVacancyMapper.From(source);

        // assert
        result.TrainingProvider.Should().BeEquivalentTo(trainingProvider);
    }
    
    [Test, MoqAutoData]
    public void TransferInfo_Is_Mapped(TransferInfo transferInfo)
    {
        // arrange
        var source = AllVacancyFieldsFake.Create();
        source.TransferInfo = JsonSerializer.Serialize(transferInfo, Global.JsonSerializerOptions);
        
        // act
        var result = GqlVacancyMapper.From(source);

        // assert
        result.TransferInfo.Should().BeEquivalentTo(transferInfo);
    }
    
    [Test, MoqAutoData]
    public void Wage_Is_Mapped(Wage wage)
    {
        // arrange
        var source = AllVacancyFieldsFake.Create();
        source.Wage_CompanyBenefitsInformation = wage.CompanyBenefitsInformation;
        source.Wage_Duration = wage.Duration;
        source.Wage_DurationUnit = (GqlDurationUnit)Enum.Parse(typeof(DurationUnit), wage.DurationUnit.ToString()!);
        source.Wage_FixedWageYearlyAmount = wage.FixedWageYearlyAmount;
        source.Wage_WageAdditionalInformation = wage.WageAdditionalInformation;
        source.Wage_WageType = (GqlWageType)Enum.Parse(typeof(WageType), wage.WageType.ToString()!);
        source.Wage_WeeklyHours = wage.WeeklyHours;
        source.Wage_WorkingWeekDescription = wage.WorkingWeekDescription;
            
        // act
        var result = GqlVacancyMapper.From(source);

        // assert
        result.Wage.Should().BeEquivalentTo(wage);
    }
    
    
    [Test]
    [MoqInlineAutoData(GqlAvailableWhere.AcrossEngland, AvailableWhere.AcrossEngland)]
    [MoqInlineAutoData(GqlAvailableWhere.MultipleLocations, AvailableWhere.MultipleLocations)]
    [MoqInlineAutoData(GqlAvailableWhere.OneLocation, AvailableWhere.OneLocation)]
    public void EmployerLocationOption_Is_Mapped(GqlAvailableWhere? srcValue, AvailableWhere? dstValue)
    {
        // arrange
        var source = AllVacancyFieldsFake.Create();
        source.EmployerLocationOption = srcValue;

        // act
        var result = GqlVacancyMapper.From(source);

        // assert
        result.EmployerLocationOption.Should().Be(dstValue);
    }
    
    [Test, MoqAutoData]
    public void Then_Location_Option_Is_Mapped_To_Null_When_Not_Set()
    {
        // arrange
        var source = AllVacancyFieldsFake.Create();
        source.EmployerLocationOption = null;
        source.Status = VacancyStatus.Draft;

        // act
        var result = GqlVacancyMapper.From(source);

        // assert
        result.EmployerLocationOption.Should().BeNull();
    }
    
    [Test, MoqAutoData]
    public void Then_Location_Option_Is_Mapped_To_Multiple()
    {
        // arrange
        var source = AllVacancyFieldsFake.Create();
        source.EmployerLocationOption = null;
        source.Status = VacancyStatus.Live;

        // act
        var result = GqlVacancyMapper.From(source);

        // assert
        result.EmployerLocationOption.Should().Be(AvailableWhere.MultipleLocations);
    }
    
    [Test, MoqAutoData]
    public void Then_Location_Option_Is_Mapped_To_Single(Address address)
    {
        // arrange
        var source = AllVacancyFieldsFake.Create();
        source.EmployerLocationOption = null;
        source.Status = VacancyStatus.Live;
        source.EmployerLocations = JsonSerializer.Serialize(new List<Address> { address }, Global.JsonSerializerOptions);

        // act
        var result = GqlVacancyMapper.From(source);

        // assert
        result.EmployerLocationOption.Should().Be(AvailableWhere.OneLocation);
    }
    
    [Test, MoqAutoData]
    public void Then_Location_Option_Is_Mapped_To_National(Address address)
    {
        // arrange
        var source = AllVacancyFieldsFake.Create();
        source.EmployerLocationOption = null;
        source.Status = VacancyStatus.Live;
        source.EmployerLocations = null;

        // act
        var result = GqlVacancyMapper.From(source);

        // assert
        result.EmployerLocationOption.Should().Be(AvailableWhere.AcrossEngland);
    }
}