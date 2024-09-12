using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.InnerApi.Courses.Requests;

namespace SFA.DAS.ApprenticeApp.UnitTests.InnerApi.Courses.Requests
{
    public class StandardOptionKsbsRequestTests
    {
        [Test, AutoData]
        public void TestUrlIsCorrectlyBuilt()
        {
            string standardUid = "TestStandardUid";
            string option = "TestOption";
            var instance = new GetStandardOptionKsbsRequest(standardUid, option);

            instance.GetUrl.Should().Be($"api/courses/Standards/TestStandardUid/options/TestOption/ksbs");
        }
    }
}
