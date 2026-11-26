using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAvailableCoursesForProvider;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Standards.Queries.GetAvailableCoursesForProvider;

[TestFixture]
public class GetAvailableCoursesForProviderQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_EmptyProviderCourses_ReturnsAllStandards(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetAvailableCoursesForProviderQueryHandler sut,
        GetAvailableCoursesForProviderQueryResult expected,
        int ukprn)
    {
        GetAvailableCoursesForProviderQuery request = new(ukprn, CourseType.Apprenticeship);
        apiClientMock.Setup(a => a.Get<GetAvailableCoursesForProviderQueryResult>(It.IsAny<GetAvailableCoursesForProviderRequest>())).ReturnsAsync(expected);

        var result = await sut.Handle(request, new CancellationToken());

        Assert.That(result, Is.EqualTo(expected));
    }
}
