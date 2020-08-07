using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Commands.UpdateApplication;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Commands
{
    public class WhenHandlingUpdateApplicationCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_CommitmentService_Is_Called_To_Get_Apprenticeship_Details(
            long accountId,
            long accountLegalEntityId,
            long[] apprenticeshipIds,
            Guid applicationId,
            [Frozen] Mock<ICommitmentsService> commitmentsService,
            UpdateApplicationCommandHandler handler)
        {

            var command = new UpdateApplicationCommand(applicationId, accountId, accountLegalEntityId, apprenticeshipIds);

            await handler.Handle(command, CancellationToken.None);

            commitmentsService.Verify(x => x.GetApprenticeshipDetails(command.AccountId, command.ApprenticeshipIds), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_The_EmployerIncentivesService_Is_Called_To_Update_The_Application(
            long accountId,
            long accountLegalEntityId,
            long[] apprenticeshipIds,
            Guid applicationId,
            ApprenticeshipResponse[] apprenticeshipDetails,
            [Frozen] Mock<IEmployerIncentivesService> employerIncentivesService,
            [Frozen] Mock<ICommitmentsService> commitmentsService,
            UpdateApplicationCommandHandler handler)
        {

            var command = new UpdateApplicationCommand(applicationId, accountId, accountLegalEntityId, apprenticeshipIds);
            commitmentsService.Setup(x => x.GetApprenticeshipDetails(It.IsAny<long>(), It.IsAny<long[]>())).ReturnsAsync(apprenticeshipDetails);

            await handler.Handle(command, CancellationToken.None);

            employerIncentivesService.Verify(
                x => x.UpdateIncentiveApplication(
                    It.Is<UpdateIncentiveApplicationRequestData>(p => p.IncentiveApplicationId == command.ApplicationId &&
                                                           p.AccountId == command.AccountId &&
                                                           p.Apprenticeships.Length == apprenticeshipDetails.Length)),
                Times.Once);
        }
    }
}