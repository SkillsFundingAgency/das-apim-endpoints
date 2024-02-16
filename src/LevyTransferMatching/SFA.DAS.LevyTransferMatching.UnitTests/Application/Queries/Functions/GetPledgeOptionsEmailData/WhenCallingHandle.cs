using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Functions;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Functions.GetPledgeOptionsEmailData
{
    public class WhenCallingHandle
    {
        private readonly Fixture _fixture;

        public WhenCallingHandle()
        {
            _fixture = new Fixture();
            _fixture.Customize<GetPledgesResponse.Pledge>(c => c.With(x => x.Status, "Active"));
        }

        [Test, MoqAutoData]
        public async Task Then_Returns_Email_Data(
            GetPledgeOptionsEmailDataQuery query,
            List<TeamMember> usersResponse,
            [Frozen] Mock<ILevyTransferMatchingService> levyTransferMatchingService,
            [Frozen] Mock<IAccountsService> accountsService,
            GetPledgeOptionsEmailDataQueryHandler handler)
        {
            usersResponse.Add(new TeamMember
            {
                CanReceiveNotifications = true,
                Name = _fixture.Create<string>(),
                Role = "Owner",
                Email = _fixture.Create<string>()
            });

            var pledgesResponse = _fixture.Create<GetPledgesResponse>();

            levyTransferMatchingService
                .Setup(x => x.GetPledges(It.IsAny<GetPledgesRequest>()))
                .ReturnsAsync(pledgesResponse);

            accountsService
                .Setup(x => x.GetAccountUsers(It.IsAny<long>()))
                .ReturnsAsync(usersResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.EmailDataList, Is.Not.Null);
            Assert.That(result.EmailDataList, Is.Not.Empty);
        }
    }
}
