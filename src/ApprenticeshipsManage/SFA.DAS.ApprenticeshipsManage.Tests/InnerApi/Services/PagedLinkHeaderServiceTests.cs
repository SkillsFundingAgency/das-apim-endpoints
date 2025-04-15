using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using SFA.DAS.ApprenticeshipsManage.Application.Queries.GetApprenticeships;
using SFA.DAS.ApprenticeshipsManage.Infrastructure;
using SFA.DAS.ApprenticeshipsManage.InnerApi.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeshipsManage.Tests.InnerApi.Services;
public class PagedLinkHeaderServiceTests
{
    private const string Schema = "https";
    private const string Domain = "mydomain.test";
    private const string Path = "/api/get-data";

    private readonly QueryCollection _queryCollection = new(new Dictionary<string, StringValues>
    {
        { "start", "2025-01-01" },
        { "end", "2026-01-01" },
    });

    [Test, MoqAutoData]
    public void Then_Next_And_Prev_Links_Are_Returned_When_Not_First_Nor_Last_Page(
        PagedQuery request,
        GetApprenticeshipsQueryResult response,
        Mock<HttpContext> httpContext,
        [Frozen] Mock<IHttpContextAccessor> mockHttpContextAccessor,
        PagedLinkHeaderService sut
    )
    {
        response.TotalItems = 100;
        response.Page = 2;
        response.PageSize = 10;
        request.PageSize = 10;
        request.Page = 2;

        var httpRequest = new Mock<HttpRequest>();
        httpRequest.Setup(x => x.Scheme).Returns(Schema);
        httpRequest.Setup(x => x.Host).Returns(new HostString(Domain));
        httpRequest.Setup(x => x.Path).Returns(Path);


        httpRequest.Setup(x => x.Query).Returns(_queryCollection);

        httpContext.Setup(m => m.Request).Returns(httpRequest.Object);

        mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext.Object);

        var links = sut.GetPageLinks(request, response);
        links.Should().NotBeNull();

        var prevLink = links.Value.First().Split(",").FirstOrDefault(x => x.Contains("rel=\"prev\""));
        var nextLink = links.Value.First().Split(",").FirstOrDefault(x => x.Contains("rel=\"next\""));

        prevLink.Should().NotBeNull();
        nextLink.Should().NotBeNull();

        prevLink.Split(";").First().Should().Be($"{Schema}://{Domain}{Path}?start=2025-01-01&end=2026-01-01&page={request.Page - 1}&pageSize={response.PageSize}");
        nextLink.Split(";").First().Should().Be($"{Schema}://{Domain}{Path}?start=2025-01-01&end=2026-01-01&page={request.Page + 1}&pageSize={response.PageSize}");
    }

    [Test, MoqAutoData]
    public void Then_Only_Next_Links_Are_Returned_When_First_Page(
        PagedQuery request,
        GetApprenticeshipsQueryResult response,
        Mock<HttpContext> httpContext,
        [Frozen] Mock<IHttpContextAccessor> mockHttpContextAccessor,
        PagedLinkHeaderService sut
    )
    {
        response.TotalItems = 100;
        response.Page = 1;
        response.PageSize = 10;
        request.PageSize = 10;
        request.Page = 1;

        var httpRequest = new Mock<HttpRequest>();
        httpRequest.Setup(x => x.Scheme).Returns(Schema);
        httpRequest.Setup(x => x.Host).Returns(new HostString(Domain));
        httpRequest.Setup(x => x.Path).Returns(Path);
        httpRequest.Setup(x => x.Query).Returns(_queryCollection);

        httpContext.Setup(m => m.Request).Returns(httpRequest.Object);

        mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext.Object);

        var links = sut.GetPageLinks(request, response);
        links.Should().NotBeNull();

        var prevLink = links.Value.First().Split(",").FirstOrDefault(x => x.Contains("rel=\"prev\""));
        var nextLink = links.Value.First().Split(",").FirstOrDefault(x => x.Contains("rel=\"next\""));

        prevLink.Should().BeNull();
        nextLink.Should().NotBeNull();

        nextLink.Split(";").First().Should().Be($"{Schema}://{Domain}{Path}?start=2025-01-01&end=2026-01-01&page={request.Page + 1}&pageSize={response.PageSize}");
    }

    [Test, MoqAutoData]
    public void Then_Only_Prev_Links_Are_Returned_When_Last_Page(
        PagedQuery request,
        GetApprenticeshipsQueryResult response,
        Mock<HttpContext> httpContext,
        [Frozen] Mock<IHttpContextAccessor> mockHttpContextAccessor,
        PagedLinkHeaderService sut
    )
    {
        response.TotalItems = 100;
        response.Page = 10;
        response.PageSize = 10;
        request.PageSize = 10;
        request.Page = 10;

        var httpRequest = new Mock<HttpRequest>();
        httpRequest.Setup(x => x.Scheme).Returns(Schema);
        httpRequest.Setup(x => x.Host).Returns(new HostString(Domain));
        httpRequest.Setup(x => x.Path).Returns(Path);
        httpRequest.Setup(x => x.Query).Returns(_queryCollection);

        httpContext.Setup(m => m.Request).Returns(httpRequest.Object);

        mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext.Object);

        var links = sut.GetPageLinks(request, response);
        links.Should().NotBeNull();

        var prevLink = links.Value.First().Split(",").FirstOrDefault(x => x.Contains("rel=\"prev\""));
        var nextLink = links.Value.First().Split(",").FirstOrDefault(x => x.Contains("rel=\"next\""));

        nextLink.Should().BeNull();
        prevLink.Should().NotBeNull();

        prevLink.Split(";").First().Should().Be($"{Schema}://{Domain}{Path}?start=2025-01-01&end=2026-01-01&page={request.Page - 1}&pageSize={response.PageSize}");
    }
}
