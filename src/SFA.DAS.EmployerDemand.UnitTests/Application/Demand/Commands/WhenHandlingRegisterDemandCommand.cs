using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.RegisterDemand;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.UnitTests.Application.Demand.Commands
{
    public class WhenHandlingRegisterDemandCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called(
            RegisterDemandCommand command,
            PostCreateCourseDemand apiResponse,
            [Frozen]Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> apiClient,
            RegisterDemandCommandHandler handler)
        {
            //Arrange
            apiClient.Setup(x => x.Post<PostCreateCourseDemand>(It.Is<PostCreateCourseDemandRequest>(c=>
                    
                    ((CreateCourseDemandData)c.Data).Id.Equals(command.Id)
                    && ((CreateCourseDemandData)c.Data).ContactEmailAddress.Equals(command.ContactEmailAddress)
                    && ((CreateCourseDemandData)c.Data).OrganisationName.Equals(command.OrganisationName)
                    && ((CreateCourseDemandData)c.Data).NumberOfApprentices.Equals(command.NumberOfApprentices)
                    && ((CreateCourseDemandData)c.Data).Location.LocationPoint.GeoPoint.First() == command.Lat
                    && ((CreateCourseDemandData)c.Data).Location.LocationPoint.GeoPoint.Last() == command.Lon
                    && ((CreateCourseDemandData)c.Data).Location.Name.Equals(command.LocationName)
                    && ((CreateCourseDemandData)c.Data).Course.Title.Equals(command.CourseTitle)
                    && ((CreateCourseDemandData)c.Data).Course.Level.Equals(command.CourseLevel)
                    && ((CreateCourseDemandData)c.Data).Course.Id.Equals(command.CourseId)
                )))
                .ReturnsAsync(apiResponse);

            //Act
            var actual = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            actual.Should().Be(apiResponse.Id);
        }
    }
}