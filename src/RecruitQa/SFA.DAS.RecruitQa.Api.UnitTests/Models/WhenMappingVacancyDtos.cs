using FluentAssertions.Execution;
using SFA.DAS.RecruitQa.Api.Models;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.RecruitQa.InnerApi.Responses;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Models;

public class WhenMappingVacancyDtos
{
    [Test, AutoData]
    public void Then_ToInnerDto_Maps_All_Fields_From_PostVacancyRequest(PostVacancyRequest source)
    {
        var mapper = new VacancyMapper();

        var actual = mapper.ToInnerDto(source);

        using (new AssertionScope())
        {
            actual.AccountId.Should().Be(source.AccountId);
            actual.AccountLegalEntityId.Should().Be(source.AccountLegalEntityId);
            actual.AdditionalQuestion1.Should().Be(source.AdditionalQuestion1);
            actual.AdditionalQuestion2.Should().Be(source.AdditionalQuestion2);
            actual.AdditionalTrainingDescription.Should().Be(source.AdditionalTrainingDescription);
            actual.AnonymousReason.Should().Be(source.AnonymousReason);
            actual.ApplicationInstructions.Should().Be(source.ApplicationInstructions);
            actual.ApplicationMethod.Should().Be(source.ApplicationMethod);
            actual.ApplicationUrl.Should().Be(source.ApplicationUrl);
            actual.ApprenticeshipType.Should().Be(source.ApprenticeshipType);
            actual.ApprovedDate.Should().Be(source.ApprovedDate);
            actual.ClosedDate.Should().Be(source.ClosedDate);
            actual.ClosingDate.Should().Be(source.ClosingDate);
            actual.ClosureReason.Should().Be(source.ClosureReason);
            actual.Contact.Should().BeEquivalentTo(source.Contact);
            actual.CreatedDate.Should().Be(source.CreatedDate);
            actual.DeletedDate.Should().Be(source.DeletedDate);
            actual.Description.Should().Be(source.Description);
            actual.DisabilityConfident.Should().Be(source.DisabilityConfident);
            actual.EmployerDescription.Should().Be(source.EmployerDescription);
            actual.EmployerLocationInformation.Should().Be(source.EmployerLocationInformation);
            actual.EmployerLocationOption.Should().Be(source.EmployerLocationOption);
            actual.EmployerLocations.Should().BeEquivalentTo(source.EmployerLocations);
            actual.EmployerName.Should().Be(source.EmployerName);
            actual.EmployerNameOption.Should().Be(source.EmployerNameOption);
            actual.EmployerRejectedReason.Should().Be(source.EmployerRejectedReason);
            actual.EmployerReviewFieldIndicators.Should().BeEquivalentTo(source.EmployerReviewFieldIndicators);
            actual.EmployerWebsiteUrl.Should().Be(source.EmployerWebsiteUrl);
            actual.GeoCodeMethod.Should().Be(source.GeoCodeMethod);
            actual.HasChosenProviderContactDetails.Should().Be(source.HasChosenProviderContactDetails);
            actual.HasOptedToAddQualifications.Should().Be(source.HasOptedToAddQualifications);
            actual.HasSubmittedAdditionalQuestions.Should().Be(source.HasSubmittedAdditionalQuestions);
            actual.LastUpdatedDate.Should().Be(source.LastUpdatedDate);
            actual.LegalEntityName.Should().Be(source.LegalEntityName);
            actual.LiveDate.Should().Be(source.LiveDate);
            actual.NumberOfPositions.Should().Be(source.NumberOfPositions);
            actual.OutcomeDescription.Should().Be(source.OutcomeDescription);
            actual.OwnerType.Should().Be(source.OwnerType);
            actual.ProgrammeId.Should().Be(source.ProgrammeId);
            actual.ProviderReviewFieldIndicators.Should().BeEquivalentTo(source.ProviderReviewFieldIndicators);
            actual.Qualifications.Should().BeEquivalentTo(source.Qualifications);
            actual.ReviewCount.Should().Be(source.ReviewCount);
            actual.ReviewRequestedByUserId.Should().Be(source.ReviewRequestedByUserId);
            actual.ReviewRequestedDate.Should().Be(source.ReviewRequestedDate);
            actual.ShortDescription.Should().Be(source.ShortDescription);
            actual.Skills.Should().BeEquivalentTo(source.Skills);
            actual.SourceOrigin.Should().Be(source.SourceOrigin);
            actual.SourceType.Should().Be(source.SourceType);
            actual.SourceVacancyReference.Should().Be(source.SourceVacancyReference);
            actual.StartDate.Should().Be(source.StartDate);
            actual.Status.Should().Be(source.Status);
            actual.SubmittedByUserId.Should().Be(source.SubmittedByUserId);
            actual.SubmittedDate.Should().Be(source.SubmittedDate);
            actual.ThingsToConsider.Should().Be(source.ThingsToConsider);
            actual.Title.Should().Be(source.Title);
            actual.TrainingDescription.Should().Be(source.TrainingDescription);
            actual.TrainingProvider.Should().BeEquivalentTo(source.TrainingProvider);
            actual.TransferInfo.Should().BeEquivalentTo(source.TransferInfo);
            actual.VacancyReference.Should().Be(source.VacancyReference);
            actual.Wage.Should().BeEquivalentTo(source.Wage);
        }
    }

    [Test, AutoData]
    public void Then_ToOuterDto_Maps_All_Fields_From_PutVacancyResponse(PutVacancyResponse source)
    {
        var mapper = new VacancyMapper();

        var actual = mapper.ToOuterDto(source);

        using (new AssertionScope())
        {
            actual.Id.Should().Be(source.Id);
            actual.AccountId.Should().Be(source.AccountId);
            actual.AccountLegalEntityId.Should().Be(source.AccountLegalEntityId);
            actual.AdditionalQuestion1.Should().Be(source.AdditionalQuestion1);
            actual.AdditionalQuestion2.Should().Be(source.AdditionalQuestion2);
            actual.AdditionalTrainingDescription.Should().Be(source.AdditionalTrainingDescription);
            actual.AnonymousReason.Should().Be(source.AnonymousReason);
            actual.ApplicationInstructions.Should().Be(source.ApplicationInstructions);
            actual.ApplicationMethod.Should().Be(source.ApplicationMethod);
            actual.ApplicationUrl.Should().Be(source.ApplicationUrl);
            actual.ApprenticeshipType.Should().Be(source.ApprenticeshipType);
            actual.ApprovedDate.Should().Be(source.ApprovedDate);
            actual.ClosedDate.Should().Be(source.ClosedDate);
            actual.ClosingDate.Should().Be(source.ClosingDate);
            actual.ClosureReason.Should().Be(source.ClosureReason);
            actual.Contact.Should().BeEquivalentTo(source.Contact);
            actual.CreatedDate.Should().Be(source.CreatedDate);
            actual.DeletedDate.Should().Be(source.DeletedDate);
            actual.Description.Should().Be(source.Description);
            actual.DisabilityConfident.Should().Be(source.DisabilityConfident);
            actual.EmployerDescription.Should().Be(source.EmployerDescription);
            actual.EmployerLocationInformation.Should().Be(source.EmployerLocationInformation);
            actual.EmployerLocationOption.Should().Be(source.EmployerLocationOption);
            actual.EmployerLocations.Should().BeEquivalentTo(source.EmployerLocations);
            actual.EmployerName.Should().Be(source.EmployerName);
            actual.EmployerNameOption.Should().Be(source.EmployerNameOption);
            actual.EmployerRejectedReason.Should().Be(source.EmployerRejectedReason);
            actual.EmployerReviewFieldIndicators.Should().BeEquivalentTo(source.EmployerReviewFieldIndicators);
            actual.EmployerWebsiteUrl.Should().Be(source.EmployerWebsiteUrl);
            actual.GeoCodeMethod.Should().Be(source.GeoCodeMethod);
            actual.HasChosenProviderContactDetails.Should().Be(source.HasChosenProviderContactDetails);
            actual.HasOptedToAddQualifications.Should().Be(source.HasOptedToAddQualifications);
            actual.HasSubmittedAdditionalQuestions.Should().Be(source.HasSubmittedAdditionalQuestions);
            actual.LastUpdatedDate.Should().Be(source.LastUpdatedDate);
            actual.LegalEntityName.Should().Be(source.LegalEntityName);
            actual.LiveDate.Should().Be(source.LiveDate);
            actual.NumberOfPositions.Should().Be(source.NumberOfPositions);
            actual.OutcomeDescription.Should().Be(source.OutcomeDescription);
            actual.OwnerType.Should().Be(source.OwnerType!.Value);
            actual.ProgrammeId.Should().Be(source.ProgrammeId);
            actual.ProviderReviewFieldIndicators.Should().BeEquivalentTo(source.ProviderReviewFieldIndicators);
            actual.Qualifications.Should().BeEquivalentTo(source.Qualifications);
            actual.ReviewCount.Should().Be(source.ReviewCount);
            actual.ReviewDate.Should().Be(source.ReviewDate);
            actual.ShortDescription.Should().Be(source.ShortDescription);
            actual.Skills.Should().BeEquivalentTo(source.Skills);
            actual.SourceOrigin.Should().Be(source.SourceOrigin);
            actual.SourceType.Should().Be(source.SourceType);
            actual.SourceVacancyReference.Should().Be(source.SourceVacancyReference);
            actual.StartDate.Should().Be(source.StartDate);
            actual.Status.Should().Be(source.Status);
            actual.SubmittedByUserId.Should().Be(source.SubmittedByUserId);
            actual.SubmittedDate.Should().Be(source.SubmittedDate);
            actual.ThingsToConsider.Should().Be(source.ThingsToConsider);
            actual.Title.Should().Be(source.Title);
            actual.TrainingDescription.Should().Be(source.TrainingDescription);
            actual.TrainingProvider.Should().BeEquivalentTo(source.TrainingProvider);
            actual.TransferInfo.Should().BeEquivalentTo(source.TransferInfo);
            actual.VacancyReference.Should().Be(source.VacancyReference);
            actual.Wage.Should().BeEquivalentTo(source.Wage);
        }
    }
}
