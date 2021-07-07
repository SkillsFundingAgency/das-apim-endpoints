using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCourseDemand;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.UnitTests.Application.Demand.Queries
{
    public class WhenHandlingGetCourseDemandQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Data_Is_Returned_From_The_Api(
            GetCourseDemandQuery query,
            GetEmployerDemandResponse getDemandResponse,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> apiClient,
            GetCourseDemandQueryHandler handler)
        {
            //Arrange
            apiClient.Setup(x =>
                    x.Get<GetEmployerDemandResponse>(
                        It.Is<GetEmployerDemandRequest>(c => c.GetUrl.Contains($"demand/{query.Id}"))))
                .ReturnsAsync(getDemandResponse);

            //act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.EmployerDemand.Should().BeEquivalentTo(getDemandResponse);
        }
    }
}