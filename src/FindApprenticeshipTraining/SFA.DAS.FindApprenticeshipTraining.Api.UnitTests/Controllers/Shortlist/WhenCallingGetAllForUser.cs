using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetShortlistForUser;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;
using GetShortlistForUserResponse = SFA.DAS.FindApprenticeshipTraining.Api.Models.GetShortlistForUserResponse;
using GetShortlistItem = SFA.DAS.FindApprenticeshipTraining.Api.Models.GetShortlistItem;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.Shortlist
{
  
    public class WhenCallingGetAllForUser
    {
        private const string Location1 = "location 1";
        private const string Location2 = "location 2";
       
        [Test]
        public async Task Then_Gets_Shortlist_For_User_From_Mediator_Empty_Shortlist()
        {
            var mockMediator = new Mock<IMediator>();
            var shortlistUserId = Guid.NewGuid();
            var mediatorResult = new GetShortlistForUserResult();
            var controller = new ShortlistController(mockMediator.Object, new Mock<ILogger<ShortlistController>>().Object);

            mediatorResult.Shortlist = new List<InnerApi.Responses.GetShortlistItem>();

            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetShortlistForUserQuery>(query => query.ShortlistUserId == shortlistUserId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetAllForUser(shortlistUserId) as ObjectResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetShortlistForUserResponse;
            model!.Shortlist.Should().BeEquivalentTo(mediatorResult.Shortlist.Select(item => (GetShortlistItem)item));
            model.Shortlist.Should().BeEmpty();
        }

        [TestCase(2,LocationType.Provider,1,LocationType.Provider,Location2)]
        [TestCase(1,  LocationType.Provider,  2, LocationType.Provider, Location1)]
        [TestCase(2, LocationType.Provider, 1, LocationType.Regional, Location2)]
        [TestCase(1, LocationType.Provider, 2, LocationType.Regional, Location1)]
        [TestCase(1, LocationType.Provider, 1, LocationType.Provider, Location1)]
        [TestCase(1, LocationType.National, 1, LocationType.Provider, Location1)]
        [TestCase(1, LocationType.Provider, 1, LocationType.National, Location2)]
        public async Task Then_Gets_Shortlist_For_User_From_Mediator(decimal distance1,LocationType locationType1,  decimal distance2, LocationType locationType2,string expectedFirstDescription)
         {
             var mockMediator = new Mock<IMediator>();
             var shortlistUserId = Guid.NewGuid();
             var mediatorResult = new GetShortlistForUserResult();
             var controller = new ShortlistController(mockMediator.Object, new Mock<ILogger<ShortlistController>>().Object);

             var shortlist = new List<InnerApi.Responses.GetShortlistItem>();
             shortlist.Add(BuildShortlistItem(Location1, distance1, locationType1));
             shortlist.Add(BuildShortlistItem(Location2, distance2, locationType2));

             mediatorResult.Shortlist = shortlist;

             mockMediator
                 .Setup(mediator => mediator.Send(
                     It.Is<GetShortlistForUserQuery>(query => query.ShortlistUserId == shortlistUserId),
                     It.IsAny<CancellationToken>()))
                 .ReturnsAsync(mediatorResult);

             var controllerResult = await controller.GetAllForUser(shortlistUserId) as ObjectResult;

             controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
             var model = controllerResult.Value as GetShortlistForUserResponse;
             model!.Shortlist.Should().BeEquivalentTo(mediatorResult.Shortlist.Select(item => (GetShortlistItem)item));
             model.Shortlist.First().LocationDescription.Should().Be(expectedFirstDescription);
         }

         [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            Guid shortlistUserId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ShortlistController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetShortlistForUserQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetAllForUser(shortlistUserId) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        private static InnerApi.Responses.GetShortlistItem BuildShortlistItem(string description1, decimal distance1, LocationType locationType)
        {
            return new InnerApi.Responses.GetShortlistItem
            {
                LocationDescription = description1,
                Course = new GetStandardsListItem { TypicalJobTitles = string.Empty, StandardDates = new StandardDate() },
                ProviderDetails = new GetProviderStandardItem
                {
                    ProviderAddress = new GetProviderStandardItemAddress(),
                    DeliveryModels = new List<DeliveryModel>
                    {
                        new DeliveryModel {DistanceInMiles = distance1, LocationType = locationType},
                    },
                    AchievementRates = new List<GetAchievementRateItem>(),
                }
            };
        }
    }
}