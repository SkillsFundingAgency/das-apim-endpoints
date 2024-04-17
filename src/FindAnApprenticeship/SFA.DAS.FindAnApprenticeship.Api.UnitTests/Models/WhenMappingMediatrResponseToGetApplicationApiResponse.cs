using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplication;
using static SFA.DAS.FindAnApprenticeship.Api.Models.Applications.GetApplicationApiResponse;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;

public class WhenMappingMediatrResponseToGetApplicationApiResponse
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped(GetApplicationQueryResult source)
    {
        var actual = (GetApplicationApiResponse)source;

        actual.Candidate.Should().BeEquivalentTo(source.CandidateDetails);
    }
    
    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped_For_Null_Additional_Questions(GetApplicationQueryResult source)
    {
        source.ApplicationQuestions.AdditionalQuestion1 = null;
        source.ApplicationQuestions.AdditionalQuestion2 = null;
        
        var actual = (GetApplicationApiResponse)source;

        actual.Candidate.Should().BeEquivalentTo(source.CandidateDetails);
        actual.ApplicationQuestions.AdditionalQuestion1.Should().BeNull();
        actual.ApplicationQuestions.AdditionalQuestion2.Should().BeNull();
    }

    [Test, AutoData]
    public void Then_Candidate_The_Fields_Are_Mapped_To_CandidateDetailsSection(GetApplicationQueryResult.Candidate source)
    {
        var actual = (CandidateDetailsSection)source;

        actual.Should().BeEquivalentTo(source);
    }

    [Test, AutoData]
    public void Then_Education_The_Fields_Are_Mapped_To_EducationDetailsSection(GetApplicationQueryResult.EducationHistorySection source)
    {
        var actual = (EducationHistorySection)source;

        actual.Should().BeEquivalentTo(source);
    }

    [Test, AutoData]
    public void Then_Work_The_Fields_Are_Mapped_To_WorkDetailsSection(GetApplicationQueryResult.WorkHistorySection source)
    {
        var actual = (WorkHistorySection)source;

        actual.Should().BeEquivalentTo(source);
    }

    [Test, AutoData]
    public void Then_ApplicationQuestion_The_Fields_Are_Mapped_To_ApplicationQuestionDetailsSection(GetApplicationQueryResult.ApplicationQuestionsSection source)
    {
        var actual = (ApplicationQuestionsSection)source;

        actual.Should().BeEquivalentTo(source);
    }

    [Test, AutoData]
    public void Then_InterviewAdjustments_The_Fields_Are_Mapped_To_InterviewAdjustmentsSection(GetApplicationQueryResult.InterviewAdjustmentsSection source)
    {
        var actual = (InterviewAdjustmentsSection)source;

        actual.Should().BeEquivalentTo(source);
    }

    [Test, AutoData]
    public void Then_DisabilityConfident_The_Fields_Are_Mapped_To_InterviewAdjustmentsSection(GetApplicationQueryResult.DisabilityConfidenceSection source)
    {
        var actual = (DisabilityConfidenceSection)source;

        actual.Should().BeEquivalentTo(source);
    }

    [Test, AutoData]
    public void Then_WhatIsYourInterest_The_Fields_Are_Mapped_To_WhatIsYourInterestSection(GetApplicationQueryResult.WhatIsYourInterestSection source)
    {
        var actual = (WhatIsYourInterestSection)source;

        actual.Should().BeEquivalentTo(source);
    }

    [Test, AutoData]
    public void Then_AboutYouSection_The_Fields_Are_Mapped_To_AboutYouSection(GetApplicationQueryResult.AboutYouSection source)
    {
        var actual = (AboutYouSection)source;

        actual.Should().BeEquivalentTo(source);
    }
}