using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Assessors.Application.Queries.GetStandardDetails;
using SFA.DAS.Assessors.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Assessors.UnitTests.Application.Queries
{
    public class WhenHandlingGetStandardDetailsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Specific_Standard_From_Courses_Api(
            GetStandardDetailsQuery query,
            StandardDetailResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetStandardDetailsQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<StandardDetailResponse>(It.IsAny<GetStandardDetailsByStandardUIdRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.StandardDetails.Should().BeEquivalentTo(apiResponse);
        }

        [Test, MoqAutoData]
        public async Task And_There_Is_No_Funding_Data_Then_Map_MaxFunding_From_VersionDetail(
            GetStandardDetailsQuery query,
            StandardDetailResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetStandardDetailsQueryHandler handler)
        {
            apiResponse.ApprenticeshipFunding = null;
            mockApiClient
                .Setup(client => client.Get<StandardDetailResponse>(It.IsAny<GetStandardDetailsByStandardUIdRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.StandardDetails.MaxFunding.Should().Be(apiResponse.VersionDetail.MaxFunding);
        }

        [Test, MoqAutoData]
        public async Task And_There_Is_No_Funding_Data_Then_Map_TypicalDuration_From_VersionDetail(
            GetStandardDetailsQuery query,
            StandardDetailResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetStandardDetailsQueryHandler handler)
        {
            apiResponse.ApprenticeshipFunding = null;
            mockApiClient
                .Setup(client => client.Get<StandardDetailResponse>(It.IsAny<GetStandardDetailsByStandardUIdRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.StandardDetails.TypicalDuration.Should().Be(apiResponse.VersionDetail.TypicalDuration);
        }
    }
}