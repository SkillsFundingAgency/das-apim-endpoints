using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Commands;
using SFA.DAS.ApprenticeApp.Application.Queries.Details;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class TasksHandlerTests
    {
        [Test, MoqAutoData]
        public async Task GetTaskByTaskIdQueryTest(
            GetTaskByTaskIdQueryHandler sut,
            GetTaskByTaskIdQuery query,
            CancellationToken cancellationToken)
        {
            query.TaskId = 1;
            query.ApprenticeshipId = 1;

            await sut.Handle(query, cancellationToken);
            sut.Should().NotBeNull();
        }

        [Test, MoqAutoData]
        public async Task GetTaskCategoriesQueryTest(
            GetTaskCategoriesQueryHandler sut,
            GetTaskCategoriesQuery query,
            CancellationToken cancellationToken)
        {
            query.ApprenticeshipId = 1;
            await sut.Handle(query, cancellationToken);
            sut.Should().NotBeNull();
        }

        [Test, MoqAutoData]
        public async Task GetTasksByApprenticeshipIdQueryTest(
            GetTasksByApprenticeshipIdQueryHandler sut,
            GetTasksByApprenticeshipIdQuery query,
            CancellationToken cancellationToken)
        {
            query.ApprenticeshipId = 1;
            query.FromDate = new System.DateTime();
            query.ToDate = new System.DateTime();
            query.Status = 1;

            await sut.Handle(query, cancellationToken);
            sut.Should().NotBeNull();
        }

        [Test, MoqAutoData]
        public async Task DeleteApprenticeTaskCommandTest(
          DeleteApprenticeTaskCommandHandler sut,
          DeleteApprenticeTaskCommand query,
          CancellationToken cancellationToken)
        {
            query.ApprenticeshipId = 1;
            query.TaskId = 1;

            await sut.Handle(query, cancellationToken);
            sut.Should().NotBeNull();
        }

    }
}