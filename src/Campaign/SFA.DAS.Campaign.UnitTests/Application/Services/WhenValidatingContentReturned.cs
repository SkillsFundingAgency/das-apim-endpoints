using System.Net;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Campaign.Application.Services;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Application.Services
{
    public class WhenValidatingContentReturned
    {
        [Test, MoqAutoData]
        public void Then_False_Is_Returned_If_No_Content(
            ContentService service)
        {
            //Act
            var actual = service.HasContent(new ApiResponse<CmsContent>(null, HttpStatusCode.OK, ""));
            
            //Assert
            actual.Should().BeFalse();
        }
        
        [Test, RecursiveMoqAutoData]
        public void Then_False_Is_Returned_If_Total_Is_Zero(
            CmsContent model,
            ContentService service)
        {
            //Arrange
            model.Total = 0;
            
            //Act
            var actual = service.HasContent(new ApiResponse<CmsContent>(model, HttpStatusCode.OK, ""));
            
            //Assert
            actual.Should().BeFalse();
        }
        
        [Test, RecursiveMoqAutoData]
        public void Then_True_Is_Returned_If_Total_Is_Greater_Than_Zero_And_Not_Null_Content(
            CmsContent model,
            ContentService service)
        {
            //Act
            var actual = service.HasContent(new ApiResponse<CmsContent>(model, HttpStatusCode.OK, ""));
            
            //Assert
            actual.Should().BeTrue();
        }
    }
}