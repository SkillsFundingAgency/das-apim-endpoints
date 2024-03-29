﻿using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.EmployerAan.Application.Members.Queries.GetMembers;
using SFA.DAS.EmployerAan.Common;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.UnitTests.Application.Members.Queries.GetMembers;
public class GetMembersQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnMembers(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        GetMembersQueryHandler handler,
        GetMembersQueryResult expected,
        List<MemberUserType> userType,
        bool? isRegionalChair,
        List<int> regionIds,
        string keyword,
        int? page,
        int? pageSize,
        CancellationToken cancellationToken)
    {
        var query = new GetMembersQuery
        {
            Keyword = keyword,
            UserType = userType,
            IsRegionalChair = isRegionalChair,
            RegionIds = regionIds,
            Page = page,
            PageSize = pageSize
        };

        apiClient.Setup(x => x.GetMembers(It.IsAny<Dictionary<string, string[]>>(), cancellationToken)).ReturnsAsync(expected);
        var actual = await handler.Handle(query, cancellationToken);
        actual.Should().Be(expected);
    }
}
