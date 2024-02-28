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

            Assert.That(result, Is.Not.Null);

            Assert.That(_apiResponse.Apprenticeships.Count, Is.EqualTo(result.Apprenticeships.Count()));
            Assert.That(_apiResponse.TotalApprenticeshipsFound, Is.EqualTo(result.TotalApprenticeshipsFound));

            var i = 0;

            foreach (var apprenticeship in result.Apprenticeships)
            {
                var expected = _apiResponse.Apprenticeships.ToArray()[i];
                Assert.That(expected.Id, Is.EqualTo(apprenticeship.Id));
                Assert.That(expected.TransferSenderId, Is.EqualTo(apprenticeship.TransferSenderId));
                Assert.That(expected.Uln , Is.EqualTo(apprenticeship.Uln));
                Assert.That(expected.ProviderId , Is.EqualTo(apprenticeship.ProviderId));
                Assert.That(expected.ProviderName , Is.EqualTo(apprenticeship.ProviderName));
                Assert.That(expected.FirstName , Is.EqualTo(apprenticeship.FirstName));
                Assert.That(expected.LastName , Is.EqualTo(apprenticeship.LastName));
                Assert.That(expected.CourseCode , Is.EqualTo(apprenticeship.CourseCode));
                Assert.That(expected.CourseName , Is.EqualTo(apprenticeship.CourseName));
                Assert.That(expected.StartDate , Is.EqualTo(apprenticeship.StartDate));
                Assert.That(expected.EndDate , Is.EqualTo(apprenticeship.EndDate));
                Assert.That(expected.Cost, Is.EqualTo(apprenticeship.Cost));
                Assert.That(expected.PledgeApplicationId , Is.EqualTo(apprenticeship.PledgeApplicationId));
                Assert.That(expected.HasHadDataLockSuccess, Is.EqualTo(apprenticeship.HasHadDataLockSuccess));
                Assert.That(_courseList.First().Level, Is.EqualTo(apprenticeship.CourseLevel));
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
