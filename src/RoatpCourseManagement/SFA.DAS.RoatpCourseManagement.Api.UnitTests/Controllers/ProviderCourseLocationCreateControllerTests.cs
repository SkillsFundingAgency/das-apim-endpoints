using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.AddNationalLocation;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers
{
    [TestFixture]
    public class ProviderCourseLocationCreateControllerTests
    {
        [Test, AutoData]
        public async Task AddNationalLocationToProviderCourseLocations_CallsHandler(int ukprn, int larsCode, AddNationalLocationToProviderCourseLocationsCommand command)
        {
            var mediatorMock = new Mock<IMediator>();
            var sut = new ProviderCourseLocationCreateController(Mock.Of<ILogger<ProviderCourseLocationCreateController>>(), mediatorMock.Object);

            var response = await sut.AddNationalLocationToProviderCourseLocations(ukprn, larsCode, command);

            mediatorMock.Verify(m => m.Send(command, new CancellationToken()));

            var result = (NoContentResult)response;

            result.Should().NotBeNull();
        }
    }
}
