using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Api.Controllers;
using SFA.DAS.FindApprenticeshipJobs.Api.Models;
using SFA.DAS.FindApprenticeshipJobs.Application.Commands.SavedSearch.SendNotification;
using SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.FindApprenticeshipJobs.Api.UnitTests.Controllers
{
    [TestFixture]
    public class WhenPostSavedSearchSendNotification
    {
        [Test, MoqAutoData]
        public async Task Then_NoContent_Returned_From_Mediator(
            SavedSearchApiRequest mockApiRequest,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SavedSearchesController sut)
        {
            mockMediator.Setup(x => x.Send(It.IsAny<PostSendSavedSearchNotificationCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            var actual = await sut.SendNotification(mockApiRequest, It.IsAny<CancellationToken>()) as NoContentResult;
            actual.Should().NotBeNull();

            mockMediator.Verify(x => x.Send(It.IsAny<PostSendSavedSearchNotificationCommand>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_An_Exception_Then_InternalServerError_Returned(
            SavedSearchApiRequest mockApiRequest,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] SavedSearchesController sut)
        {
            mockMediator.Setup(x => x.Send(It.IsAny<PostSendSavedSearchNotificationCommand>(),
                It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            var actual = await sut.SendNotification(mockApiRequest, It.IsAny<CancellationToken>()) as StatusCodeResult;

            actual.Should().NotBeNull();
            actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task Handler_Uses_Level_Codes_In_SearchParams()
        {
            // arrange
            var apiClient = new Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>>();
            apiClient.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchSavedSearchApiRequest>()))
                .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, string.Empty));

            var notificationService = new Mock<INotificationService>();
            notificationService.Setup(x => x.Send(It.IsAny<SendEmailCommand>())).Returns(Task.CompletedTask);

            var env = new EmailEnvironmentHelper("TEST");

            var handler = new PostSendSavedSearchNotificationCommandHandler(apiClient.Object, notificationService.Object, env);

            var command = new PostSendSavedSearchNotificationCommand
            {
                Id = Guid.NewGuid(),
                User = new PostSendSavedSearchNotificationCommand.UserDetails { Id = Guid.NewGuid(), FirstName = "Test", Email = "a@b.com" },
                Vacancies = new List<PostSendSavedSearchNotificationCommand.Vacancy>
                {
                    new PostSendSavedSearchNotificationCommand.Vacancy
                    {
                        Title = "T",
                        VacancyReference = "123",
                        EmployerName = "E",
                        EmployerLocation = new Address { Postcode = "P" },
                        ClosingDate = "2022-12-31"
                    }
                },
                Levels = new List<PostSendSavedSearchNotificationCommand.Level>
                {
                    new() { Code = 2, Name = "GCSE" },
                    new() { Code = 3, Name = "A level" }
                }
            };

            // act
            await handler.Handle(command, CancellationToken.None);

            // assert
            notificationService.Verify(x => x.Send(It.Is<SendEmailCommand>(c => c.Tokens.ContainsKey("searchParams") && c.Tokens["searchParams"].ToString().Contains("Apprenticeship level: Level 2, Level 3"))), Times.Once);
        }
    }
}
