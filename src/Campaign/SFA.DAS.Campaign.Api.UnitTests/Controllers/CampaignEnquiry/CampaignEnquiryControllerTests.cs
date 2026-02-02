using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SFA.DAS.Campaign.Api.Controllers;
using SFA.DAS.Campaign.Models;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Models;
using System;
using NSubstitute;
using SFA.DAS.Campaign.InnerApi.Requests;
using System.Net;
using NSubstitute.ExceptionExtensions;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Configuration;

namespace SFA.DAS.Campaign.Api.UnitTests.Controllers.CampaignEnquiry;

public class CampaignEnquiryControllerTests
{
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
        var logger = new LoggerFactory().CreateLogger<CampaignEnquiryController>();
        var apiClient = Substitute.For<ICampaignApiClient<CampaignApiConfiguration>>();
        var controller = new CampaignEnquiryController(logger, apiClient);
        var userData = new EnquiryUserDataModel();
        var response = new ApiResponse<EnquiryUserDataModel>(userData, HttpStatusCode.Created, null, null)
        {
            RawContent = null
        };
        apiClient.PostWithResponseCode<EnquiryUserDataModel>(Arg.Any<PostRegisterInterestApiRequest>())
            .Returns(Task.FromResult(response));

        // Act
        var result = await controller.RegisterInterest(userData);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Test]
    public async Task RegisterInterest_Returns_BadRequest_Result_On_BadRequest()
    {
        // Arrange
        var logger = new LoggerFactory().CreateLogger<CampaignEnquiryController>();
        var apiClient = Substitute.For<ICampaignApiClient<CampaignApiConfiguration>>();
        var controller = new CampaignEnquiryController(logger, apiClient);
        var userData = new EnquiryUserDataModel();
        var response = new ApiResponse<EnquiryUserDataModel>(null, HttpStatusCode.BadRequest, null, null)
        {
            RawContent = null
        };
        apiClient.PostWithResponseCode<EnquiryUserDataModel>(Arg.Any<PostRegisterInterestApiRequest>())
            .Returns(Task.FromResult(response));

        // Act
        var result = await controller.RegisterInterest(userData);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task RegisterInterest_Returns_InternalServerError_Result_On_InternalServerError()
    {
        // Arrange
        var logger = new LoggerFactory().CreateLogger<CampaignEnquiryController>();
        var apiClient = Substitute.For<ICampaignApiClient<CampaignApiConfiguration>>();
        var controller = new CampaignEnquiryController(logger, apiClient);
        var userData = new EnquiryUserDataModel();
        var response = new ApiResponse<EnquiryUserDataModel>(null, HttpStatusCode.InternalServerError, null, null)
        {
            RawContent = null
        };
        apiClient.PostWithResponseCode<EnquiryUserDataModel>(Arg.Any<PostRegisterInterestApiRequest>())
            .Returns(Task.FromResult(response));

        // Act
        var result = await controller.RegisterInterest(userData);

        // Assert
        result.Should().BeOfType<StatusCodeResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Test]
    public void RegisterInterest_Throws_InvalidOperationException_On_Unexpected_StatusCode()
    {
        // Arrange
        var logger = new LoggerFactory().CreateLogger<CampaignEnquiryController>();
        var apiClient = NSubstitute.Substitute.For<ICampaignApiClient<CampaignApiConfiguration>>();
        var controller = new CampaignEnquiryController(logger, apiClient);
        var userData = new EnquiryUserDataModel();
        var response = new ApiResponse<EnquiryUserDataModel>(null, HttpStatusCode.Forbidden, null, null)
        {
            RawContent = null
        };
        apiClient.PostWithResponseCode<EnquiryUserDataModel>(Arg.Any<PostRegisterInterestApiRequest>())
            .Returns(Task.FromResult(response));

        // Act & Assert
        FluentActions.Invoking(() => controller.RegisterInterest(userData))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Campaign Interest didn't receive a successful response from Inner API");
    }

    [Test]
    public void RegisterInterest_Logs_Error_On_Exception()
    {
        // Arrange
        var logger = Substitute.For<ILogger<CampaignEnquiryController>>();
        var apiClient = Substitute.For<ICampaignApiClient<CampaignApiConfiguration>>();
        var controller = new CampaignEnquiryController(logger, apiClient);
        var userData = new EnquiryUserDataModel();
        apiClient.PostWithResponseCode<EnquiryUserDataModel>(Arg.Any<PostRegisterInterestApiRequest>())
            .Throws(new Exception("Test exception"));
        // Act
        Func<Task> act = async () => { await controller.RegisterInterest(userData); };
        // Assert
        act.Should().ThrowAsync<Exception>().WithMessage("Test exception");
        logger.Received(1).LogError(Arg.Any<Exception>(), "Error attempting to register Campaign Interest");
    }

    [Test]
    public async Task RegisterInterest_Logs_Information_On_Success()
    {
        // Arrange
        var logger = Substitute.For<ILogger<CampaignEnquiryController>>();
        var apiClient = Substitute.For<ICampaignApiClient<CampaignApiConfiguration>>();
        var controller = new CampaignEnquiryController(logger, apiClient);
        var userData = new EnquiryUserDataModel();
        var response = new ApiResponse<EnquiryUserDataModel>(userData, HttpStatusCode.Created, null, null)
        {
            RawContent = null
        };
        apiClient.PostWithResponseCode<EnquiryUserDataModel>(Arg.Any<PostRegisterInterestApiRequest>())
            .Returns(Task.FromResult(response));

        // Act
        await controller.RegisterInterest(userData);

        // Assert
        logger.Received(1).LogInformation("Register Campaign Interest Outer API: Received request to add user details to campaign");
        logger.Received(1).LogInformation("Register Campaign Interest Outer API: Successfully added user details to campaign");
    }

    [Test]
    public async Task RegisterInterest_Logs_Warning_On_BadRequest()
    {
        // Arrange
        var logger = Substitute.For<ILogger<CampaignEnquiryController>>();
        var apiClient = Substitute.For<ICampaignApiClient<CampaignApiConfiguration>>();
        var controller = new CampaignEnquiryController(logger, apiClient);
        var userData = new EnquiryUserDataModel();
        var response = new ApiResponse<EnquiryUserDataModel>(null, HttpStatusCode.BadRequest, null, null)
        {
            RawContent = null
        };
        apiClient.PostWithResponseCode<EnquiryUserDataModel>(Arg.Any<PostRegisterInterestApiRequest>())
            .Returns(Task.FromResult(response));
        // Act
        await controller.RegisterInterest(userData);
        // Assert
        logger.Received(1).LogWarning("Register Campaign Interest Outer API received Bad request from Inner API");
    }

    [Test]
    public async Task RegisterInterest_Logs_Error_On_InternalServerError()
    {
        // Arrange
        var logger = Substitute.For<ILogger<CampaignEnquiryController>>();
        var apiClient = Substitute.For<ICampaignApiClient<CampaignApiConfiguration>>();
        var controller = new CampaignEnquiryController(logger, apiClient);
        var userData = new EnquiryUserDataModel();
        var response = new ApiResponse<EnquiryUserDataModel>(null, HttpStatusCode.InternalServerError, null, null)
        {
            RawContent = null
        };
        apiClient.PostWithResponseCode<EnquiryUserDataModel>(Arg.Any<PostRegisterInterestApiRequest>())
            .Returns(Task.FromResult(response));
        // Act
        await controller.RegisterInterest(userData);
        // Assert
        logger.Received(1).LogError("Register Campaign Interest Outer API received Internal server error from Inner API");
    }

    [Test]
    public void RegisterInterest_Throws_Exception_On_ApiClient_Exception()
    {
        // Arrange
        var logger = Substitute.For<ILogger<CampaignEnquiryController>>();
        var apiClient = Substitute.For<ICampaignApiClient<CampaignApiConfiguration>>();
        var controller = new CampaignEnquiryController(logger, apiClient);
        var userData = new EnquiryUserDataModel();
        apiClient.PostWithResponseCode<EnquiryUserDataModel>(Arg.Any<PostRegisterInterestApiRequest>())
            .Throws(new Exception("API client exception"));
        // Act & Assert
        FluentActions.Invoking(() => controller.RegisterInterest(userData))
            .Should().ThrowAsync<Exception>()
            .WithMessage("API client exception");
    }

    [Test]
    public void RegisterInterest_Throws_Exception_On_Null_UserData()
    {
        // Arrange
        var logger = Substitute.For<ILogger<CampaignEnquiryController>>();
        var apiClient = Substitute.For<ICampaignApiClient<CampaignApiConfiguration>>();
        var controller = new CampaignEnquiryController(logger, apiClient);
        EnquiryUserDataModel userData = null;
        // Act & Assert
        FluentActions.Invoking(() => controller.RegisterInterest(userData))
            .Should().ThrowAsync<ArgumentNullException>()
            .WithMessage("*userData*");
    }

    [Test]
    public void RegisterInterest_Throws_Exception_On_ApiClient_Null_Response()
    {
        // Arrange
        var logger = Substitute.For<ILogger<CampaignEnquiryController>>();
        var apiClient = Substitute.For<ICampaignApiClient<CampaignApiConfiguration>>();
        var controller = new CampaignEnquiryController(logger, apiClient);
        var userData = new EnquiryUserDataModel();
        apiClient.PostWithResponseCode<EnquiryUserDataModel>(Arg.Any<PostRegisterInterestApiRequest>())
            .Returns((ApiResponse<EnquiryUserDataModel>)null);
        // Act & Assert
        FluentActions.Invoking(() => controller.RegisterInterest(userData))
            .Should().ThrowAsync<NullReferenceException>()
            .WithMessage("*response*");
    }

    [Test]
    public void RegisterInterest_Throws_Exception_On_ApiClient_Null_StatusCode()
    {
        // Arrange
        var logger = Substitute.For<ILogger<CampaignEnquiryController>>();
        var apiClient = Substitute.For<ICampaignApiClient<CampaignApiConfiguration>>();
        var controller = new CampaignEnquiryController(logger, apiClient);
        var userData = new EnquiryUserDataModel();
        var response = new ApiResponse<EnquiryUserDataModel>(null, 0, null, null)
        {
            RawContent = null
        };
        apiClient.PostWithResponseCode<EnquiryUserDataModel>(Arg.Any<PostRegisterInterestApiRequest>())
            .Returns(Task.FromResult(response));
        // Act & Assert
        FluentActions.Invoking(() => controller.RegisterInterest(userData))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Campaign Interest didn't receive a successful response from Inner API");
    }
}
