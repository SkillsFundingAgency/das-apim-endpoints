﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplication.GetApplicationQueryResult;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Candidate;

public class WhenMappingCandidateAddressFromApiResponse
{
    [Test, MoqAutoData]
    public void Then_Fields_Are_Mapped_Correctly(GetAddressApiResponse source)
    {
        var actual = (CandidateAddress)source;

        actual.Should().BeEquivalentTo(source, options => options.Excluding(fil => fil.CandidateId).Excluding(c=>c.Uprn));
    }

    [Test, AutoData]
    public void Then_Education_The_Fields_Are_Mapped_To_EducationDetailsSection(GetTrainingCourseApiResponse source)
    {
        var actual = (EducationHistorySection.TrainingCourse)source;

        actual.Should().BeEquivalentTo(source, options => options.Excluding(fil => fil.ApplicationId));
    }

    [Test, AutoData]
    public void Then_Work_The_Fields_Are_Mapped_To_WorkDetailsSection(GetWorkHistoryItemApiResponse source)
    {
        var actual = (WorkHistorySection.Job)source;

        actual.Should().BeEquivalentTo(source, options => options
            .Excluding(fil => fil.WorkHistoryType)
            .Excluding(fil => fil.ApplicationId)
        );
    }

    [Test, AutoData]
    public void Then_Volunteering_The_Fields_Are_Mapped_To_WorkDetailsSection(GetWorkHistoryItemApiResponse source)
    {
        var actual = (WorkHistorySection.VolunteeringAndWorkExperience)source;

        actual.Should().BeEquivalentTo(source, options => options
            .Excluding(fil => fil.WorkHistoryType)
            .Excluding(fil => fil.ApplicationId)
        );
    }
}