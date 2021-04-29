﻿using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.RegisterDemand;
using SFA.DAS.EmployerDemand.Domain.Models;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.UnitTests.Application.Demand.Commands
{
    public class WhenHandlingRegisterDemandCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_Email_Sent_If_ResponseCode_Is_Created(
            RegisterDemandCommand command,
            PostCreateCourseDemand responseBody,
            [Frozen]Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> apiClient,
            [Frozen]Mock<INotificationService> mockNotificationService,
            RegisterDemandCommandHandler handler)
        {
            //Arrange
            var apiResponse = new ApiResponse<PostCreateCourseDemand>(responseBody, HttpStatusCode.Created);
            apiClient.Setup(x => x.PostWithResponseCode<PostCreateCourseDemand>(It.Is<PostCreateCourseDemandRequest>(c=>
                    
                    ((CreateCourseDemandData)c.Data).Id.Equals(command.Id)
                    && ((CreateCourseDemandData)c.Data).ContactEmailAddress.Equals(command.ContactEmailAddress)
                    && ((CreateCourseDemandData)c.Data).OrganisationName.Equals(command.OrganisationName)
                    && ((CreateCourseDemandData)c.Data).NumberOfApprentices.Equals(command.NumberOfApprentices)
                    && ((CreateCourseDemandData)c.Data).Location.LocationPoint.GeoPoint.First() == command.Lat
                    && ((CreateCourseDemandData)c.Data).Location.LocationPoint.GeoPoint.Last() == command.Lon
                    && ((CreateCourseDemandData)c.Data).Location.Name.Equals(command.LocationName)
                    && ((CreateCourseDemandData)c.Data).Course.Title.Equals(command.CourseTitle)
                    && ((CreateCourseDemandData)c.Data).Course.Level.Equals(command.CourseLevel)
                    && ((CreateCourseDemandData)c.Data).Course.Id.Equals(command.CourseId)
                    && ((CreateCourseDemandData)c.Data).Course.Route.Equals(command.CourseSector)
                )))
                .ReturnsAsync(apiResponse);

            SendEmailCommand actualEmail = null;
            mockNotificationService
                .Setup(service => service.Send(It.IsAny<SendEmailCommand>()))
                .Callback((SendEmailCommand args) => actualEmail = args)
                .Returns(Task.CompletedTask);
            var expectedEmail = new CreateDemandConfirmationEmail(
                command.ContactEmailAddress,
                command.OrganisationName, 
                command.CourseTitle, 
                command.CourseLevel,
                command.LocationName, 
                command.NumberOfApprentices);

            //Act
            var actual = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            actual.Should().Be(apiResponse.Body.Id);
            actualEmail.Tokens.Should().BeEquivalentTo(expectedEmail.Tokens);
            actualEmail.RecipientsAddress.Should().BeEquivalentTo(expectedEmail.RecipientAddress);
            actualEmail.TemplateId.Should().BeEquivalentTo(expectedEmail.TemplateId);
        }
        
         [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_Email_Not_Sent_If_ResponseCode_Is_Not_Created(
            RegisterDemandCommand command,
            PostCreateCourseDemand responseBody,
            [Frozen]Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> apiClient,
            [Frozen]Mock<INotificationService> mockNotificationService,
            RegisterDemandCommandHandler handler)
        {
            //Arrange
            var apiResponse = new ApiResponse<PostCreateCourseDemand>(responseBody, HttpStatusCode.Accepted);
            apiClient.Setup(x => x.PostWithResponseCode<PostCreateCourseDemand>(It.IsAny<PostCreateCourseDemandRequest>(
                )))
                .ReturnsAsync(apiResponse);

            //Act
            var actual = await handler.Handle(command, CancellationToken.None);
            
            //Assert
            actual.Should().Be(apiResponse.Body.Id);
            mockNotificationService.Verify(x=>x.Send(It.IsAny<SendEmailCommand>()), Times.Never);
        }

        [Test, MoqAutoData]
        public void And_Demand_Not_Saved_Then_No_Confirmation_Email(
            RegisterDemandCommand command,
            HttpRequestContentException apiException,
            [Frozen]Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> apiClient,
            [Frozen]Mock<INotificationService> mockNotificationService,
            RegisterDemandCommandHandler handler)
        {
            //Arrange
            apiClient
                .Setup(client => client.PostWithResponseCode<PostCreateCourseDemand>(It.IsAny<PostCreateCourseDemandRequest>()))
                .ThrowsAsync(apiException);

            //Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
            
            //Assert
            act.Should().Throw<HttpRequestContentException>();
            mockNotificationService.Verify(service => service.Send(It.IsAny<SendEmailCommand>()), 
                Times.Never);
        }
    }
}