using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCourses;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Assessors.UnitTests.Application.Queries
{
    public class WhenHandlingGetCourses
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Service_Called_To_Get_Standards_And_Frameworks(
            GetCoursesQuery query,
            GetStandardsListResponse standardsListResponse,
            GetFrameworksListResponse frameworksListResponse,
            [Frozen] Mock<ICourseService> courseService,
            GetCoursesQueryHandler handler)
        {
            //Arrange
            foreach (var standard in standardsListResponse.Standards)
            {
                standard.Level = (int) ApprenticeshipLevel.Advanced;
            }
            foreach (var standard in frameworksListResponse.Frameworks)
            {
                standard.Level = (int) ApprenticeshipLevel.Advanced;
            }
            courseService.Setup(x => x.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
                .ReturnsAsync(standardsListResponse);
            courseService.Setup(x => x.GetAllFrameworks<GetFrameworksListResponse>(nameof(GetFrameworksListResponse)))
                .ReturnsAsync(frameworksListResponse);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            var trainingProgrammes = standardsListResponse.Standards.Select(c=>(TrainingProgramme)c).ToList();
            trainingProgrammes.AddRange(frameworksListResponse.Frameworks.Select(c=>(TrainingProgramme)c).ToList());
            actual.TrainingProgrammes.Should().BeEquivalentTo(trainingProgrammes);
        }
    }
}