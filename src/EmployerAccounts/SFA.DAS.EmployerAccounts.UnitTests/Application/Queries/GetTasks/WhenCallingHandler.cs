using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.GetTasks;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.GetTasks
{
    [TestFixture]
    public class WhenCallingHandler
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Tasks_Returns_GetTasksQueryResult(
            GetTasksQuery query,
            GetTasksQueryHandler handler)
        {
            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().BeEquivalentTo(new GetTasksQueryResult());
        }
    }
}