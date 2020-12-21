using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.Accounts;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    [TestFixture]
    public class WhenCallingGetLegalEntityByHashedId
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_Returning_The_Vendor_Id(
            string hashedLegalEntityId,
            AccountLegalEntity response,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            EmployerIncentivesService service
        )
        {
            client.Setup(x =>
                    x.Get<AccountLegalEntity>(
                        It.Is<GetLegalEntityByHashedIdRequest>(c => c.HashedLegalEntityId == hashedLegalEntityId)))
                .ReturnsAsync(response);

            var actual = await service.GetLegalEntityByHashedId(hashedLegalEntityId);

            actual.Should().BeEquivalentTo(response);
        }
    }
}
