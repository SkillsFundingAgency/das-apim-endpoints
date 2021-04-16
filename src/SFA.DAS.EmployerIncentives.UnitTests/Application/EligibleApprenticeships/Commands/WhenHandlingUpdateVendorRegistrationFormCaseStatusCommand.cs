using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Commands
{
    [TestFixture]
    public class WhenHandlingUpdateVendorRegistrationFormCaseStatusCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_legal_entity_is_updated_with_the_supplied_case_status(
            [Frozen] Mock<IEmployerIncentivesService> incentivesService,
            UpdateVendorRegistrationCaseStatusCommandHandler handler,
            UpdateVendorRegistrationCaseStatusCommand command)
        {
            var legalEntity = new Fixture().Create<AccountLegalEntity>();
            legalEntity.AccountId = command.AccountId;
            legalEntity.AccountLegalEntityId = command.AccountLegalEntityId;
            incentivesService.Setup(x => x.GetLegalEntity(command.AccountId, command.AccountLegalEntityId))
                .ReturnsAsync(legalEntity);

            await handler.Handle(command, CancellationToken.None);

            incentivesService.Verify(x => x.GetLegalEntity(command.AccountId, command.AccountLegalEntityId), Times.Once);
            incentivesService.Verify(x => x.UpdateVendorRegistrationCaseStatus(It.Is<UpdateVendorRegistrationCaseStatusRequest>
                (y => y.HashedLegalEntityId == legalEntity.HashedLegalEntityId && y.Status == command.VrfCaseStatus)), Times.Once);
        }

    }
}
