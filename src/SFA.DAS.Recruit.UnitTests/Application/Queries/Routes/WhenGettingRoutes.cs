using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Recruit.Application.Queries.Routes;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.Routes
{
    public class WhenGettingRoutes
    {
        [Test, MoqAutoData]
        public async Task Then_The_Service_Called_And_Data_Returned(
            GetRoutesQuery query,
            GetRoutesListResponse response,
            [Frozen] Mock<ICourseService> service,
            GetRoutesQueryHandler handler)
        {
            //Arrange
            service.Setup(x => x.GetRoutes())
                .ReturnsAsync(response);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.Routes.Should().BeEquivalentTo(response.Routes);
        }
    }
}