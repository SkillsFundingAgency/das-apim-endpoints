using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models;
using SFA.DAS.EmployerIncentives.Models.Commitments;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Services
{
    public class WhenCallingGetEligibleApprenticeships
    {
        [Test,MoqAutoData]
        public async Task Then_Each_Apprentice_Record_Is_Checked(
            List<ApprenticeshipItem> allApprenticeship,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            EmployerIncentivesService service)
        {
            //Arrange
            client.Setup(x =>
                    x.GetResponseCode(It.IsAny<GetEligibleApprenticeshipsRequest>()))
                .ReturnsAsync(HttpStatusCode.OK);
            
            //Act
            var actual = await service.GetEligibleApprenticeships(allApprenticeship);
            
            //Assert
            actual.ToList().Should().BeEquivalentTo(allApprenticeship);
            client.Verify(x => x.GetResponseCode(It.IsAny<GetEligibleApprenticeshipsRequest>()),
                Times.Exactly(allApprenticeship.Count));
        }
        
        [Test,MoqAutoData]
        public async Task Then_Each_Apprentice_Record_Is_Checked_And_If_Client_Returns_Ok_Is_Returned(
            List<ApprenticeshipItem> allApprenticeship,
            ApprenticeshipItem passingApprenticeship,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            EmployerIncentivesService service)
        { 
            //Arrange
            allApprenticeship.Add(passingApprenticeship);
            client.Setup(x =>
                 x.GetResponseCode(It.Is<GetEligibleApprenticeshipsRequest>(c =>
                     c.GetUrl.Contains(passingApprenticeship.Uln.ToString())
                     && c.GetUrl.Contains(passingApprenticeship.StartDate.ToString("yyyy-MM-dd"))
                     )))
                .ReturnsAsync(HttpStatusCode.OK);

            client.Setup(x =>
                 x.GetResponseCode(It.Is<GetEligibleApprenticeshipsRequest>(c =>
                     !c.GetUrl.Contains(passingApprenticeship.Uln.ToString())
                     && !c.GetUrl.Contains(passingApprenticeship.StartDate.ToString("yyyy-MM-dd"))
                     )))
                .ReturnsAsync(HttpStatusCode.NotFound);
            
            //Act
            var actual = await service.GetEligibleApprenticeships(allApprenticeship);
            
            //Assert
            actual.ToList().Should().ContainEquivalentOf(passingApprenticeship);
            actual.Length.Should().Be(1);
        }

        [Test, MoqAutoData]
        public void Then_An_Exception_Is_Thrown_If_The_Uln_Is_Not_Found(
            List<ApprenticeshipItem> allApprenticeship,
            ApprenticeshipItem passingApprenticeship,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            EmployerIncentivesService service)
        {
            //Arrange
            client.Setup(x =>
                    x.GetResponseCode(It.IsAny<GetEligibleApprenticeshipsRequest>()))
                .ReturnsAsync(HttpStatusCode.InternalServerError);
            
            //Act
            Assert.ThrowsAsync<ApplicationException>(() => service.GetEligibleApprenticeships(allApprenticeship));
        }

    }
}