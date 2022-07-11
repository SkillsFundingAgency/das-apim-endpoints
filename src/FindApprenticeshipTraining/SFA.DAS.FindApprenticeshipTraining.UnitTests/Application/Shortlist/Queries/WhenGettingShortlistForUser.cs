using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetShortlistForUser;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Shortlist.Queries
{
    public class WhenGettingShortlistForUser
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_The_Shortlist_From_CourseDeliveryApi_And_Course_From_CoursesApi(
            GetShortlistForUserQuery query,
            GetShortlistForUserResponse apiResponse,
            List<GetApprenticeFeedbackResponse> apprenticeFeedbackResponse,
            GetStandardsListResponse cachedCourses,
            List<GetStandardsListItem> standards,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockCourseDeliveryApiClient,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackClient,
            [Frozen] Mock<ICachedCoursesService> mockCachedCoursesService,
            GetShortlistForUserQueryHandler handler)
        {
            var shortlistFromApi = apiResponse.Shortlist.ToList();
            for (var i = 0; i < shortlistFromApi.Count; i++)
            {
                standards[i].LarsCode = shortlistFromApi[i].CourseId;
                apprenticeFeedbackResponse[i].Ukprn = shortlistFromApi[i].ProviderDetails.Ukprn;
            }
            cachedCourses.Standards = standards;
            mockCourseDeliveryApiClient
                .Setup(client => client.Get<GetShortlistForUserResponse>(
                    It.Is<GetShortlistForUserRequest>(request =>
                        request.ShortlistUserId == query.ShortlistUserId)))
                .ReturnsAsync(apiResponse);
            mockCachedCoursesService
                .Setup(service => service.GetCourses())
                .ReturnsAsync(cachedCourses);
            mockApprenticeFeedbackClient
                .Setup(s => s.PostWithResponseCode<IEnumerable<GetApprenticeFeedbackResponse>>(It.Is<PostApprenticeFeedbackRequest>
                (t => 
                ((PostApprenticeFeedbackRequestData)t.Data).Ukprns.Except(apiResponse.Shortlist.Select(s => s.ProviderDetails.Ukprn)).Count() == 0 &&
                apiResponse.Shortlist.Select(s => s.ProviderDetails.Ukprn).Except(((PostApprenticeFeedbackRequestData)t.Data).Ukprns).Count() == 0
                ),true)).ReturnsAsync(new ApiResponse<IEnumerable<GetApprenticeFeedbackResponse>>(apprenticeFeedbackResponse, HttpStatusCode.OK, string.Empty));


            var result = await handler.Handle(query, CancellationToken.None);

            result.Shortlist.Should().BeEquivalentTo(apiResponse.Shortlist,
                options => options.Excluding(item => item.Course).Excluding(item => item.ProviderDetails.ApprenticeFeedback));
            foreach (var item in result.Shortlist)
            {
                item.Course.Should().NotBeNull();
                item.Course.Should().BeEquivalentTo(cachedCourses.Standards.Single(listItem => listItem.LarsCode == item.CourseId));

                item.ProviderDetails.ApprenticeFeedback.Should().NotBeNull();
                item.ProviderDetails.ApprenticeFeedback.Should().BeEquivalentTo(apprenticeFeedbackResponse.First(s => s.Ukprn == item.ProviderDetails.Ukprn));
            }
        }
    }
}