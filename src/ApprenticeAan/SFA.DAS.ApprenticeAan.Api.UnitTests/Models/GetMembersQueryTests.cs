using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ApprenticeAan.Api.Models;
using SFA.DAS.ApprenticeAan.Application.Members.Queries.GetMembers;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Models;
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
    public void Operator_PopulatesQueryFromModelWithNulls()
    {
        var model = new GetMembersRequestModel();

        var query = (GetMembersQuery)model;

        query.Keyword.Should().BeNull();
        query.IsRegionalChair.Should().BeNull();
        query.Page.Should().BeNull();
        query.PageSize.Should().BeNull();
    }
}