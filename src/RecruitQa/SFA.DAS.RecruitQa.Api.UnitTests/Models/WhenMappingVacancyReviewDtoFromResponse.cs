using AutoFixture;
using FluentAssertions.Execution;
using SFA.DAS.RecruitQa.Api.Models;
using SFA.DAS.RecruitQa.InnerApi.Responses;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Models;

public class WhenMappingVacancyReviewDtoFromResponse
{
    [Test, AutoData]
    public void Then_All_Fields_Are_Mapped_From_GetVacancyReviewResponse(GetVacancyReviewResponse source, long reference)
    {
        // Arrange 
        source.VacancyReference = $"VAC{reference}";

        // Act
        var actual = (VacancyReviewDto)source;

        // Assert
        using (new AssertionScope())
        {
            actual.Id.Should().Be(source.Id);
            actual.VacancyReference.Should().Be(reference);
            actual.VacancyTitle.Should().Be(source.VacancyTitle);
            actual.CreatedDate.Should().Be(source.CreatedDate);
            actual.SlaDeadLine.Should().Be(source.SlaDeadLine);
            actual.ReviewedDate.Should().Be(source.ReviewedDate);
            actual.Status.Should().Be(source.Status);
            actual.SubmissionCount.Should().Be(source.SubmissionCount);
            actual.ReviewedByUserEmail.Should().Be(source.ReviewedByUserEmail);
            actual.SubmittedByUserEmail.Should().Be(source.SubmittedByUserEmail);
            actual.ClosedDate.Should().Be(source.ClosedDate);
            actual.ManualOutcome.Should().Be(source.ManualOutcome);
            actual.ManualQaComment.Should().Be(source.ManualQaComment);
            actual.ManualQaFieldIndicators.Should().BeEquivalentTo(source.ManualQaFieldIndicators);
            actual.AutomatedQaOutcome.Should().Be(source.AutomatedQaOutcome);
            actual.AutomatedQaOutcomeIndicators.Should().Be(source.AutomatedQaOutcomeIndicators);
            actual.DismissedAutomatedQaOutcomeIndicators.Should().BeEquivalentTo(source.DismissedAutomatedQaOutcomeIndicators);
            actual.UpdatedFieldIdentifiers.Should().BeEquivalentTo(source.UpdatedFieldIdentifiers);
            actual.VacancySnapshot.Should().Be(source.VacancySnapshot);
            actual.OwnerType.Should().Be(source.OwnerType);
            actual.VacancyId.Should().Be(source.VacancyId);
        }
    }
}
