using System;
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
using SFA.DAS.FindApprenticeshipTraining.Services;
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
             List<GetApprenticeFeedbackSummaryItem> apprenticeFeedbackResponse,
             List<GetEmployerFeedbackSummaryItem> employerFeedbackResponse,
             GetStandardsListResponse cachedCourses,
             List<GetStandardsListItem> standards,
             List<ShortlistItem> shortlistItems,
             [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackClient,
             [Frozen] Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>> mockEmployerFeedbackClient,
             [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpV2ApiClient,
             [Frozen] Mock<ICachedCoursesService> mockCachedCoursesService,
             [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortListApiClient,
             GetShortlistForUserQueryHandler handler)
         {
            var shortlistFromApi = apiResponse.Shortlist.ToList();
             for (var i = 0; i < shortlistFromApi.Count; i++)
             {
                 standards[i].LarsCode = shortlistFromApi[i].CourseId;
                 apprenticeFeedbackResponse[i].Ukprn = shortlistFromApi[i].ProviderDetails.Ukprn;
                 employerFeedbackResponse[i].Ukprn = shortlistFromApi[i].ProviderDetails.Ukprn; 
                 shortlistItems[i].Larscode = shortlistFromApi[i].CourseId;
                 shortlistItems[i].Ukprn = shortlistFromApi[i].ProviderDetails.Ukprn;
                 shortlistItems[i].Id = shortlistFromApi[i].Id;
                 shortlistItems[i].ShortlistUserId = query.ShortlistUserId;
                 shortlistFromApi[i].ShortlistUserId = query.ShortlistUserId;
                 shortlistItems[i].LocationDescription = shortlistFromApi[i].LocationDescription;
                 shortlistItems[i].CreatedDate = shortlistFromApi[i].CreatedDate;
                 shortlistFromApi[i].ProviderDetails.StandardId = standards[i].LarsCode;
             }

             cachedCourses.Standards = standards;

            mockShortListApiClient
                .Setup(client => client.GetAll<ShortlistItem>(
                    It.Is<GetShortlistForUserIdRequest>(request =>
                        request.ShortlistUserId == query.ShortlistUserId)))
                .ReturnsAsync(shortlistItems);

            mockCachedCoursesService
                 .Setup(service => service.GetCourses())
                 .ReturnsAsync(cachedCourses);

            mockApprenticeFeedbackClient
                 .Setup(s => s.GetAll<GetApprenticeFeedbackSummaryItem>(It.IsAny<GetApprenticeFeedbackSummaryRequest>()))
                 .ReturnsAsync(apprenticeFeedbackResponse);

            mockEmployerFeedbackClient
                .Setup(s => s.GetAll<GetEmployerFeedbackSummaryItem>(It.IsAny<GetEmployerFeedbackSummaryRequest>()))
                .ReturnsAsync(employerFeedbackResponse);

            mockRoatpV2ApiClient.Setup(x =>
                    x.Get<GetProviderDetailsForCourse>(It.IsAny<GetProviderByCourseAndUkprnRequest>()))
                .ReturnsAsync(new GetProviderDetailsForCourse());

            foreach (var shortlistItem in shortlistItems)
            {
                var providerDetails = shortlistFromApi.First(s =>
                    s.CourseId == shortlistItem.Larscode && s.ProviderDetails.Ukprn == shortlistItem.Ukprn).ProviderDetails;
                mockRoatpV2ApiClient.Setup(x =>
                        x.Get<GetProviderDetailsForCourse>( It.Is<GetProviderByCourseAndUkprnRequest>(request=>request.ProviderId == shortlistItem.Ukprn && request.CourseId == shortlistItem.Larscode && request.Latitude == shortlistItem.Latitude && request.Longitude==shortlistItem.Longitude)))
                    .ReturnsAsync(new GetProviderDetailsForCourse
                    {
                        Ukprn = shortlistItem.Ukprn, 
                        LarsCode = shortlistItem.Larscode, 
                        Latitude = shortlistItem.Latitude, 
                        Longitude = shortlistItem.Longitude,
                        Name = providerDetails.Name,
                        TradingName = providerDetails.TradingName,
                        MarketingInfo = providerDetails.MarketingInfo,
                        StandardInfoUrl = providerDetails.StandardInfoUrl,
                        Email = providerDetails.Email,
                        Phone = providerDetails.Phone,
                        AchievementRates = shortlistFromApi.First(x=>x.ProviderDetails.Ukprn==shortlistItem.Ukprn && x.ProviderDetails.StandardId==shortlistItem.Larscode).ProviderDetails.AchievementRates.ToList(),
                        DeliveryModels = shortlistFromApi.First(x => x.ProviderDetails.Ukprn == shortlistItem.Ukprn && x.ProviderDetails.StandardId == shortlistItem.Larscode).ProviderDetails.DeliveryModels.ToList(),
                        Address1 = providerDetails.ProviderAddress.Address1,
                        Address2 = providerDetails.ProviderAddress.Address2,
                        Address3 = providerDetails.ProviderAddress.Address3,
                        Address4 = providerDetails.ProviderAddress.Address4, 
                        Town = providerDetails.ProviderAddress.Town, 
                        Postcode = providerDetails.ProviderAddress.Postcode, 
                        ProviderHeadOfficeDistanceInMiles = providerDetails.ProviderAddress.DistanceInMiles
                    });
            }

            var result = await handler.Handle(query, CancellationToken.None);
        
             result.Shortlist.Should().BeEquivalentTo(apiResponse.Shortlist,
                 options => options
                     .Excluding(item => item.Course)
                     .Excluding(item => item.ProviderDetails.ApprenticeFeedback)
                     .Excluding(item => item.ProviderDetails.ShortlistId)
                     .Excluding(item => item.ProviderDetails.DeliveryTypes)
                     .Excluding(item => item.ProviderDetails.EmployerFeedback)
                     .Excluding(item=>item.ProviderDetails.DeliveryModelsShortestDistance)
             );

             foreach (var item in result.Shortlist)
             {
                 item.ProviderDetails.DeliveryTypes.Should().BeNull();
                 item.Course.Should().NotBeNull();
                 item.Course.Should().BeEquivalentTo(cachedCourses.Standards.Single(listItem => listItem.LarsCode == item.CourseId));
             
                 item.ProviderDetails.ApprenticeFeedback.Should().NotBeNull();
                 item.ProviderDetails.ApprenticeFeedback.Should().BeEquivalentTo(apprenticeFeedbackResponse.First(s => s.Ukprn == item.ProviderDetails.Ukprn));

                 item.ProviderDetails.EmployerFeedback.Should().NotBeNull();
                 item.ProviderDetails.EmployerFeedback.Should().BeEquivalentTo(employerFeedbackResponse.First(s => s.Ukprn == item.ProviderDetails.Ukprn));

            }
        }
    }
}