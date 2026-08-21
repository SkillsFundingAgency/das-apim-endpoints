using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Admin.Api.Controllers;
using SFA.DAS.Admin.Api.Models.UserActions;
using SFA.DAS.Admin.Application.Queries.GetAllUserActivityByCode;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Admin.Api.UnitTests.Controllers.UserActions
{
    public class WhenGettingAllUserActivityByCode
    {
        [Test, MoqAutoData]
        public async Task Then_The_AllUserActivity_Is_Returned(
            string code,
            GetAllUserActivityByCodeResponse response,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UserActionsController controller)
        {
            // Arrange
            var queryResult = new GetAllUserActivityByCodeQueryResult
            {
                UserId = response.UserId,
                GovUKIdentifier = response.GovUKIdentifier,
                EmailAddress = response.EmailAddress,
                PhoneNumber = response.PhoneNumber,
                CreatedAt = response.CreatedAt,
                LastLoginAt = response.LastLoginAt,
                IsLocked = response.IsLocked,
                LockedTime = response.LockedTime,
                UserActions = response.UserActions?.ConvertAll(a => new GetAllUserActivityByCodeQueryResult.UserAction
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    ActionType = a.ActionType,
                    ActionCode = a.ActionCode,
                    ActionTime = a.ActionTime,
                    ActionStatus = a.ActionStatus,
                    Uln = a.Uln,
                    FamilyName = a.FamilyName,
                    GivenNames = a.GivenNames,
                    CertificateId = a.CertificateId,
                    CertificateType = a.CertificateType,
                    CourseName = a.CourseName,
                    AdminActions = a.AdminActions?.ConvertAll(ad => new GetAllUserActivityByCodeQueryResult.AdminAction
                    {
                        Username = ad.Username,
                        ActionTime = ad.ActionTime,
                        Action = ad.Action
                    }),
                    UserMatches = a.UserMatches?.ConvertAll(um => new GetAllUserActivityByCodeQueryResult.UserMatch
                    {
                        Id = um.Id,
                        Uln = um.Uln,
                        FamilyName = um.FamilyName,
                        DateOfBirth = um.DateOfBirth,
                        EventTime = um.EventTime,
                        CertificateType = um.CertificateType,
                        CourseCode = um.CourseCode,
                        CourseName = um.CourseName,
                        CourseLevel = um.CourseLevel,
                        DateAwarded = um.DateAwarded,
                        ProviderName = um.ProviderName,
                        Ukprn = um.Ukprn,
                        IsMatched = um.IsMatched,
                        IsFailed = um.IsFailed
                    })
                })
            };

            mediator
                .Setup(x => x.Send(It.Is<GetAllUserActivityByCodeQuery>(q => q.Code == code), CancellationToken.None))
                .ReturnsAsync(queryResult);

            // Act
            var actual = await controller.GetAllUserActivityByCode(code) as ObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeEquivalentTo(response);

            mediator.Verify(m => m.Send(It.Is<GetAllUserActivityByCodeQuery>(q => q.Code == code), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_NotFound_Returned_If_No_Activity(
            string code,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UserActionsController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.Is<GetAllUserActivityByCodeQuery>(q => q.Code == code), CancellationToken.None))
                .ReturnsAsync((GetAllUserActivityByCodeQueryResult)null);

            // Act
            var actual = await controller.GetAllUserActivityByCode(code) as NotFoundResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            mediator.Verify(m => m.Send(It.Is<GetAllUserActivityByCodeQuery>(q => q.Code == code), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_Exception(
            string code,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UserActionsController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.IsAny<GetAllUserActivityByCodeQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.GetAllUserActivityByCode(code) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.IsAny<GetAllUserActivityByCodeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
