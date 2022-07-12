using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Commands.RecalculateEarnings;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.RecalculateEarnings;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Commands
{
    [TestFixture]
    public class WhenHandlingRecalculateEarningsCommand
    {
        [Test, MoqAutoData]
        public async Task Then_the_recalculate_earnings_request_is_sent_via_the_service(
            [Frozen] Mock<IEmployerIncentivesService> employerIncentivesService,
            [Frozen] RecalculateEarningsCommandHandler handler,
            RecalculateEarningsCommand command)
        {
            employerIncentivesService.Setup(x => x.RecalculateEarnings(command.RecalculateEarningsRequest))
                .Returns(Task.CompletedTask);

            await handler.Handle(command, CancellationToken.None);

            employerIncentivesService.Verify(x => x.RecalculateEarnings(It.Is<RecalculateEarningsRequest>(
                r => r.IncentiveLearnerIdentifiers.Equals(command.RecalculateEarningsRequest.IncentiveLearnerIdentifiers))), Times.Once);
        }
    }
}
