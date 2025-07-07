using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Api.Models.Apprentices;
using SFA.DAS.Approvals.Application.Apprentices.Commands.EditApprenticeship;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Apprentices;

[TestFixture]
public class WhenPostingEditApprenticeship
{
    [Test, MoqAutoData]
    public async Task EditApprenticeshipResponseIsReturned(
        [Frozen] Mock<IMediator> mediator,
        EditApprenticeshipResult commandResult,
        EditApprenticeshipRequest request,
        long providerId,
        long apprenticeshipId,
        [NoAutoProperties]ApprenticesController controller)
    {
        // Arrange
        mediator.Setup(x => x.Send(It.Is<EditApprenticeshipCommand>(c =>
                    c.ApprenticeshipId == apprenticeshipId &&
                    c.ProviderId == providerId &&
                    c.EmployerAccountId == request.EmployerAccountId &&
                    c.FirstName == request.FirstName &&
                    c.LastName == request.LastName &&
                    c.Email == request.Email &&
                    c.DateOfBirth == request.DateOfBirth &&
                    c.ULN == request.ULN &&
                    c.CourseCode == request.CourseCode &&
                    c.Version == request.Version &&
                    c.Option == request.Option &&
                    c.Cost == request.Cost &&
                    c.EmployerReference == request.EmployerReference &&
                    c.StartDate == request.StartDate &&
                    c.EndDate == request.EndDate &&
                    c.DeliveryModel == request.DeliveryModel &&
                    c.ProviderReference == request.ProviderReference &&
                    c.EmploymentEndDate == request.EmploymentEndDate &&
                    c.EmploymentPrice == request.EmploymentPrice),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(commandResult);

        // Act
        var result = await controller.EditApprenticeship(providerId, null, apprenticeshipId, request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okObjectResult = (OkObjectResult)result;
        okObjectResult.Value.Should().BeOfType<EditApprenticeshipResponse>();
        var objectResult = (EditApprenticeshipResponse)okObjectResult.Value;

        objectResult.ApprenticeshipId.Should().Be(commandResult.ApprenticeshipId);
        objectResult.HasOptions.Should().Be(commandResult.HasOptions);
    }

    [Test, MoqAutoData]
    public async Task EditApprenticeshipCommandIsSentWithCorrectParameters(
        [Frozen] Mock<IMediator> mediator,
        EditApprenticeshipResult commandResult,
        EditApprenticeshipRequest request,
        long providerId,
        long apprenticeshipId,
        [NoAutoProperties]ApprenticesController controller)
    {
        // Arrange
        mediator.Setup(x => x.Send(It.Is<EditApprenticeshipCommand>(c =>
                    c.ApprenticeshipId == apprenticeshipId &&
                    c.ProviderId == providerId &&
                    c.EmployerAccountId == request.EmployerAccountId &&
                    c.FirstName == request.FirstName &&
                    c.LastName == request.LastName &&
                    c.Email == request.Email &&
                    c.DateOfBirth == request.DateOfBirth &&
                    c.ULN == request.ULN &&
                    c.CourseCode == request.CourseCode &&
                    c.Version == request.Version &&
                    c.Option == request.Option &&
                    c.Cost == request.Cost &&
                    c.EmployerReference == request.EmployerReference &&
                    c.StartDate == request.StartDate &&
                    c.EndDate == request.EndDate &&
                    c.DeliveryModel == request.DeliveryModel &&
                    c.ProviderReference == request.ProviderReference &&
                    c.EmploymentEndDate == request.EmploymentEndDate &&
                    c.EmploymentPrice == request.EmploymentPrice),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(commandResult);

        // Act
        await controller.EditApprenticeship(providerId, null, apprenticeshipId, request);

        // Assert
        mediator.Verify(x => x.Send(It.Is<EditApprenticeshipCommand>(c =>
            c.ApprenticeshipId == apprenticeshipId &&
            c.ProviderId == providerId &&
            c.EmployerAccountId == request.EmployerAccountId &&
            c.FirstName == request.FirstName &&
            c.LastName == request.LastName &&
            c.Email == request.Email &&
            c.DateOfBirth == request.DateOfBirth &&
            c.ULN == request.ULN &&
            c.CourseCode == request.CourseCode &&
            c.Version == request.Version &&
            c.Option == request.Option &&
            c.Cost == request.Cost &&
            c.EmployerReference == request.EmployerReference &&
            c.StartDate == request.StartDate &&
            c.EndDate == request.EndDate &&
            c.DeliveryModel == request.DeliveryModel &&
            c.ProviderReference == request.ProviderReference &&
            c.EmploymentEndDate == request.EmploymentEndDate &&
            c.EmploymentPrice == request.EmploymentPrice), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task EditApprenticeshipResponseIsReturned_EmployerContext(
        [Frozen] Mock<IMediator> mediator,
        EditApprenticeshipResult commandResult,
        EditApprenticeshipRequest request,
        long accountId,
        long apprenticeshipId,
        [NoAutoProperties]ApprenticesController controller)
    {
        // Arrange
        request.EmployerAccountId = null; // Clear this so it uses accountId from route
        mediator.Setup(x => x.Send(It.Is<EditApprenticeshipCommand>(c =>
                    c.ApprenticeshipId == apprenticeshipId &&
                    c.ProviderId == null &&
                    c.EmployerAccountId == accountId &&
                    c.FirstName == request.FirstName &&
                    c.LastName == request.LastName &&
                    c.Email == request.Email &&
                    c.DateOfBirth == request.DateOfBirth &&
                    c.ULN == request.ULN &&
                    c.CourseCode == request.CourseCode &&
                    c.Version == request.Version &&
                    c.Option == request.Option &&
                    c.Cost == request.Cost &&
                    c.EmployerReference == request.EmployerReference &&
                    c.StartDate == request.StartDate &&
                    c.EndDate == request.EndDate &&
                    c.DeliveryModel == request.DeliveryModel &&
                    c.ProviderReference == request.ProviderReference &&
                    c.EmploymentEndDate == request.EmploymentEndDate &&
                    c.EmploymentPrice == request.EmploymentPrice),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(commandResult);

        // Act
        var result = await controller.EditApprenticeship(null, accountId, apprenticeshipId, request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okObjectResult = (OkObjectResult)result;
        okObjectResult.Value.Should().BeOfType<EditApprenticeshipResponse>();
        var objectResult = (EditApprenticeshipResponse)okObjectResult.Value;

        objectResult.ApprenticeshipId.Should().Be(commandResult.ApprenticeshipId);
        objectResult.HasOptions.Should().Be(commandResult.HasOptions);
    }

    [Test, MoqAutoData]
    public async Task EditApprenticeshipCommandIsSentWithCorrectParameters_EmployerContext(
        [Frozen] Mock<IMediator> mediator,
        EditApprenticeshipResult commandResult,
        EditApprenticeshipRequest request,
        long accountId,
        long apprenticeshipId,
        [NoAutoProperties]ApprenticesController controller)
    {
        // Arrange
        request.EmployerAccountId = null; // Clear this so it uses accountId from route
        mediator.Setup(x => x.Send(It.Is<EditApprenticeshipCommand>(c =>
                    c.ApprenticeshipId == apprenticeshipId &&
                    c.ProviderId == null &&
                    c.EmployerAccountId == accountId &&
                    c.FirstName == request.FirstName &&
                    c.LastName == request.LastName &&
                    c.Email == request.Email &&
                    c.DateOfBirth == request.DateOfBirth &&
                    c.ULN == request.ULN &&
                    c.CourseCode == request.CourseCode &&
                    c.Version == request.Version &&
                    c.Option == request.Option &&
                    c.Cost == request.Cost &&
                    c.EmployerReference == request.EmployerReference &&
                    c.StartDate == request.StartDate &&
                    c.EndDate == request.EndDate &&
            c.DeliveryModel == request.DeliveryModel &&
            c.ProviderReference == request.ProviderReference &&
            c.EmploymentEndDate == request.EmploymentEndDate &&
            c.EmploymentPrice == request.EmploymentPrice),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(commandResult);

        // Act
        await controller.EditApprenticeship(null, accountId, apprenticeshipId, request);

        // Assert
        mediator.Verify(x => x.Send(It.Is<EditApprenticeshipCommand>(c =>
            c.ApprenticeshipId == apprenticeshipId &&
            c.ProviderId == null &&
            c.EmployerAccountId == accountId &&
            c.FirstName == request.FirstName &&
            c.LastName == request.LastName &&
            c.Email == request.Email &&
            c.DateOfBirth == request.DateOfBirth &&
            c.ULN == request.ULN &&
            c.CourseCode == request.CourseCode &&
            c.Version == request.Version &&
            c.Option == request.Option &&
            c.Cost == request.Cost &&
            c.EmployerReference == request.EmployerReference &&
            c.StartDate == request.StartDate &&
            c.EndDate == request.EndDate &&
            c.DeliveryModel == request.DeliveryModel &&
            c.ProviderReference == request.ProviderReference &&
            c.EmploymentEndDate == request.EmploymentEndDate &&
            c.EmploymentPrice == request.EmploymentPrice), It.IsAny<CancellationToken>()), Times.Once);
    }
}