using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.Courses;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;

using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Services;

public class WhenMappingLearnerCourses
{
    [Test, MoqAutoData]
    public void Then_CourseName_Is_Present_Take_Course_TrainingName(
        Course course,
        List<GetAllStandardsResponse.TrainingProgramme> standards,
        [Greedy] MapLearnerRecords sut)
    {
        var result = sut.PopulateMissingTrainingNames(new List<Course> { course }, standards);

        result[0].TrainingCode.Should().Be(course.TrainingCode);
        result[0].TrainingName.Should().Be(course.TrainingName);
    }

    [Test, MoqAutoData]
    public void Then_CourseName_Is_Not_Present_Take_TrainingProgramme_TrainingName(
        Course course,
        List<GetAllStandardsResponse.TrainingProgramme> standards,
        [Greedy] MapLearnerRecords sut)
    {
        course.TrainingCode = standards[0].CourseCode;
        course.TrainingName = null;

        var result = sut.PopulateMissingTrainingNames(new List<Course> { course }, standards);

        result[0].TrainingCode.Should().Be(course.TrainingCode);
        result[0].TrainingName.Should().Be(standards[0].Name);
    }
}