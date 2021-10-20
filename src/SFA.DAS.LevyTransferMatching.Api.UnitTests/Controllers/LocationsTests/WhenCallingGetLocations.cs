﻿using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetLocations;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.LocationsTests
{
    public class WhenCallingGetLocations
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Locations_From_Mediator(
            string searchTerm,
            GetLocationsResult getLocationsResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LocationController locationController)
        {
            mockMediator.SetupMediatorResponseToReturnAsync<GetLocationsResult, GetLocationsQuery>(getLocationsResult, o => o.SearchTerm == searchTerm);
          
            var controllerResult = await locationController.GetLocations("test");
            var okObjectResult = controllerResult as OkObjectResult;
            var locations = okObjectResult.Value as LocationsDto;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(locations);
            Assert.AreEqual(okObjectResult.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(getLocationsResult.Locations.Count(), locations.Names.Count());
        }
    }
}
