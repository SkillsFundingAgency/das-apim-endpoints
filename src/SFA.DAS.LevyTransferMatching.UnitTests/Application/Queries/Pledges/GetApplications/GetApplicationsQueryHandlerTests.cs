﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Pledges.GetApplications
{
    [TestFixture]
    public class GetApplicationsQueryHandlerTests
    {
        private GetApplicationsQueryHandler _handler;
        private Mock<ILevyTransferMatchingService> _service;
        private Mock<ICoursesApiClient<CoursesApiConfiguration>> _coursesApiClient;
        private GetApplicationsQuery _query;
        private Models.Account _account;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _query = _fixture.Create<GetApplicationsQuery>();
            _account = _fixture.Create<Models.Account>();

            _service = new Mock<ILevyTransferMatchingService>();
            _service.Setup(x => x.GetApplications(It.Is<GetApplicationsRequest>(p => p.PledgeId == _query.PledgeId)))
                .ReturnsAsync(new GetApplicationsResponse()
                {
                    Applications = new List<SharedOuterApi.Models.Application>()
                    {
                        new SharedOuterApi.Models.Application()
                        {
                            StandardId = "1"
                        },
                        new SharedOuterApi.Models.Application()
                        {
                            StandardId = "2"
                        }
                    }
                });

            _service.Setup(x => x.GetPledge(_query.PledgeId.Value))
                .ReturnsAsync(new Pledge
                {
                    Locations = new List<LocationDataItem>(),
                    Sectors = new List<string>(),
                    JobRoles = new List<string>(),
                    Levels = new List<string>()
                });

            _coursesApiClient = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();

            _coursesApiClient.Setup(x =>
                    x.Get<GetStandardsListItem>(It.Is<GetStandardDetailsByIdRequest>(p => p.Id == "1")))
                    .ReturnsAsync(new GetStandardsListItem()
                    {
                        StandardUId = "1",
                        Title = _fixture.Create<string>(),
                        LarsCode = _fixture.Create<int>(),
                        Level = _fixture.Create<int>(),
                        StandardDates = _fixture.Create<StandardDate>()
                    })
                ;

            _coursesApiClient.Setup(x =>
                    x.Get<GetStandardsListItem>(It.Is<GetStandardDetailsByIdRequest>(p => p.Id == "2")))
                .ReturnsAsync(new GetStandardsListItem()
                {
                    StandardUId = "2",
                    Title = _fixture.Create<string>(),
                    LarsCode = _fixture.Create<int>(),
                    Level = _fixture.Create<int>(),
                    StandardDates = _fixture.Create<StandardDate>()
                })
                ;

            _coursesApiClient.Setup(x =>
                    x.Get<GetStandardsListItem>(It.Is<GetStandardDetailsByIdRequest>(p => p.Id == "2")))
                .ReturnsAsync(new GetStandardsListItem()
                {
                    StandardUId = "2",
                    Title = _fixture.Create<string>(),
                    LarsCode = _fixture.Create<int>(),
                    Level = _fixture.Create<int>(),
                    StandardDates = _fixture.Create<StandardDate>()
                })
                ;

        }

        [Test]
        [AutoData]
        public async Task Correct_Standard_Is_Added_To_The_Application()
        {
            _handler = new GetApplicationsQueryHandler(_service.Object, _coursesApiClient.Object);

            var result = await _handler.Handle(_query, CancellationToken.None);

            Assert.IsTrue(result.Applications.All(app => app.Standard.StandardUId == app.StandardId));
        }
    }
}
