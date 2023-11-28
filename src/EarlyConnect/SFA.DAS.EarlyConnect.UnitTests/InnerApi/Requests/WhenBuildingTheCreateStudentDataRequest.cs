using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheCreateStudentDataRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(StudentDataList studentDataList)
        {
            var actual = new CreateStudentDataRequest(studentDataList);

            actual.PostUrl.Should().Be("api/student-data");
        }
    }
}