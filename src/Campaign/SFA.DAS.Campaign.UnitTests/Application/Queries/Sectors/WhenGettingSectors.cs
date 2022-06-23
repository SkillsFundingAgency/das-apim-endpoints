using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Campaign.Application.Queries.Sectors;
using SFA.DAS.Campaign.InnerApi.Requests;
using SFA.DAS.Campaign.InnerApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Application.Queries.Sectors
{
    public class WhenGettingSectors
    {
        [Test, MoqAutoData]
        public async Task Then_The_Service_Called_And_Data_Returned(
            GetSectorsQuery query,
            GetRoutesListResponse response,
            [Frozen] Mock<ICourseService> service,
            GetSectorsQueryHandler handler)
        {
            //Arrange
            service.Setup(x => x.GetRoutes())
                .ReturnsAsync(response);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.Sectors.Should().BeEquivalentTo(response.Routes);
        }
    }
}