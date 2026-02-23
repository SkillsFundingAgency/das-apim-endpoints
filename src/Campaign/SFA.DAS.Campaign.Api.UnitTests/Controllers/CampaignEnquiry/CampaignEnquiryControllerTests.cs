using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using SFA.DAS.Campaign.Api.Controllers;
using SFA.DAS.Campaign.InnerApi.Requests;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.Campaign.Api.UnitTests.Controllers.CampaignEnquiry;

public class CampaignEnquiryControllerTests
{
    private ILogger<CampaignEnquiryController> _logger;
    private ICampaignApiClient _apiClient;
    private CampaignEnquiryController _controller;
    private EnquiryUserDataModel _userData;

    [SetUp]
    public void SetUp()
    {
        _logger = Substitute.For<ILogger<CampaignEnquiryController>>();
        _apiClient = Substitute.For<ICampaignApiClient>();
        _controller = new CampaignEnquiryController(_logger, _apiClient);
        _userData = new EnquiryUserDataModel();
    }

    [Test]
    public void RegisterInterest_Has_ProducesResponseType_Attributes()
    {
        var methodInfo = typeof(CampaignEnquiryController).GetMethod("RegisterInterest");
        var producesResponseTypeAttributes = methodInfo.GetCustomAttributes(typeof(ProducesResponseTypeAttribute), true);
        producesResponseTypeAttributes.Length.Should().Be(3);

        var createdAttribute = (ProducesResponseTypeAttribute)producesResponseTypeAttributes[0];
        createdAttribute.Type.Should().Be(typeof(EnquiryUserDataModel));
        createdAttribute.StatusCode.Should().Be(StatusCodes.Status201Created);

        var badRequestAttribute = (ProducesResponseTypeAttribute)producesResponseTypeAttributes[1];
        badRequestAttribute.Type.Should().Be(typeof(EnquiryUserDataModel));
        badRequestAttribute.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

        var internalServerErrorAttribute = (ProducesResponseTypeAttribute)producesResponseTypeAttributes[2];
        internalServerErrorAttribute.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Test]
    public async Task RegisterInterest_Returns_Created_Result_On_Success()
    {
        // Arrange
        var response = new ApiResponse<EnquiryUserDataModel>(_userData, HttpStatusCode.Created, null, null)
        {
            RawContent = null
        };
        _apiClient.PostWithResponseCode<EnquiryUserDataModel>(Arg.Any<PostRegisterInterestApiRequest>())
            .Returns(Task.FromResult(response));

        // Act
        var result = await _controller.RegisterInterest(_userData);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Test]
    public async Task RegisterInterest_Returns_BadRequest_Result_On_BadRequest()
    {
        // Arrange
        var response = new ApiResponse<EnquiryUserDataModel>(null, HttpStatusCode.BadRequest, null, null)
        {
            RawContent = null
        };
        _apiClient.PostWithResponseCode<EnquiryUserDataModel>(Arg.Any<PostRegisterInterestApiRequest>())
            .Returns(Task.FromResult(response));

        // Act
        var result = await _controller.RegisterInterest(_userData);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task RegisterInterest_Returns_InternalServerError_Result_On_InternalServerError()
    {
        // Arrange
        var response = new ApiResponse<EnquiryUserDataModel>(null, HttpStatusCode.InternalServerError, null, null)
        {
            RawContent = null
        };
        _apiClient.PostWithResponseCode<EnquiryUserDataModel>(Arg.Any<PostRegisterInterestApiRequest>())
            .Returns(Task.FromResult(response));

        // Act
        var result = await _controller.RegisterInterest(_userData);

        // Assert
        result.Should().BeOfType<StatusCodeResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Test]
    public void RegisterInterest_Throws_InvalidOperationException_On_Unexpected_StatusCode()
    {
        // Arrange
        var response = new ApiResponse<EnquiryUserDataModel>(null, HttpStatusCode.Forbidden, null, null)
        {
            RawContent = null
        };
        _apiClient.PostWithResponseCode<EnquiryUserDataModel>(Arg.Any<PostRegisterInterestApiRequest>())
            .Returns(Task.FromResult(response));

        // Act & Assert
        FluentActions.Invoking(() => _controller.RegisterInterest(_userData))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Campaign Interest didn't receive a successful response from Inner API");
    }

    [Test]
    public void RegisterInterest_Logs_Error_On_Exception()
    {
        // Arrange
        _apiClient.PostWithResponseCode<EnquiryUserDataModel>(Arg.Any<PostRegisterInterestApiRequest>())
            .Throws(new Exception("Test exception"));

        // Act
        Func<Task> act = async () => { await _controller.RegisterInterest(_userData); };

        // Assert
        act.Should().ThrowAsync<Exception>().WithMessage("Test exception");
        _logger.Received(1).LogError(Arg.Any<Exception>(), "Error attempting to register Campaign Interest");
    }

    [Test]
    public async Task RegisterInterest_Logs_Information_On_Success()
    {
        // Arrange
        var response = new ApiResponse<EnquiryUserDataModel>(_userData, HttpStatusCode.Created, null, null)
        {
            RawContent = null
        };
        _apiClient.PostWithResponseCode<EnquiryUserDataModel>(Arg.Any<PostRegisterInterestApiRequest>())
            .Returns(Task.FromResult(response));

        // Act
        await _controller.RegisterInterest(_userData);

        // Assert
        _logger.Received(1).LogInformation("Register Campaign Interest Outer API: Received request to add user details to campaign");
        _logger.Received(1).LogInformation("Register Campaign Interest Outer API: Successfully added user details to campaign");
    }

    [Test]
    public async Task RegisterInterest_Logs_Warning_On_BadRequest()
    {
        // Arrange
        var response = new ApiResponse<EnquiryUserDataModel>(null, HttpStatusCode.BadRequest, null, null)
        {
            RawContent = null
        };
        _apiClient.PostWithResponseCode<EnquiryUserDataModel>(Arg.Any<PostRegisterInterestApiRequest>())
            .Returns(Task.FromResult(response));
        // Act
        await _controller.RegisterInterest(_userData);
        // Assert
        _logger.Received(1).LogWarning("Register Campaign Interest Outer API received Bad request from Inner API");
    }

    [Test]
    public async Task RegisterInterest_Logs_Error_On_InternalServerError()
    {
        // Arrange
        var response = new ApiResponse<EnquiryUserDataModel>(null, HttpStatusCode.InternalServerError, null, null)
        {
            RawContent = null
        };
        _apiClient.PostWithResponseCode<EnquiryUserDataModel>(Arg.Any<PostRegisterInterestApiRequest>())
            .Returns(Task.FromResult(response));

        // Act
        await _controller.RegisterInterest(_userData);

        // Assert
        _logger.Received(1).LogError("Register Campaign Interest Outer API received Internal server error from Inner API");
    }

    [Test]
    public void RegisterInterest_Throws_Exception_On_ApiClient_Exception()
    {
        // Arrange
        _apiClient.PostWithResponseCode<EnquiryUserDataModel>(Arg.Any<PostRegisterInterestApiRequest>())
            .Throws(new Exception("API client exception"));

        // Act & Assert
        FluentActions.Invoking(() => _controller.RegisterInterest(_userData))
            .Should().ThrowAsync<Exception>()
            .WithMessage("API client exception");
    }

    [Test]
    public void RegisterInterest_Throws_Exception_On_Null_UserData()
    {
        // Arrange
        EnquiryUserDataModel userData = null;

        // Act & Assert
        FluentActions.Invoking(() => _controller.RegisterInterest(userData))
            .Should().ThrowAsync<ArgumentNullException>()
            .WithMessage("*userData*");
    }

    [Test]
    public void RegisterInterest_Throws_Exception_On_ApiClient_Null_Response()
    {
        // Arrange
        _apiClient.PostWithResponseCode<EnquiryUserDataModel>(Arg.Any<PostRegisterInterestApiRequest>())
            .Returns((ApiResponse<EnquiryUserDataModel>)null);

        // Act & Assert
        FluentActions.Invoking(() => _controller.RegisterInterest(_userData))
            .Should().ThrowAsync<NullReferenceException>()
            .WithMessage("*response*");
    }

    [Test]
    public void RegisterInterest_Throws_Exception_On_ApiClient_Null_StatusCode()
    {
        // Arrange
        var response = new ApiResponse<EnquiryUserDataModel>(null, 0, null, null)
        {
            RawContent = null
        };
        _apiClient.PostWithResponseCode<EnquiryUserDataModel>(Arg.Any<PostRegisterInterestApiRequest>())
            .Returns(Task.FromResult(response));

        // Act & Assert
        FluentActions.Invoking(() => _controller.RegisterInterest(_userData))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Campaign Interest didn't receive a successful response from Inner API");
    }
}
