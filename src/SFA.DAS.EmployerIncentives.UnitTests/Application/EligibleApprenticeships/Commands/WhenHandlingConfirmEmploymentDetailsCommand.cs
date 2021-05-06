using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Commands.ConfirmApplication;
using SFA.DAS.EmployerIncentives.Application.Commands.ConfirmEmploymentDetails;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.EmploymentDetails;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Commands
{
    [TestFixture]
    public class WhenHandlingConfirmEmploymentDetailsCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_With_The_Request_To_Confirm_Employment_Details(
            ConfirmEmploymentDetailsRequest request,
            [Frozen] Mock<ICommitmentsService> commitmentsService,
            [Frozen] Mock<IEmployerIncentivesService> employerIncentivesService,
            ConfirmEmploymentDetailsCommandHandler handler)
        {
            var command = new ConfirmEmploymentDetailsCommand(request);

            await handler.Handle(command, CancellationToken.None);

            commitmentsService.Verify(x => x.GetApprenticeshipDetails(
                    It.Is<long>(r => r == command.ConfirmEmploymentDetailsRequest.AccountId),
                    It.IsAny<IEnumerable<long>>()), Times.Once);

            employerIncentivesService.Verify(x => x.UpdateIncentiveApplication(
                    It.Is<UpdateIncentiveApplicationRequestData>(
                        r =>
                            r.AccountId == command.ConfirmEmploymentDetailsRequest.AccountId &&
                            r.IncentiveApplicationId == command.ConfirmEmploymentDetailsRequest.ApplicationId)),
                        Times.Once);
        }
    }
}
