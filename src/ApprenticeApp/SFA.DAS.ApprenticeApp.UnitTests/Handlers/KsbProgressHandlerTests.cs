using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Commands;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class KsbProgressHandlerTests
    {
        [Test, MoqAutoData]
        public async Task RemoveTaskToKsbProgressCommandHandlerTest(
            RemoveTaskToKsbProgressCommandHandler sut,
            RemoveTaskToKsbProgressCommand query,
            CancellationToken cancellationToken)
        {
            query.TaskId = 1;
            query.ApprenticeshipId = 1;

            await sut.Handle(query, cancellationToken);
            sut.Should().NotBeNull();
        }


    }
}