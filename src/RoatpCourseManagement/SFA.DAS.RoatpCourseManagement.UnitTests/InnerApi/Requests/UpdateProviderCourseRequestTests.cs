using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
<<<<<<< HEAD:src/RoatpCourseManagement/SFA.DAS.RoatpCourseManagement.UnitTests/InnerApi/Requests/UpdateProviderCourseRequestTests.cs
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;
=======
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.UpdateContactDetails;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
>>>>>>> 06c8c044 (More separated pipelines, rename RoatpCourseManagement for alignment, clear up -old bits):src/RoatpCourseManagement/SFA.DAS.RoatpCourseManagement.UnitTests/InnerApi/Requests/UpdateContactDetailsRequestTests.cs

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class UpdateProviderCourseRequestTests
    {
        [Test, AutoData]
        public void Constructor_ConstructsRequest(ProviderCourseUpdateModel data)
        {
            var request = new UpdateProviderCourseRequest(data);

            request.Data.Should().Be(data);
            request.Ukprn.Should().Be(data.Ukprn);
            request.LarsCode.Should().Be(data.LarsCode);
            request.PostUrl.Should().Be($"providers/{data.Ukprn}/courses/{data.LarsCode}/");
        }
    }
}
