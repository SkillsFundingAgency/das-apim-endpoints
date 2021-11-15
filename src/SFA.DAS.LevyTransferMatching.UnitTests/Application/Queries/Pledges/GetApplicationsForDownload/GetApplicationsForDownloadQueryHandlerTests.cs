using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Server.Kestrel.Core.Features;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplicationsForDownload;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Pledges.GetApplicationsForDownload
{
    public class GetApplicationsForDownloadQueryHandlerTests
    {
        private GetApplicationsForDownloadQueryHandler _handler;
        private Mock<ILevyTransferMatchingService> _service;
        private Mock<ICoursesApiClient<CoursesApiConfiguration>> _coursesApiClient;
        private Mock<IReferenceDataService> _referenceDataService;
        private readonly Fixture _fixture = new Fixture();
        private const string StandardId = "1";

        [SetUp]
        public void SetUp()
        {
            _coursesApiClient = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();
            _service = new Mock<ILevyTransferMatchingService>();
            _referenceDataService = new Mock<IReferenceDataService>();
            
            _handler = new GetApplicationsForDownloadQueryHandler(_coursesApiClient.Object, _service.Object,
                _referenceDataService.Object);
        }

        [Test]
        [MoqAutoData]
        public async Task When_No_Applications_Exist_Returns_Empty_Object(GetApplicationsForDownloadQuery query)
        {
            SetupServiceToReturnResponse(query, new GetApplicationsResponse
            {
              //  Applications = new List<SharedOuterApi.Models.Application>()
            });
            
            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.IsFalse(result.Applications.Any());
        }

        [Test]
        [MoqAutoData]
        public async Task When_Applications_Exist_Returns_Built_Model(GetApplicationsForDownloadQuery query, GetApplicationsResponse response)
        {
            SetupServiceToReturnResponse(query, response);
            SetupReferenceDataService();
            SetupRetrievalOfStandardTasks();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.IsTrue(result.Applications.All(app => app.Standard.StandardUId == StandardId));
        }

        private void SetupReferenceDataService()
        {
            _referenceDataService.Setup(x => x.GetJobRoles()).ReturnsAsync(new List<ReferenceDataItem>());
            _referenceDataService.Setup(x => x.GetLevels()).ReturnsAsync(new List<ReferenceDataItem>());
            _referenceDataService.Setup(x => x.GetSectors()).ReturnsAsync(new List<ReferenceDataItem>());
        }

        private void SetupServiceToReturnResponse(GetApplicationsForDownloadQuery query, GetApplicationsResponse response)
        {
            if (response.Applications.Any())
            {
                //var responseApplications = new List<SharedOuterApi.Models.Application> {response.Applications.First()};
                //response.Applications = responseApplications;
                response.Applications.First().StandardId = StandardId;
            }

            _service.Setup(o =>
                    o.GetApplications(It.Is<GetApplicationsRequest>(q =>
                        q.AccountId == query.AccountId && q.PledgeId == query.PledgeId)))
                .ReturnsAsync(response);
        }

        private void SetupRetrievalOfStandardTasks()
        {
            _coursesApiClient.Setup(x =>
                    x.Get<GetStandardsListItem>(It.Is<GetStandardDetailsByIdRequest>(p => p.Id == "1")))
                .ReturnsAsync(new GetStandardsListItem()
                {
                    StandardUId = StandardId,
                    Title = _fixture.Create<string>(),
                    LarsCode = _fixture.Create<int>(),
                    Level = _fixture.Create<int>(),
                    StandardDates = _fixture.Create<StandardDate>()
                });
        }
    }
}
