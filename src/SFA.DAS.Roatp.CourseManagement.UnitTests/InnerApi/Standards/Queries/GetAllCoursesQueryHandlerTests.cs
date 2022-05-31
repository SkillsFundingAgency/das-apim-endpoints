
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetAllCoursesQuery;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Roatp.CourseManagement.UnitTests.InnerApi.Standards.Queries
{
    [TestFixture]
    public class GetAllCoursesQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_CallsInnerApi(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            List<GetAllCoursesResponse> courses,
            GetAllCoursesQuery query,
            GetAllCoursesQueryHandler sut)
        {
            //MFCMFC
            apiClientMock.Setup(c => c.Get<List<GetAllCoursesResponse>>(It.Is<GetAllCoursesQuery>(q => q == query))).ReturnsAsync(courses);
            var result = await sut.Handle(query, new CancellationToken());
            //result.ProviderLocations.Should().BeEquivalentTo(locations);
        }
    }
}
