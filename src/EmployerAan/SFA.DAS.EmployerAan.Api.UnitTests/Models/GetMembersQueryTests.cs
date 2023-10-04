using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.EmployerAan.Application.Members.Queries.GetMembers;
using SFA.DAS.EmployerAan.Application.Models;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Models;
public class GetMembersRequestModelTests
{

    [Test, AutoData]
    public void Operator_PopulatesQueryFromModel(GetMembersRequestModel model)
    {
        var query = (GetMembersQuery)model;

        query.Keyword.Should().Be(model.Keyword);
        query.IsRegionalChair.Should().Be(model.IsRegionalChair);
        query.Page.Should().Be(model.Page);
        query.PageSize.Should().Be(model.PageSize);
    }

    [Test, AutoData]
    public void Operator_PopulatesQueryFromModelWithNulls(Guid memberId)
    {
        var model = new GetMembersRequestModel();

        var query = (GetMembersQuery)model;

        query.Keyword.Should().BeNull();
        query.IsRegionalChair.Should().BeNull();
        query.Page.Should().BeNull();
        query.PageSize.Should().BeNull();
    }
}
