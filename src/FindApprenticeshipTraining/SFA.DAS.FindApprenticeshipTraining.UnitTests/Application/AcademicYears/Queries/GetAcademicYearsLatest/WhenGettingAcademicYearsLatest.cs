using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.AcademicYears.Queries.GetLatest;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.AcademicYears.Queries.GetAcademicYearsLatest;
public class WhenGettingAcademicYearsLatest
{
    [Test]
    [MoqAutoData]
    public async Task GetAcademicYearsLatest_ReturnsResult(
        GetAcademicYearsLatestQuery query,
        GetAcademicYearsLatestQueryResponse response,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpV2ApiClient,
        GetAcademicYearsLatestQueryHandler sut
    )
    {
        mockRoatpV2ApiClient.Setup(client =>
                client.Get<GetAcademicYearsLatestQueryResponse>(
                    It.IsAny<GetAcademicYearsLatestRequest>()
                )
            )
            .ReturnsAsync(response);

        var result = await sut.Handle(query, CancellationToken.None);

        result.QarPeriod.Should().Be(response.QarPeriod);
        result.ReviewPeriod.Should().Be(response.ReviewPeriod);
    }
}
