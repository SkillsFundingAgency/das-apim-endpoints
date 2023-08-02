using MediatR;
using Moq;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Application.User.GetUserAccounts;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers;

public class EmployerAccountsControllerTests
{
    [Test]
    public async Task GetUserAccounts_InvokesMediator()
    {
        var userId = Guid.NewGuid().ToString();
        var email = Guid.NewGuid().ToString();
        CancellationToken token = new();
        Mock<IMediator> mediatorMock = new();
        EmployerAccountsController sut = new(mediatorMock.Object);

        await sut.GetUserAccounts(userId, email, token);

        mediatorMock.Verify(m => m.Send(It.Is<GetUserAccountsQuery>(q => q.UserId == userId && q.Email == email), token));
    }
}
