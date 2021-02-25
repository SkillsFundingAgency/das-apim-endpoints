using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Services;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.TrainingCourses.Services
{
    public class WhenGettingShortlistItemsForUser
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_Shortlist_Items_Returned_For_User(
            Guid userId,
            GetShortlistUserItemCountResponse shortlistResponse,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> client,
            ShortlistService service)
        {
            //Arrange
            client
                .Setup(x => x.Get<GetShortlistUserItemCountResponse>(
                    It.Is<GetShortlistUserItemCountRequest>(c => c.GetUrl.Contains(userId.ToString()))))
                .ReturnsAsync(shortlistResponse);
            
            //Act
            var actual = await service.GetShortlistItemCount(userId);
            
            //Assert
            actual.Should().Be(shortlistResponse.Count);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_There_Is_No_Shortlist_Zero_Is_Returned(
            Guid userId,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> client,
            ShortlistService service)
        {
            //Arrange
            client
                .Setup(x => x.Get<GetShortlistUserItemCountResponse>(
                    It.Is<GetShortlistUserItemCountRequest>(c => c.GetUrl.Contains(userId.ToString()))))
                .ReturnsAsync((GetShortlistUserItemCountResponse)null);
            
            //Act
            var actual = await service.GetShortlistItemCount(userId);
            
            //Assert
            actual.Should().Be(0);
        }
        
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Not_Called_If_Null_And_Zero_Returned(
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> client,
            ShortlistService service)
        {
            //Act
            var actual = await service.GetShortlistItemCount(null);
            
            //Assert
            actual.Should().Be(0);
            client.Verify(x => x.Get<GetShortlistUserItemCountResponse>(
                    It.IsAny<GetShortlistUserItemCountRequest>()), Times.Never);
        }
        
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Not_Called_If_Empty_Guid_And_Zero_Returned(
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> client,
            ShortlistService service)
        {
            //Act
            var actual = await service.GetShortlistItemCount(Guid.Empty);
            
            //Assert
            actual.Should().Be(0);
            client.Verify(x => x.Get<GetShortlistUserItemCountResponse>(
                It.IsAny<GetShortlistUserItemCountRequest>()), Times.Never);
        }
    }
}