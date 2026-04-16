using System.Net;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolsSupport.Api.Controllers;
using SFA.DAS.ToolsSupport.Api.Models.EmployerAccount;
using SFA.DAS.ToolsSupport.Api.sources.EmployerAccount;
using SFA.DAS.ToolsSupport.Application.Commands.ChangeUserRole;
using SFA.DAS.ToolsSupport.Application.Commands.SupportCreateInvitation;
using SFA.DAS.ToolsSupport.Application.Commands.SupportResendInvitation;
using SFA.DAS.ToolsSupport.Application.Queries.GetAccountFinance;
using SFA.DAS.ToolsSupport.Application.Queries.GetAccountOrganisations;
using SFA.DAS.ToolsSupport.Application.Queries.GetEmployerAccountDetails;
using SFA.DAS.ToolsSupport.Application.Queries.GetPayeSchemeLevyDeclarations;
using SFA.DAS.ToolsSupport.Application.Queries.GetTeamMembers;
using SFA.DAS.ToolsSupport.Application.Queries.SearchEmployerAccounts;

namespace SFA.DAS.ToolsSupport.Api.UnitTests.Controllers;

public class EmployerAccountsControllerTests
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Account_Details_From_Mediator(
           long accountId,
           [Frozen] Mock<IMediator> mockMediator,
           [Greedy] EmployerAccountsController controller)
    {
        var getDetailsResult = new GetEmployerAccountDetailsResult();

        mockMediator
               .Setup(mediator => mediator.Send(
                   It.Is<GetEmployerAccountDetailsQuery>(x =>
                   x.AccountId == accountId),
                   It.IsAny<CancellationToken>())).ReturnsAsync(getDetailsResult);

        var controllerResult = await controller.GetAccountDetails(accountId) as ObjectResult;

        controllerResult.Should().NotBeNull();
        var model = controllerResult.Value as GetEmployerAccountDetailsResponse;

        model.Account.Should().NotBeNull();
        model.Account.Should().BeEquivalentTo(getDetailsResult);
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_Organisations_From_Mediator(
       long accountId,
       GetAccountOrganisationsQueryResult getOrganisationsResult,
       [Frozen] Mock<IMediator> mockMediator,
       [Greedy] EmployerAccountsController controller)
    {
        mockMediator
               .Setup(mediator => mediator.Send(
                   It.Is<GetAccountOrganisationsQuery>(x =>
                   x.AccountId == accountId),
                   It.IsAny<CancellationToken>())).ReturnsAsync(getOrganisationsResult);

        var controllerResult = await controller.GetAccountOrganisations(accountId) as ObjectResult;

        controllerResult.Should().NotBeNull();
        var model = controllerResult.Value as GetAccountOrganisationsResponse;

        model.LegalEntities.Should().NotBeNull();
        model.LegalEntities.Should().BeEquivalentTo(getOrganisationsResult.LegalEntities);
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_TeamMembers_From_Mediator(
       long accountId,
       GetTeamMembersQueryResult getTeamMembersResult,
       [Frozen] Mock<IMediator> mockMediator,
       [Greedy] EmployerAccountsController controller)
    {
        mockMediator
               .Setup(mediator => mediator.Send(
                   It.Is<GetTeamMembersQuery>(x =>
                   x.AccountId == accountId),
                   It.IsAny<CancellationToken>())).ReturnsAsync(getTeamMembersResult);

        var controllerResult = await controller.GetTeamMembers(accountId) as ObjectResult;

        controllerResult.Should().NotBeNull();
        var model = controllerResult.Value as GetTeamMembersResponse;

        model.TeamMembers.Should().NotBeNull();
        model.TeamMembers.Should().BeEquivalentTo(getTeamMembersResult.TeamMembers);
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_FinanceData_From_Mediator(
       long accountId,
       [Frozen] Mock<IMediator> mockMediator,
       [Greedy] EmployerAccountsController controller)
    {
        var getAccountFinanceResult = new GetAccountFinanceQueryResult();
        mockMediator
               .Setup(mediator => mediator.Send(
                   It.Is<GetAccountFinanceQuery>(x =>
                   x.AccountId == accountId),
                   It.IsAny<CancellationToken>())).ReturnsAsync(getAccountFinanceResult);

        var controllerResult = await controller.GetAccountFinance(accountId) as ObjectResult;

        controllerResult.Should().NotBeNull();
        var model = controllerResult.Value as GetAccountFinanceResponse;

        model.PayeSchemes.Should().NotBeNull();
        model.PayeSchemes.Should().BeEquivalentTo(getAccountFinanceResult.PayeSchemes);
        model.Transactions.Should().NotBeNull();
        model.Transactions.Should().BeEquivalentTo(getAccountFinanceResult.Transactions);
        model.Balance.Should().Be(getAccountFinanceResult.Balance);
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_PayeLevyDeclarations_From_Mediator(
       long accountId,
       string payeRef,
       [Frozen] Mock<IMediator> mockMediator,
       [Greedy] EmployerAccountsController controller)
    {
        var getPayeLevyDeclarationsResult = new GetPayeSchemeLevyDeclarationsResult();
        mockMediator
               .Setup(mediator => mediator.Send(
                   It.Is<GetPayeSchemeLevyDeclarationsQuery>(x =>
                   x.AccountId == accountId &&
                   x.PayeRef == payeRef),
                   It.IsAny<CancellationToken>())).ReturnsAsync(getPayeLevyDeclarationsResult);

        var controllerResult = await controller.GetPayeLevyDeclarations(accountId, payeRef) as ObjectResult;

        controllerResult.Should().NotBeNull();
        var model = controllerResult.Value as GetPayeLevyDeclarationsResponse;

        model.LevyDeclarations.Should().NotBeNull();
        model.LevyDeclarations.Count.Should().Be(getPayeLevyDeclarationsResult.LevySubmissions.Declarations.Count);

        model.UnexpectedError.Should().Be(getPayeLevyDeclarationsResult.StatusCode == PayeLevySubmissionsResponseCodes.UnexpectedError);
        model.PayeSchemeName.Should().BeEquivalentTo(getPayeLevyDeclarationsResult.PayeScheme.Name);
        model.PayeSchemeRef.Should().BeEquivalentTo(getPayeLevyDeclarationsResult.PayeScheme.ObscuredPayeRef);
        model.PayeSchemeFormatedAddedDate.Should().BeEquivalentTo(string.Empty);
    }

    [Test, MoqAutoData]
    public async Task SendInvitation_ShouldReturnOk_WhenRequestIsValid(
        SupportCreateInvitationRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerAccountsController controller
        )
    {
        // Arrange
        mockMediator.Setup(m => m.Send(It.Is<SupportCreateInvitationCommand>(cmd =>
            cmd.HashedAccountId == request.HashedAccountId &&
            cmd.FullName == request.FullName &&
            cmd.Email == request.Email &&
            cmd.Role == request.Role), It.IsAny<CancellationToken>()))
            .ReturnsAsync(HttpStatusCode.OK)
            .Verifiable();

        // Act
        var response = await controller.SendInvitation(request) as OkResult;

        // Assert
        mockMediator.Verify();
        response.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task SendInvitation_ShouldReturnBadRequest_WhenResponseStatusIsNotOk(
        SupportCreateInvitationRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerAccountsController controller
        )
    {
        // Arrange
        mockMediator.Setup(m => m.Send(It.IsAny<SupportCreateInvitationCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(HttpStatusCode.BadRequest)
            .Verifiable();

        // Act
        var response = await controller.SendInvitation(request) as BadRequestResult;

        // Assert
        mockMediator.Verify();
        response.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task SendInvitation_ShouldReturnBadRequest_WhenExceptionIsThrown(
        SupportCreateInvitationRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerAccountsController controller
        )
    {
        // Arrange
        mockMediator.Setup(m => m.Send(It.IsAny<SupportCreateInvitationCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Some error"))
            .Verifiable();

        // Act
        var response = await controller.SendInvitation(request) as BadRequestResult;

        // Assert
        mockMediator.Verify();
        response.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task ResendInvitation_ShouldReturnOk_WhenRequestIsValid(
        SupportResendInvitationRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerAccountsController controller
        )
    {
        // Arrange
        mockMediator.Setup(m => m.Send(It.Is<SupportResendInvitationCommand>(cmd =>
            cmd.HashedAccountId == request.HashedAccountId &&
            cmd.Email == request.Email), It.IsAny<CancellationToken>()))
            .ReturnsAsync(HttpStatusCode.OK)
            .Verifiable();

        // Act
        var response = await controller.ResendInvitation(request) as OkResult;

        // Assert
        mockMediator.Verify();
        response.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task ResendInvitation_ShouldReturnBadRequest_WhenResponseStatusIsNotOk(
        SupportResendInvitationRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerAccountsController controller
        )
    {
        // Arrange
        mockMediator.Setup(m => m.Send(It.IsAny<SupportResendInvitationCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(HttpStatusCode.BadRequest)
            .Verifiable();

        // Act
        var response = await controller.ResendInvitation(request) as BadRequestResult;

        // Assert
        mockMediator.Verify();
        response.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task ResendInvitation_ShouldReturnBadRequest_WhenExceptionIsThrown(
        SupportResendInvitationRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerAccountsController controller
        )
    {
        // Arrange
        mockMediator.Setup(m => m.Send(It.IsAny<SupportResendInvitationCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Some error"))
            .Verifiable();

        // Act
        var response = await controller.ResendInvitation(request) as BadRequestResult;

        // Assert
        mockMediator.Verify();
        response.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task ChangeUserRole_ShouldReturnOk_WhenRequestIsValid(
    ChangeUserRoleRequest request,
    [Frozen] Mock<IMediator> mockMediator,
    [Greedy] EmployerAccountsController controller
        )
    {
        // Arrange
        mockMediator.Setup(m => m.Send(It.Is<ChangeUserRoleCommand>(cmd =>
            cmd.HashedAccountId == request.HashedAccountId &&
            cmd.Email == request.Email), It.IsAny<CancellationToken>()))
            .ReturnsAsync(HttpStatusCode.OK)
            .Verifiable();

        // Act
        var response = await controller.ChangeUserRole(request) as OkResult;

        // Assert
        mockMediator.Verify();
        response.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task ChangeUserRole_ShouldReturnBadRequest_WhenResponseStatusIsNotOk(
        ChangeUserRoleRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerAccountsController controller
        )
    {
        // Arrange
        mockMediator.Setup(m => m.Send(It.IsAny<ChangeUserRoleCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(HttpStatusCode.BadRequest)
            .Verifiable();

        // Act
        var response = await controller.ChangeUserRole(request) as BadRequestResult;

        // Assert
        mockMediator.Verify();
        response.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task ChangeUserRole_ShouldReturnBadRequest_WhenExceptionIsThrown(
        ChangeUserRoleRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerAccountsController controller
        )
    {
        // Arrange
        mockMediator.Setup(m => m.Send(It.IsAny<ChangeUserRoleCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Some error"))
            .Verifiable();

        // Act
        var response = await controller.ChangeUserRole(request) as BadRequestResult;

        // Assert
        mockMediator.Verify();
        response.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_EmployerAccounts_By_EmployerName_From_Mediator(
        string employerName,
        SearchEmployerAccountsQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerAccountsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<SearchEmployerAccountsQuery>(x => x.EmployerName == employerName),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.Get(null, null, employerName) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult.Value.Should().BeEquivalentTo(mediatorResult);
    }
}
