using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Commands.SignAgreement;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Commands
{
    public class WhenHandlingSignAgreementCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_With_The_Request_Sign_The_Agreement(
            SignAgreementCommand command,
            [Frozen] Mock<IEmployerIncentivesService> employerIncentivesService,
            SignAgreementCommandHandler handler)
        {
            await handler.Handle(command, CancellationToken.None);
            
            employerIncentivesService.Verify(x => x.SignAgreement(command.AccountId, command.AccountLegalEntityId, It.Is<SignAgreementRequest>(y => y.AgreementVersion == command.AgreementVersion)), Times.Once);
        }
    }
}