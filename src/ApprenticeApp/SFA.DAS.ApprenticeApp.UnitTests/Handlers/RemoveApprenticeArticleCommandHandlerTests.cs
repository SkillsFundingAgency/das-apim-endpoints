using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeSubscriptions;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class RemoveApprenticeArticleCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task AddAppSubCommandHandlerTest(
            RemoveApprenticeArticleCommandHandler sut,
            RemoveApprenticeArticleCommand command,
            CancellationToken cancellationToken)
        {
            command.EntryId = "123";
            await sut.Handle(command, cancellationToken);
            sut.Should().NotBeNull();
        }
    }
}