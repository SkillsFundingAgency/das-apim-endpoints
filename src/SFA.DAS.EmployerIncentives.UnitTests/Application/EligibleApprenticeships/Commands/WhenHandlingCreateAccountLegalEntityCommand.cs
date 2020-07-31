using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Commands.AddLegalEntity;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Commands
{
    public class WhenHandlingCreateAccountLegalEntityCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_With_The_Request(
            CreateAccountLegalEntityCommand command,
            AccountLegalEntity response,
            [Frozen] Mock<IEmployerIncentivesService> employerIncentivesService,
            CreateAccountLegalEntityCommandHandler handler)
        {
            employerIncentivesService.Setup(x => x.CreateLegalEntity(command.AccountId,
                    It.Is<AccountLegalEntityCreateRequest>(c => 
                        c.LegalEntityId.Equals(command.LegalEntityId)
                        && c.OrganisationName.Equals(command.OrganisationName)
                        && c.AccountLegalEntityId.Equals(command.AccountLegalEntityId))))
                .ReturnsAsync(response);

            var actual = await handler.Handle(command, CancellationToken.None);
            
            actual.AccountLegalEntity.Should().BeEquivalentTo(response);
        }
    }
}