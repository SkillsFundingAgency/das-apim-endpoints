using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Assessors.Application.Queries.GetStandardOptions;
using SFA.DAS.Assessors.InnerApi.Requests;
using SFA.DAS.Assessors.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Assessors.UnitTests.Application.Queries
{
    public class WhenHandlingGetStandardOptionsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_gets_standards_and_options_from_courses_api(GetStandardOptionsQuery query,
            GetStandardOptionsListResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetStandardOptionsQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetStandardOptionsListResponse>(It.IsAny<GetStandardOptionsListRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Standards.Should().BeEquivalentTo(apiResponse.Standards);
        }
    }
}
