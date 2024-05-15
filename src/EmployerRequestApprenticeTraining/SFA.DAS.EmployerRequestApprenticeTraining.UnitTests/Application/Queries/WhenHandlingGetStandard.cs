using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetStandard;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Queries
{
    public class WhenHandlingGetStandard
    {
        [Test, MoqAutoData]
        public async Task Then_Get_Standard_From_The_Api(
           StandardDetailResponse standardDetailResponse,
           GetStandardQuery query,
           [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
           GetStandardQueryHandler handler)
        {
            // Arrange
            mockCoursesApiClient
                .Setup(client => client.Get<StandardDetailResponse>(It.IsAny<GetStandardDetailsByIdRequest>()))
                .ReturnsAsync(standardDetailResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Standard.Should().BeEquivalentTo((Standard)standardDetailResponse, 
                options => options.ExcludingMissingMembers());
        }
    }
}
