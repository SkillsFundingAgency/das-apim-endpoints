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
            string submittedByEmail,
            string submittedByName,
            [Frozen] Mock<IApplicationService> applicationService,
            ConfirmApplicationCommandHandler handler)
        {

            var command = new ConfirmApplicationCommand(applicationId, accountId, submittedOn, submittedByEmail, submittedByName);

            await handler.Handle(command, CancellationToken.None);

            applicationService.Verify(x => x.Confirm(
                    It.Is<ConfirmIncentiveApplicationRequest>(
                        r =>
                            r.Data.DateSubmitted == command.DateSubmitted &&
                            r.Data.SubmittedByEmail == command.SubmittedByEmail &&
                            r.Data.AccountId == command.AccountId &&
                            r.Data.IncentiveApplicationId == command.ApplicationId &&
                            r.Data.SubmittedByName == command.SubmittedByName),
                    CancellationToken.None),
                Times.Once);
        }
    }
}