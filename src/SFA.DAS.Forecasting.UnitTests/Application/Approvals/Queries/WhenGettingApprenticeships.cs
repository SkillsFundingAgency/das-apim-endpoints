using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Forecasting.Application.Approvals.Queries.GetApprenticeships;
using SFA.DAS.Forecasting.InnerApi.Requests;
using SFA.DAS.Forecasting.InnerApi.Responses;
using SFA.DAS.Forecasting.Models.Courses;
using SFA.DAS.Forecasting.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Forecasting.UnitTests.Application.Approvals.Queries
{
    [TestFixture]
    public class WhenGettingApprenticeships
    {
        private GetApprenticeshipsQueryHandler _handler;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;
        private GetApprenticeshipsResponse _apiResponse;
        private readonly Fixture _fixture = new Fixture();
        private GetApprenticeshipsQuery _query;
        private Mock<ICourseLookupService> _courseLookupService;
        private List<Course> _courseList;

        [SetUp]
        public void Setup()
        {
            _apiResponse = _fixture.Create<GetApprenticeshipsResponse>();
            _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _apiClient.Setup(x => x.Get<GetApprenticeshipsResponse>(It.IsAny<GetApprenticeshipsRequest>()))
                .ReturnsAsync(_apiResponse);

            _courseList = _fixture.Create<List<Course>>();
            _courseLookupService = new Mock<ICourseLookupService>();
            _courseLookupService.Setup(x => x.GetAllCourses()).ReturnsAsync(_courseList);
            _apiResponse.Apprenticeships.ForEach(x => x.CourseCode = _courseList.First().Id);

            _handler = new GetApprenticeshipsQueryHandler(_apiClient.Object, _courseLookupService.Object);

            _query = _fixture.Create<GetApprenticeshipsQuery>();
        }

        [Test]
        public async Task Then_Apprenticeships_Are_Returned_Correctly()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);

            Assert.IsNotNull(result);

            Assert.AreEqual(_apiResponse.Apprenticeships.Count, result.Apprenticeships.Count());
            Assert.AreEqual(_apiResponse.TotalApprenticeshipsFound, result.TotalApprenticeshipsFound);

            var i = 0;

            foreach (var apprenticeship in result.Apprenticeships)
            {
                var expected = _apiResponse.Apprenticeships.ToArray()[i];
                Assert.AreEqual(expected.Id, apprenticeship.Id);
                Assert.AreEqual(expected.TransferSenderId, apprenticeship.TransferSenderId);
                Assert.AreEqual(expected.Uln , apprenticeship.Uln);
                Assert.AreEqual(expected.ProviderId , apprenticeship.ProviderId);
                Assert.AreEqual(expected.ProviderName , apprenticeship.ProviderName);
                Assert.AreEqual(expected.FirstName , apprenticeship.FirstName);
                Assert.AreEqual(expected.LastName , apprenticeship.LastName);
                Assert.AreEqual(expected.CourseCode , apprenticeship.CourseCode);
                Assert.AreEqual(expected.CourseName , apprenticeship.CourseName);
                Assert.AreEqual(expected.StartDate , apprenticeship.StartDate);
                Assert.AreEqual(expected.EndDate , apprenticeship.EndDate);
                Assert.AreEqual(expected.Cost, apprenticeship.Cost);
                Assert.AreEqual(expected.PledgeApplicationId , apprenticeship.PledgeApplicationId);
                Assert.AreEqual(expected.HasHadDataLockSuccess, apprenticeship.HasHadDataLockSuccess);
                Assert.AreEqual(_courseList.First().Level, apprenticeship.CourseLevel);
                i++;
            }
        }

        [Test]
        public async Task Then_Apprenticeships_Are_Filtered_By_AccountId()
        {
            await _handler.Handle(_query, CancellationToken.None);
            _apiClient.Verify(x => x.Get<GetApprenticeshipsResponse>(It.Is<GetApprenticeshipsRequest>(r => r.AccountId == _query.AccountId)));
               
        }

        [Test]
        public async Task Then_Apprenticeships_Are_Filtered_By_Status()
        {
            await _handler.Handle(_query, CancellationToken.None);
            _apiClient.Verify(x => x.Get<GetApprenticeshipsResponse>(It.Is<GetApprenticeshipsRequest>(r => r.Status == _query.Status)));
            
        }

        [Test]
        public async Task Then_Paging_Options_Are_Reflected()
        {
            await _handler.Handle(_query, CancellationToken.None);
            _apiClient.Verify(x => x.Get<GetApprenticeshipsResponse>(It.Is<GetApprenticeshipsRequest>(r => r.PageNumber == _query.PageNumber && r.PageItemCount == _query.PageItemCount)));
        }
    }
}
