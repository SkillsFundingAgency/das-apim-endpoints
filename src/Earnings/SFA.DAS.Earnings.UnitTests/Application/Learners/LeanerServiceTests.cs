using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SFA.DAS.Earnings.Api;
using SFA.DAS.Earnings.Api.Controllers;
using SFA.DAS.Earnings.Api.Learnerdata;

namespace SFA.DAS.Earnings.UnitTests.Application.Learners;

public class LeanerServiceTests
{
    private LearnerDataController _controller;
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;
    
    [SetUp]
    public void SetUp()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }
    
    [TearDown]
    public void TearDown()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }

    [Test]
    public async Task Get_first_page()
    {
        // Arrange
        var page = 1;
        var pageSize = 10;
        var ukprn = 12345678;
        var academicyear = 2223;
        
        var url = $"/providers/{ukprn}/academicyears/{academicyear}/apprenticeships?page={page}&pageSize={pageSize}";

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        Assert.That(HttpStatusCode.OK, Is.EqualTo(response.StatusCode));

        var headerValues = string.Join(" ", response.Headers.GetValues("Link"));
        Assert.That(headerValues.Contains("rel=\"prev\""),Is.False, "'prev' link should not be defined");
        Assert.That(headerValues.Contains("rel=\"next\""), Is.True, "Missing 'next' link");
    }
    
    [Test]
    public async Task Get_last_page()
    {
        // Arrange
        var page = 100;
        var pageSize = 10;
        var ukprn = 12345678;
        var academicyear = 2223;
        
        var url = $"/providers/{ukprn}/academicyears/{academicyear}/apprenticeships?page={page}&pageSize={pageSize}";

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        Assert.That(HttpStatusCode.OK, Is.EqualTo(response.StatusCode));

        var headerValues = string.Join(" ", response.Headers.GetValues("Link"));
        Assert.That(headerValues.Contains("rel=\"prev\""),Is.True, "'prev' link should not be defined");
        Assert.That(headerValues.Contains("rel=\"next\""), Is.False, "Missing 'next' link");
    }
    
    [Test]
    public async Task Get_second_page()
    {
        // Arrange
        var page = 2;
        var pageSize = 10;
        var ukprn = 12345678;
        var academicyear = 2223;
        
        var url = $"/providers/{ukprn}/academicyears/{academicyear}/apprenticeships?page={page}&pageSize={pageSize}";

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        Assert.That(HttpStatusCode.OK, Is.EqualTo(response.StatusCode));

        var headerValues = string.Join(" ", response.Headers.GetValues("Link"));
        Assert.That(headerValues.Contains("page=1&pageSize=10>; rel=\"prev\""),Is.True, "'prev' link should not be defined");
        Assert.That(headerValues.Contains("page=3&pageSize=10>; rel=\"next\""), Is.True, "Missing 'next' link");
    }
    
    [Test]
    public async Task Get_different_pagesize()
    {
        // Arrange
        var page = 2;
        var pageSize = 23;
        var ukprn = 12345678;
        var academicyear = 2223;
        
        var url = $"/providers/{ukprn}/academicyears/{academicyear}/apprenticeships?page={page}&pageSize={pageSize}";

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        Assert.That(HttpStatusCode.OK, Is.EqualTo(response.StatusCode));

        var content = response.Content.ReadAsStringAsync().Result;
        var apprentices = JsonConvert.DeserializeObject<PagedResult>(content);
        
        Assert.That(apprentices.Apprenticeships.Count, Is.EqualTo(23));
        
        var headerValues = string.Join(" ", response.Headers.GetValues("Link"));
        Assert.That(headerValues.Contains("page=1&pageSize=23>; rel=\"prev\""),Is.True, "'prev' link should not be defined");
        Assert.That(headerValues.Contains("page=3&pageSize=23>; rel=\"next\""), Is.True, "Missing 'next' link");
    }
}