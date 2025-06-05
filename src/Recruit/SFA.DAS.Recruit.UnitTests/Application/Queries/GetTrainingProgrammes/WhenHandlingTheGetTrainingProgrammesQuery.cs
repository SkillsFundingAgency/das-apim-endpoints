using System.Collections.Generic;
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
            query.IncludeFoundationApprenticeships = false;
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
            List<GetStandardsListItem> apprenticeshipStandards,
            List<GetStandardsListItem> foundationStandards,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetTrainingProgrammesQueryHandler handler)
        {
            foreach (var standard in apprenticeshipStandards)
            {
                standard.Level = (int) ApprenticeshipLevel.Advanced;
                standard.ApprenticeshipType = "Apprenticeship";
            }
            foreach (var standard in foundationStandards)
            {
                standard.Level = (int) ApprenticeshipLevel.Intermediate;
                standard.ApprenticeshipType = "Foundation";
            }

            var getStandardsListItems = new List<GetStandardsListItem>();
            getStandardsListItems.AddRange(apprenticeshipStandards);
            getStandardsListItems.AddRange(foundationStandards);
            apiResponse.Standards = getStandardsListItems;
            mockApiClient
                .Setup(client => client.Get<GetFrameworksListResponse>(It.IsAny<GetFrameworksRequest>()))
                .ReturnsAsync(new GetFrameworksListResponse());
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetActiveStandardsListRequest>()))
                .ReturnsAsync(apiResponse);

            query.IncludeFoundationApprenticeships = false;

            var result = await handler.Handle(query, CancellationToken.None);

            result.TrainingProgrammes.Should().BeEquivalentTo(apprenticeshipStandards.Select(item => (TrainingProgramme)item));
        }
        
        [Test, MoqAutoData]
        public async Task Then_Gets_Filtered_Standards_From_Courses_Api(
            GetTrainingProgrammesQuery query,
            GetStandardsListResponse apiResponse,
            List<GetStandardsListItem> apprenticeshipStandards,
            List<GetStandardsListItem> foundationStandards,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetTrainingProgrammesQueryHandler handler)
        {
            foreach (var standard in apprenticeshipStandards)
            {
                standard.Level = (int) ApprenticeshipLevel.Advanced;
                standard.ApprenticeshipType = "Apprenticeship";
            }
            foreach (var standard in foundationStandards)
            {
                standard.Level = (int) ApprenticeshipLevel.Intermediate;
                standard.ApprenticeshipType = "Foundation";
            }

            var getStandardsListItems = new List<GetStandardsListItem>();
            getStandardsListItems.AddRange(apprenticeshipStandards);
            getStandardsListItems.AddRange(foundationStandards);
            apiResponse.Standards = getStandardsListItems;
            mockApiClient
                .Setup(client => client.Get<GetFrameworksListResponse>(It.IsAny<GetFrameworksRequest>()))
                .ReturnsAsync(new GetFrameworksListResponse());
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetActiveStandardsListRequest>()))
                .ReturnsAsync(apiResponse);
            query.IncludeFoundationApprenticeships = true;

            var result = await handler.Handle(query, CancellationToken.None);

            result.TrainingProgrammes.Where(c=>c.Id != "1392").Should().BeEquivalentTo(apiResponse.Standards.Select(item => (TrainingProgramme)item));
        }
    }
}
