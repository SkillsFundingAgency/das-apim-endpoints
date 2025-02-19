using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Api.Controllers;
using SFA.DAS.ToolsSupport.Api.Models.EmployerAccount;
using SFA.DAS.ToolsSupport.Application.Queries.GetEmployerAccountDetails;
using SFA.DAS.ToolsSupport.Models.Constants;

namespace SFA.DAS.ToolsSupport.Api.UnitTests.Controllers;

public class EmployerAccountControllerTests
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Account_Details_From_Mediator(
           long accountId,
           GetEmployerAccountDetailsResult getDetailsResult,
           [Frozen] Mock<IMediator> mockMediator,
           [Greedy] EmployerAccountController controller)
    {
        mockMediator
               .Setup(mediator => mediator.Send(
                   It.Is<GetEmployerAccountDetailsQuery>(x =>
                   x.AccountId == accountId
                   && x.SelectedField == AccountFieldSelection.EmployerAccount),
                   It.IsAny<CancellationToken>()));

        var controllerResult = await controller.GetAccountDetails(accountId, AccountFieldSelection.EmployerAccount) as ObjectResult;

        controllerResult.Should().NotBeNull();
        var model = controllerResult.Value as GetEmployerAccountDetailsResponse;

        model.Account.Should().NotBeNull();
    }
}
