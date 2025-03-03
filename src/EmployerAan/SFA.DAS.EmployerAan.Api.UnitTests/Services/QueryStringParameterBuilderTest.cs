﻿using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.EmployerAan.Application.Members.Queries.GetMembers;
using SFA.DAS.EmployerAan.Application.Models;
using SFA.DAS.EmployerAan.Application.Services;
using SFA.DAS.EmployerAan.Common;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Models;
public class QueryStringParameterBuilderTests
{
    [Test, AutoData]
    public void Members_PopulatesModelFromParameters(string keyword, List<MemberUserType> userType, bool isRegionalChair, List<int> regionIds, int? page, int? pageSize)
    {
        var sut = new GetMembersRequestModel
        {
            Keyword = keyword,
            UserType = userType,
            IsRegionalChair = isRegionalChair,
            RegionId = regionIds,
            Page = page,
            PageSize = pageSize
        };

        var query = (GetMembersQuery)sut;

        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(query);

        parameters.TryGetValue("keyword", out string[]? keywordResult);
        keywordResult![0].Should().Be(keyword);

        parameters.TryGetValue("userType", out var userTypeResult);
        userTypeResult!.Length.Should().Be(userType.Count);
        userType.Select(x => x.ToString()).Should().BeEquivalentTo(userTypeResult.ToList());

        parameters.TryGetValue("isRegionalChair", out string[]? isRegionalChairResult);
        isRegionalChairResult![0].Should().Be(isRegionalChair.ToString());

        parameters.TryGetValue("regionId", out var regionIdsResult);
        regionIdsResult!.Length.Should().Be(regionIds.Count);
        regionIds.Select(x => x.ToString()).Should().BeEquivalentTo(regionIdsResult.ToList());

        parameters.TryGetValue("page", out string[]? pageResult);
        pageResult![0].Should().Be(page?.ToString());

        parameters.TryGetValue("pageSize", out string[]? pageSizeResult);
        pageSizeResult![0].Should().Be(pageSize?.ToString());
    }

    [Test, TestCaseSource(nameof(_Data))]
    public void Members_ConstructParameters_UserType(List<MemberUserType> userType)
    {
        var model = new GetMembersRequestModel
        {
            UserType = userType
        };
        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(model);

        parameters.TryGetValue("userType", out var userTypeResult);
        if (userTypeResult != null)
        {
            userTypeResult![0].Should().Be(userType[0].ToString());
        }
        else
        {
            userTypeResult.Should().BeNull();
        }
    }

    private static readonly object?[] _Data =
    {
      null,
      new object[] {new List<MemberUserType> { MemberUserType.Apprentice} }
    };

    [TestCase(null)]
    [TestCase(true)]
    [TestCase(false)]
    public void Members_ConstructParameters_IsRegionalChair(bool? isRegionalChair)
    {
        var model = new GetMembersRequestModel
        {
            IsRegionalChair = isRegionalChair
        };
        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(model);

        parameters.TryGetValue("isRegionalChair", out var isRegionalChairResult);
        if (isRegionalChairResult != null)
        {
            isRegionalChairResult![0].Should().Be(isRegionalChair?.ToString());
        }
        else
        {
            isRegionalChairResult.Should().BeNull();
        }
    }

    [TestCase(null)]
    [TestCase(1)]
    public void Members_ConstructParameters_RegionIds(int? regionId)
    {
        var model = new GetMembersRequestModel();

        if (regionId != null)
        {
            model.RegionId = new List<int> { regionId.Value };
        }

        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(model);

        parameters.TryGetValue("regionId", out var regionIdResult);
        if (regionIdResult != null)
        {
            regionIdResult![0].Should().BeEquivalentTo(model.RegionId[0].ToString());
        }
        else
        {
            regionIdResult.Should().BeNull();
        }
    }

    [TestCase(null)]
    [TestCase(3)]
    public void Members_ConstructParameters_ToPage(int? page)
    {
        var model = new GetMembersRequestModel
        {
            Page = page
        };
        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(model);

        parameters.TryGetValue("page", out var pageResult);
        if (pageResult != null)
        {
            pageResult![0].Should().Be(page?.ToString());
        }
        else
        {
            pageResult.Should().BeNull();
        }
    }

    [TestCase(null)]
    [TestCase(6)]
    public void Members_ConstructParameters_ToPageSize(int? pageSize)
    {
        var model = new GetMembersRequestModel
        {
            PageSize = pageSize
        };
        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(model);

        parameters.TryGetValue("pageSize", out var pageResult);
        if (pageResult != null)
        {
            pageResult![0].Should().Be(pageSize?.ToString());
        }
        else
        {
            pageResult.Should().BeNull();
        }
    }
}

