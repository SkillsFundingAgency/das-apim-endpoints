using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Queries.GetLegalEntities;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Accounts;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Commands
{
    [TestFixture]
    public class WhenHandlingRefreshLegalEntitiesCommand
    {
        private Fixture _fixture;
        [SetUp]
        public void Arrange()
        {
            _fixture = new Fixture();
        }

        [Test, MoqAutoData]
        public async Task Then_The_legal_entities_are_refreshed(
            [Frozen] Mock<ILegalEntitiesService> legalEntitiesService,
            [Frozen] Mock<IAccountsService> accountsService,
            RefreshLegalEntitiesCommand command,
            RefreshLegalEntitiesCommandHandler handler)
        {
            var legalEntities = _fixture.CreateMany<AccountLegalEntity>(100).ToList();
            var pagedResponse = new PagedResponse<AccountLegalEntity> { Data = legalEntities, Page = command.PageNumber, TotalPages = command.PageNumber };
            accountsService.Setup(x => x.GetLegalEntitiesByPage(command.PageNumber, command.PageSize)).ReturnsAsync(pagedResponse);

            await handler.Handle(command, CancellationToken.None);

            accountsService.Verify(x => x.GetLegalEntitiesByPage(command.PageNumber, command.PageSize), Times.Once);
            legalEntitiesService.Verify(x => x.RefreshLegalEntities(pagedResponse.Data, pagedResponse.Page, command.PageSize, pagedResponse.TotalPages), Times.Once);
        }

    }
}
