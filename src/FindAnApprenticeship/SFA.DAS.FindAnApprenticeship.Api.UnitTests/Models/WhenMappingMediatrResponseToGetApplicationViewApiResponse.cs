using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplication;
using static SFA.DAS.FindAnApprenticeship.Api.Models.Applications.GetApplicationViewApiResponse;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;

public class WhenMappingMediatrResponseToGetApplicationViewApiResponse
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped(GetApplicationViewQueryResult source)
    {
        var actual = (GetApplicationViewApiResponse)source;

        actual.Candidate.Should().BeEquivalentTo(source.CandidateDetails);
    }
    
    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped_For_Null_Additional_Questions(GetApplicationViewQueryResult source)
    {
        source.ApplicationQuestions.AdditionalQuestion1 = null;
        source.ApplicationQuestions.AdditionalQuestion2 = null;
        
        var actual = (GetApplicationViewApiResponse)source;

        actual.Candidate.Should().BeEquivalentTo(source.CandidateDetails);
        actual.ApplicationQuestions.AdditionalQuestion1.Should().BeNull();
        actual.ApplicationQuestions.AdditionalQuestion2.Should().BeNull();
    }

    [Test, AutoData]
    public void Then_Candidate_The_Fields_Are_Mapped_To_CandidateDetailsSection(GetApplicationViewQueryResult.Candidate source)
    {
        var actual = (CandidateDetailsSection)source;

        actual.Should().BeEquivalentTo(source);
    }

    [Test, AutoData]
    public void Then_Education_The_Fields_Are_Mapped_To_EducationDetailsSection(GetApplicationViewQueryResult.EducationHistorySection source)
    {
        var actual = (EducationHistorySection)source;

        actual.Should().BeEquivalentTo(source);
    }

    [Test, AutoData]
    public void Then_Work_The_Fields_Are_Mapped_To_WorkDetailsSection(GetApplicationViewQueryResult.WorkHistorySection source)
    {
        var actual = (WorkHistorySection)source;

        actual.Should().BeEquivalentTo(source);
    }

    [Test, AutoData]
    public void Then_ApplicationQuestion_The_Fields_Are_Mapped_To_ApplicationQuestionDetailsSection(GetApplicationViewQueryResult.ApplicationQuestionsSection source)
    {
        var actual = (ApplicationQuestionsSection)source;

        actual.Should().BeEquivalentTo(source);
    }

    [Test, AutoData]
    public void Then_InterviewAdjustments_The_Fields_Are_Mapped_To_InterviewAdjustmentsSection(GetApplicationViewQueryResult.InterviewAdjustmentsSection source)
    {
        var actual = (InterviewAdjustmentsSection)source;

        actual.Should().BeEquivalentTo(source);
    }

    [Test, AutoData]
    public void Then_DisabilityConfident_The_Fields_Are_Mapped_To_InterviewAdjustmentsSection(GetApplicationViewQueryResult.DisabilityConfidenceSection source)
    {
        var actual = (DisabilityConfidenceSection)source;

        actual.Should().BeEquivalentTo(source);
    }

    [Test, AutoData]
    public void Then_WhatIsYourInterest_The_Fields_Are_Mapped_To_WhatIsYourInterestSection(GetApplicationViewQueryResult.WhatIsYourInterestSection source)
    {
        var actual = (WhatIsYourInterestSection)source;

        actual.Should().BeEquivalentTo(source);
    }

    [Test, AutoData]
    public void Then_AboutYouSection_The_Fields_Are_Mapped_To_AboutYouSection(GetApplicationViewQueryResult.AboutYouSection source)
    {
        var actual = (AboutYouSection)source;

        actual.Should().BeEquivalentTo(source);
    }

    [Test, AutoData]
    public void Then_VacancyDetailsSection_The_Fields_Are_Mapped_To_VacancyDetailsSection(GetApplicationViewQueryResult.VacancyDetailsSection source)
    {
        var actual = (VacancyDetailsSection)source;

        actual.Should().BeEquivalentTo(source);
    }
}