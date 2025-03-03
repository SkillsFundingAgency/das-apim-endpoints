﻿using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Functions;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Functions.GetPendingApplicationEmailData
{
    public class WhenCallingHandle
    {
        private readonly Fixture _fixture;

        public WhenCallingHandle()
        {
            _fixture = new Fixture();
            _fixture.Customize<GetApplicationsResponse.Application>(c => c.With(x => x.Status, "Pending"));
        }

        [Test, MoqAutoData]
        public async Task Then_Returns_Email_Data(
            GetPendingApplicationEmailDataQuery query,
            List<TeamMember> usersResponse,
            [Frozen] Mock<ILevyTransferMatchingService> levyTransferMatchingService,
            [Frozen] Mock<IAccountsService> accountsService,
            GetPendingApplicationEmailDataQueryHandler handler)
        {
            usersResponse.Add(new TeamMember
            {
                CanReceiveNotifications = true,
                Name = _fixture.Create<string>(),
                Role = "Owner",
                Email = _fixture.Create<string>()
            });

            var applicationsResponse = _fixture.Create<GetApplicationsResponse>();

            levyTransferMatchingService
                .Setup(x => x.GetApplications(It.IsAny<GetApplicationsRequest>()))
                .ReturnsAsync(applicationsResponse);

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
