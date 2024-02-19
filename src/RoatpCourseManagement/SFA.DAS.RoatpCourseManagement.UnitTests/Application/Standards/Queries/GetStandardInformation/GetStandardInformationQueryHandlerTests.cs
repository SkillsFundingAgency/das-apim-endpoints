using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetStandardInformation;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Standards.Queries.GetStandardInformation
{
    [TestFixture]
    public class GetStandardInformationQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_FoundCourseInformation_ReturnsResult(
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClientMock,
            GetStandardInformationQueryHandler sut,
            GetStandardInformationQuery query,
            GetStandardResponse apiResponse)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<GetStandardResponse>(It.Is<GetStandardRequest>(r => r.LarsCode == query.LarsCode && r.GetUrl == $"api/courses/Standards/{query.LarsCode}"))).ReturnsAsync(new ApiResponse<GetStandardResponse>(apiResponse, HttpStatusCode.OK, null));

            var result = await sut.Handle(query, new CancellationToken());

            result.Should().BeEquivalentTo(apiResponse, option =>
            {
                option.WithMapping<GetStandardInformationQueryResult>(s => s.ApprovalBody, m => m.RegulatorName);
                option.WithMapping<GetStandardInformationQueryResult>(s => s.Route, m => m.Sector);
                option.Excluding(s => s.SectorSubjectAreaTier1);
                return option;
            });
        }

        [Test, MoqAutoData]
        public async Task Handle_UnsuccessfulApiResponse_ThrowsException(
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClientMock,
            GetStandardInformationQueryHandler sut,
            GetStandardInformationQuery query)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<GetStandardResponse>(It.IsAny<GetStandardRequest>())).ReturnsAsync(new ApiResponse<GetStandardResponse>(null, HttpStatusCode.NotFound, null));

            Func<Task> action = () => sut.Handle(query, new CancellationToken());

            await action.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
