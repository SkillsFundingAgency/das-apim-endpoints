using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Responses;

namespace SFA.DAS.LearnerData.UnitTests.Extensions;

[TestFixture]
public class HttpContextExtensionsTests
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private DefaultHttpContext _httpContext;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [SetUp]
    public void Setup()
    {
        _httpContext = new DefaultHttpContext();
        _httpContext.Request.Scheme = "https";
        _httpContext.Request.Host = new HostString("example.com");
        _httpContext.Request.Path = "/controller/action";
    }

    [Test]
    public void Adds_prev_and_next_links_when_not_first_or_last_page()
    {
        _httpContext.Request.QueryString = new QueryString("?someParam=value");

        var query = new TestPagedQuery(page: 2, pageSize: 10);
        var response = new TestPagedQueryResult
        {
            Page = 2,
            PageSize = 10,
            TotalItems = 35 // So pages: 1, 2, 3, 4
        };

        _httpContext.SetPageLinksInResponseHeaders(query, response);

        var links = _httpContext.Response.Headers["links"].ToString();
        links.Should().Contain("page=1").And.Contain("rel=\"prev\"");
        links.Should().Contain("page=3").And.Contain("rel=\"next\"");
        links.Should().Contain("someParam=value");
    }

    [Test]
    public void Adds_only_next_link_on_first_page()
    {
        var query = new TestPagedQuery(page: 1, pageSize: 10);
        var response = new TestPagedQueryResult
        {
            Page = 1,
            PageSize = 10,
            TotalItems = 25
        };

        _httpContext.SetPageLinksInResponseHeaders(query, response);

        var links = _httpContext.Response.Headers["links"].ToString();
        links.Should().NotContain("rel=\"prev\"");
        links.Should().Contain("rel=\"next\"").And.Contain("page=2");
    }

    [Test]
    public void Adds_only_prev_link_on_last_page()
    {
        var query = new TestPagedQuery(page: 3, pageSize: 10);
        var response = new TestPagedQueryResult
        {
            Page = 3,
            PageSize = 10,
            TotalItems = 30
        };

        _httpContext.SetPageLinksInResponseHeaders(query, response);

        var links = _httpContext.Response.Headers["links"].ToString();
        links.Should().Contain("rel=\"prev\"").And.Contain("page=2");
        links.Should().NotContain("rel=\"next\"");
    }

    [Test]
    public void Adds_no_links_on_single_page()
    {
        var query = new TestPagedQuery(page: 1, pageSize: 20);
        var response = new TestPagedQueryResult
        {
            Page = 1,
            PageSize = 20,
            TotalItems = 15
        };

        _httpContext.SetPageLinksInResponseHeaders(query, response);

        _httpContext.Response.Headers.ContainsKey("links").Should().BeTrue();
        _httpContext.Response.Headers["links"].ToString().Should().BeEmpty();
    }

    [Test]
    public void Preserves_existing_query_parameters()
    {
        _httpContext.Request.QueryString = new QueryString("?foo=bar&other=value&page=2");

        var query = new TestPagedQuery(page: 2, pageSize: 5);
        var response = new TestPagedQueryResult
        {
            Page = 2,
            PageSize = 5,
            TotalItems = 20
        };

        _httpContext.SetPageLinksInResponseHeaders(query, response);

        var links = _httpContext.Response.Headers["links"].ToString();
        links.Should().Contain("foo=bar").And.Contain("other=value");
        links.Should().NotContain("page=2&pageSize=5"); // base should remove original pagination params
    }
}

public class TestItem { }

public class TestPagedQuery : PagedQuery
{
    public TestPagedQuery(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }
}

public class TestPagedQueryResult : PagedQueryResult<TestItem>
{

}