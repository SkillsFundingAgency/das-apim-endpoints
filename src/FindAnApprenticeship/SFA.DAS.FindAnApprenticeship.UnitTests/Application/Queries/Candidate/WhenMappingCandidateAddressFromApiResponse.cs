using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplication;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplication.GetApplicationQueryResult;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Candidate;

public class WhenMappingCandidateAddressFromApiResponse
{
    [Test, MoqAutoData]
    public void Then_Fields_Are_Mapped_Correctly(GetAddressApiResponse source)
    {
        var actual = (GetApplicationQueryResult.CandidateAddress)source;

        actual.Should().BeEquivalentTo(source, options => options.Excluding(fil => fil.CandidateId));
    }

    [Test, AutoData]
    public void Then_Education_The_Fields_Are_Mapped_To_EducationDetailsSection(GetTrainingCoursesApiResponse.TrainingCourseItem source)
    {
        var actual = (EducationHistorySection.TrainingCourse)source;

        actual.Should().BeEquivalentTo(source);
    }

    [Test, AutoData]
    public void Then_Work_The_Fields_Are_Mapped_To_WorkDetailsSection(GetWorkHistoriesApiResponse.WorkHistoryItem source)
    {
        var actual = (WorkHistorySection.Job)source;

        actual.Should().BeEquivalentTo(source, options => options.Excluding(fil => fil.WorkHistoryType));
    }

    [Test, AutoData]
    public void Then_Volunteering_The_Fields_Are_Mapped_To_WorkDetailsSection(GetWorkHistoriesApiResponse.WorkHistoryItem source)
    {
        var actual = (WorkHistorySection.VolunteeringAndWorkExperience)source;

        actual.Should().BeEquivalentTo(source, options => options.Excluding(fil => fil.WorkHistoryType));
    }
}