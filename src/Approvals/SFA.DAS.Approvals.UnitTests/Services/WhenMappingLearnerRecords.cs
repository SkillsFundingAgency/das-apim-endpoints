using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Services
{
    public class WhenMappingLearnerRecords
    {
        [Test, MoqAutoData]
        public async Task Then_It_Maps_The_Main_Fields_Correctly(
            LearnerDataRecord inputDataRecord,
            GetStandardsListResponse standardCourses,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApi,
            [Greedy] MapLearnerRecords sut
           )
        {

            coursesApi.Setup(x => x.Get<GetStandardsListResponse>(It.IsAny<GetStandardsExportRequest>()))
                .ReturnsAsync(standardCourses);

            var result = await sut.Map(new List<LearnerDataRecord> { inputDataRecord });

            result[0].Id.Should().Be(inputDataRecord.Id);
            result[0].FirstName.Should().Be(inputDataRecord.FirstName);
            result[0].LastName.Should().Be(inputDataRecord.LastName);
            result[0].Uln.Should().Be(inputDataRecord.Uln);
        }

        [Test, MoqAutoData]
        public async Task Then_It_Maps_The_Course_Field_Correctly(
            LearnerDataRecord[] inputDataRecords,
            GetStandardsListResponse standardCourses,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApi,
            [Greedy] MapLearnerRecords sut
        )
        {
            var courses = standardCourses.Standards.ToList();
            courses[0].LarsCode = inputDataRecords[0].StandardCode;
            courses[1].LarsCode = inputDataRecords[1].StandardCode;

            coursesApi.Setup(x => x.Get<GetStandardsListResponse>(It.IsAny<GetStandardsExportRequest>()))
                .ReturnsAsync(standardCourses);

            var result = await sut.Map(inputDataRecords);

            result[0].Course.Should().Be(courses[0].Title);
            result[1].Course.Should().Be(courses[1].Title);
        }
    }
}
