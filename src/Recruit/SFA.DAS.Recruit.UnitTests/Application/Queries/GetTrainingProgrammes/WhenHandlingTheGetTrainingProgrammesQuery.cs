using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Recruit.Application.Queries.GetTrainingProgrammes;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetTrainingProgrammes
{
    public class WhenHandlingTheGetTrainingProgrammesQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Frameworks_From_Courses_Api(
            GetTrainingProgrammesQuery query,
            GetFrameworksListResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetTrainingProgrammesQueryHandler handler)
        {
            foreach (var framework in apiResponse.Frameworks)
            {
                framework.Level = (int) ApprenticeshipLevel.Advanced;
            }

            mockApiClient
                .Setup(client => client.Get<GetFrameworksListResponse>(It.IsAny<GetFrameworksRequest>()))
                .ReturnsAsync(apiResponse);
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetActiveStandardsListRequest>()))
                .ReturnsAsync(new GetStandardsListResponse());

            var result = await handler.Handle(query, CancellationToken.None);

            result.TrainingProgrammes.Should().BeEquivalentTo(apiResponse.Frameworks.Select(item => (TrainingProgramme)item));
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_From_Courses_Api(
            GetTrainingProgrammesQuery query,
            GetStandardsListResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetTrainingProgrammesQueryHandler handler)
        {
            foreach (var standard in apiResponse.Standards)
            {
                standard.Level = (int) ApprenticeshipLevel.Advanced;
            }
            mockApiClient
                .Setup(client => client.Get<GetFrameworksListResponse>(It.IsAny<GetFrameworksRequest>()))
                .ReturnsAsync(new GetFrameworksListResponse());
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetActiveStandardsListRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.TrainingProgrammes.Should().BeEquivalentTo(apiResponse.Standards.Select(item => (TrainingProgramme)item));
        }
    }
}
