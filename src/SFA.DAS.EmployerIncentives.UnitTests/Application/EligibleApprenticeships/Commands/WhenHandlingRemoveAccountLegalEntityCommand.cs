using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Commands.RemoveLegalEntity;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Commands
{
    public class WhenHandlingRemoveAccountLegalEntityCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_With_The_Request_To_Remove(
            RemoveAccountLegalEntityCommand command,
            [Frozen] Mock<ILegalEntitiesService> legalEntitiesService,
            RemoveAccountLegalEntityCommandHandler handler)
        {
            await handler.Handle(command, CancellationToken.None);
            
            legalEntitiesService.Verify(x => x.DeleteAccountLegalEntity(command.AccountId, command.AccountLegalEntityId), Times.Once);
        }
    }
}