using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingTheGetPledgesRequest
{
    [Test, AutoData]
    public void And_No_AccountId_Supplied_Then_The_GetUrl_Is_Correctly_Built()
    {
        var actual = new GetPledgesRequest();

        "pledges?page=1".Should().Be(actual.GetUrl);
    }

    [Test, AutoData]
    public void And_The_AccountId_Supplied_Then_The_GetUrl_Is_Correctly_Built(long accountId)
    {
        var actual = new GetPledgesRequest(accountId: accountId);

        $"pledges?accountId={accountId}&page=1".Should().Be(actual.GetUrl);
    }

    [Test, AutoData]
    public void And_The_Page_Supplied_Then_The_GetUrl_Is_Correctly_Built(int page)
    {
        var actual = new GetPledgesRequest(page: page);

        $"pledges?page={page}".Should().Be(actual.GetUrl);
    }

    [Test, AutoData]
    public void And_The_PageSize_Supplied_Then_The_GetUrl_Is_Correctly_Built(int pageSize)
    {
        var actual = new GetPledgesRequest(pageSize: pageSize);

        $"pledges?page=1&pageSize={pageSize}".Should().Be(actual.GetUrl);
    }

    [Test, AutoData]
    public void And_The_SortBy_Supplied_Then_The_GetUrl_Is_Correctly_Built(string sortBy)
    {
        var actual = new GetPledgesRequest(sortBy: sortBy);

        $"pledges?sortBy={sortBy}&page=1".Should().Be(actual.GetUrl);
    }
}