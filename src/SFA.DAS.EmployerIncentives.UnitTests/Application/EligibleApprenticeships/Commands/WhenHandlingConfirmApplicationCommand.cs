using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Commands.ConfirmApplication;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Commands
{
    public class WhenHandlingConfirmApplicationCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_With_The_Request_To_Confirm(
            long accountId,
            Guid applicationId,
            DateTime submittedOn,
            string submittedBy,
            [Frozen] Mock<IEmployerIncentivesService> employerIncentivesService,
            ConfirmApplicationCommandHandler handler)
        {

            var command = new ConfirmApplicationCommand(applicationId, accountId, submittedOn, submittedBy);

            await handler.Handle(command, CancellationToken.None);

            employerIncentivesService.Verify(x => x.ConfirmIncentiveApplication(
                    It.Is<ConfirmIncentiveApplicationRequest>(
                        r =>
                            r.Data.DateSubmitted == command.DateSubmitted &&
                            r.Data.SubmittedBy == command.SubmittedBy &&
                            r.Data.AccountId == command.AccountId &&
                            r.Data.IncentiveApplicationId == command.ApplicationId),
                    CancellationToken.None),
                Times.Once);
        }
    }
}