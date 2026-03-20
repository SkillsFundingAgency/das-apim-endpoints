using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.Learners.Queries;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.Courses;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Services;

public class WhenMappingLearnerRecords
{
    [Test, MoqAutoData]
    public async Task Then_It_Maps_The_Main_Fields_Correctly(
        LearnerDataRecord inputDataRecord,
        List<GetAllStandardsResponse.TrainingProgramme> standards,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApi,
        [Greedy] MapLearnerRecords sut)
    {
        var result = await sut.Map([inputDataRecord], standards);

        result[0].Id.Should().Be(inputDataRecord.Id);
        result[0].FirstName.Should().Be(inputDataRecord.FirstName);
        result[0].LastName.Should().Be(inputDataRecord.LastName);
        result[0].Uln.Should().Be(inputDataRecord.Uln);
        result[0].StartDate.Should().Be(inputDataRecord.StartDate);
    }

    [Test, MoqAutoData]
    public async Task Then_It_Maps_The_Course_Field_From_List_When_TrainingName_Is_Null_Or_Empty(
        List<LearnerDataRecord> inputDataRecords,
        List<GetAllStandardsResponse.TrainingProgramme> courses,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApi,
        [Greedy] MapLearnerRecords sut
    )
    {
        inputDataRecords[0].TrainingName = null;
        inputDataRecords[0].TrainingCode = null;
        inputDataRecords[1].TrainingName = string.Empty;
        inputDataRecords[1].TrainingCode = null;
        courses[0].CourseCode = inputDataRecords[0].StandardCode.ToString();
        courses[1].CourseCode = inputDataRecords[1].StandardCode.ToString();

        var result = await sut.Map(inputDataRecords, courses);

        result[0].Course.Should().Be(courses[0].Name);
        result[1].Course.Should().Be(courses[1].Name);
    }

    [Test, MoqAutoData]
    public async Task Then_It_uses_TrainingName_When_Present(
        LearnerDataRecord learner,
        List<GetAllStandardsResponse.TrainingProgramme> courses,
        [Greedy] MapLearnerRecords sut)
    {
        // Arrange
        learner.TrainingName = "Unit course name from ILR";
        learner.TrainingCode = "ZSC00002";

        // Act
        var result = await sut.Map([learner], courses);

        // Assert
        result[0].Course.Should().Be("Unit course name from ILR");
    }
}