using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Api.Models.Apprentices;
using SFA.DAS.Approvals.Application.Apprentices.Commands.ConfirmEditApprenticeship;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Apprentices;

[TestFixture]
public class WhenPostingConfirmEditApprenticeship
{
    [Test, MoqAutoData]
    public async Task ConfirmEditApprenticeshipResponseIsReturned_ProviderContext(
        [Frozen] Mock<IMediator> mediator,
        ConfirmEditApprenticeshipResult commandResult,
        ConfirmEditApprenticeshipRequest request,
        long providerId,
        long apprenticeshipId,
        [NoAutoProperties]ApprenticesController controller)
    {
        // Arrange
        mediator.Setup(x => x.Send(It.Is<ConfirmEditApprenticeshipCommand>(c =>
                    c.ApprenticeshipId == apprenticeshipId &&
                    c.ProviderId == providerId &&
                    c.AccountId == null &&
                    c.FirstName == request.FirstName &&
                    c.LastName == request.LastName &&
                    c.Email == request.Email &&
                    c.DateOfBirth == request.DateOfBirth &&
                    c.Cost == request.Cost &&
                    c.ProviderReference == request.ProviderReference &&
                    c.StartDate == request.StartDate &&
                    c.EndDate == request.EndDate &&
                    c.DeliveryModel == request.DeliveryModel &&
                    c.EmploymentEndDate == request.EmploymentEndDate &&
                    c.EmploymentPrice == request.EmploymentPrice &&
                    c.CourseCode == request.CourseCode &&
                    c.Version == request.Version &&
                    c.Option == request.Option),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(commandResult);

        // Act
        var result = await controller.ConfirmEditApprenticeshipProvider(providerId, apprenticeshipId, request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okObjectResult = (OkObjectResult)result;
        okObjectResult.Value.Should().BeOfType<ConfirmEditApprenticeshipResponse>();
        var objectResult = (ConfirmEditApprenticeshipResponse)okObjectResult.Value;

        objectResult.ApprenticeshipId.Should().Be(commandResult.ApprenticeshipId);
        objectResult.NeedReapproval.Should().Be(commandResult.NeedReapproval);
    }

    [Test, MoqAutoData]
    public async Task ConfirmEditApprenticeshipCommandIsSentWithCorrectParameters_ProviderContext(
        [Frozen] Mock<IMediator> mediator,
        ConfirmEditApprenticeshipResult commandResult,
        ConfirmEditApprenticeshipRequest request,
        long providerId,
        long apprenticeshipId,
        [NoAutoProperties]ApprenticesController controller)
    {
        // Arrange
        mediator.Setup(x => x.Send(It.Is<ConfirmEditApprenticeshipCommand>(c =>
                    c.ApprenticeshipId == apprenticeshipId &&
                    c.ProviderId == providerId &&
                    c.AccountId == null &&
                    c.FirstName == request.FirstName &&
                    c.LastName == request.LastName &&
                    c.Email == request.Email &&
                    c.DateOfBirth == request.DateOfBirth &&
                    c.Cost == request.Cost &&
                    c.ProviderReference == request.ProviderReference &&
                    c.StartDate == request.StartDate &&
                    c.EndDate == request.EndDate &&
                    c.DeliveryModel == request.DeliveryModel &&
                    c.EmploymentEndDate == request.EmploymentEndDate &&
                    c.EmploymentPrice == request.EmploymentPrice &&
                    c.CourseCode == request.CourseCode &&
                    c.Version == request.Version &&
                    c.Option == request.Option),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(commandResult);

        // Act
        await controller.ConfirmEditApprenticeshipProvider(providerId, apprenticeshipId, request);

        // Assert
        mediator.Verify(x => x.Send(It.Is<ConfirmEditApprenticeshipCommand>(c =>
            c.ApprenticeshipId == apprenticeshipId &&
            c.ProviderId == providerId &&
            c.AccountId == null &&
            c.FirstName == request.FirstName &&
            c.LastName == request.LastName &&
            c.Email == request.Email &&
            c.DateOfBirth == request.DateOfBirth &&
            c.Cost == request.Cost &&
            c.ProviderReference == request.ProviderReference &&
            c.StartDate == request.StartDate &&
            c.EndDate == request.EndDate &&
            c.DeliveryModel == request.DeliveryModel &&
            c.EmploymentEndDate == request.EmploymentEndDate &&
            c.EmploymentPrice == request.EmploymentPrice &&
            c.CourseCode == request.CourseCode &&
            c.Version == request.Version &&
            c.Option == request.Option), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task ConfirmEditApprenticeshipResponseIsReturned_EmployerContext(
        [Frozen] Mock<IMediator> mediator,
        ConfirmEditApprenticeshipResult commandResult,
        ConfirmEditApprenticeshipRequest request,
        long accountId,
        long apprenticeshipId,
        [NoAutoProperties]ApprenticesController controller)
    {
        // Arrange
        mediator.Setup(x => x.Send(It.Is<ConfirmEditApprenticeshipCommand>(c =>
                    c.ApprenticeshipId == apprenticeshipId &&
                    c.ProviderId == null &&
                    c.AccountId == accountId &&
                    c.FirstName == request.FirstName &&
                    c.LastName == request.LastName &&
                    c.Email == request.Email &&
                    c.DateOfBirth == request.DateOfBirth &&
                    c.Cost == request.Cost &&
                    c.ProviderReference == request.ProviderReference &&
                    c.StartDate == request.StartDate &&
                    c.EndDate == request.EndDate &&
                    c.DeliveryModel == request.DeliveryModel &&
                    c.EmploymentEndDate == request.EmploymentEndDate &&
                    c.EmploymentPrice == request.EmploymentPrice &&
                    c.CourseCode == request.CourseCode &&
                    c.Version == request.Version &&
                    c.Option == request.Option),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(commandResult);

        // Act
        var result = await controller.ConfirmEditApprenticeshipEmployer(accountId, apprenticeshipId, request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okObjectResult = (OkObjectResult)result;
        okObjectResult.Value.Should().BeOfType<ConfirmEditApprenticeshipResponse>();
        var objectResult = (ConfirmEditApprenticeshipResponse)okObjectResult.Value;

        objectResult.ApprenticeshipId.Should().Be(commandResult.ApprenticeshipId);
        objectResult.NeedReapproval.Should().Be(commandResult.NeedReapproval);
    }

    [Test, MoqAutoData]
    public async Task ConfirmEditApprenticeshipCommandIsSentWithCorrectParameters_EmployerContext(
        [Frozen] Mock<IMediator> mediator,
        ConfirmEditApprenticeshipResult commandResult,
        ConfirmEditApprenticeshipRequest request,
        long accountId,
        long apprenticeshipId,
        [NoAutoProperties]ApprenticesController controller)
    {
        // Arrange
        mediator.Setup(x => x.Send(It.Is<ConfirmEditApprenticeshipCommand>(c =>
                    c.ApprenticeshipId == apprenticeshipId &&
                    c.ProviderId == null &&
                    c.AccountId == accountId &&
                    c.FirstName == request.FirstName &&
                    c.LastName == request.LastName &&
                    c.Email == request.Email &&
                    c.DateOfBirth == request.DateOfBirth &&
                    c.Cost == request.Cost &&
                    c.ProviderReference == request.ProviderReference &&
                    c.StartDate == request.StartDate &&
                    c.EndDate == request.EndDate &&
                    c.DeliveryModel == request.DeliveryModel &&
                    c.EmploymentEndDate == request.EmploymentEndDate &&
                    c.EmploymentPrice == request.EmploymentPrice &&
                    c.CourseCode == request.CourseCode &&
                    c.Version == request.Version &&
                    c.Option == request.Option),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(commandResult);

        // Act
        await controller.ConfirmEditApprenticeshipEmployer(accountId, apprenticeshipId, request);

        // Assert
        mediator.Verify(x => x.Send(It.Is<ConfirmEditApprenticeshipCommand>(c =>
            c.ApprenticeshipId == apprenticeshipId &&
            c.ProviderId == null &&
            c.AccountId == accountId &&
            c.FirstName == request.FirstName &&
            c.LastName == request.LastName &&
            c.Email == request.Email &&
            c.DateOfBirth == request.DateOfBirth &&
            c.Cost == request.Cost &&
            c.ProviderReference == request.ProviderReference &&
            c.StartDate == request.StartDate &&
            c.EndDate == request.EndDate &&
            c.DeliveryModel == request.DeliveryModel &&
            c.EmploymentEndDate == request.EmploymentEndDate &&
            c.EmploymentPrice == request.EmploymentPrice &&
            c.CourseCode == request.CourseCode &&
            c.Version == request.Version &&
            c.Option == request.Option), It.IsAny<CancellationToken>()), Times.Once);
    }
} 