using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeSubscriptions;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class AddApprenticeSubscriptionCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task AddAppSubCommandHandlerTest(
            AddApprenticeSubscriptionCommandHandler sut,
            AddApprenticeSubscriptionCommand command,
            CancellationToken cancellationToken)
        {
            command.ApprenticeId = new Guid();

            await sut.Handle(command, cancellationToken);

            sut.Should().NotBeNull();
        }
    }
}