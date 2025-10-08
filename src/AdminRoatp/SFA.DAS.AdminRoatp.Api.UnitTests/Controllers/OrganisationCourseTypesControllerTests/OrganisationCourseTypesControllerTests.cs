using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Commands.UpdateOrganisationCourseTypes;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers.OrganisationCourseTypesControllerTests;
public class OrganisationCourseTypesControllerTests
{
    [Test, MoqAutoData]

    public async Task UpdateCourseTypes_ValidRequest_CallsMediatorAndReturnsNoContent(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] OrganisationCourseTypesController sut,
        UpdateOrganisationCourseTypesCommand command,
        UpdateCourseTypesModel model)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<UpdateOrganisationCourseTypesCommand>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var result = await sut.UpdateCourseTypes(command.ukprn, model, It.IsAny<CancellationToken>());

        result.Should().BeOfType<NoContentResult>();
        mediatorMock.Verify(m => m.Send(It.Is<UpdateOrganisationCourseTypesCommand>(r => r.ukprn == command.ukprn && r.CourseTypeIds == model.CourseTypeIds && r.UserId == model.UserId), It.IsAny<CancellationToken>()), Times.Once());

    }
}