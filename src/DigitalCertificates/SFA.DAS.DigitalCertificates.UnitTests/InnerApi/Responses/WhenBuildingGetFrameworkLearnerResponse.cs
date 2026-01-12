using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Responses
{
    public class WhenBuildingGetFrameworkLearnerResponse
    {
        [Test, AutoData]
        public void Then_Response_Properties_Are_Set_Correctly(
            Guid id,
            string frameworkCertificateNumber,
            string apprenticeForename,
            string apprenticeSurname,
            long? apprenticeUln,
            string frameworkName,
            string pathwayName,
            string apprenticeshipLevelName,
            DateTime? apprenticeStartdate,
            DateTime? printRequestedAt,
            string printRequestedBy,
            DateTime certificationDate,
            string certificateReference)
        {
            // Arrange & Act
            var response = new GetFrameworkLearnerResponse
            {
                Id = id,
                FrameworkCertificateNumber = frameworkCertificateNumber,
                ApprenticeForename = apprenticeForename,
                ApprenticeSurname = apprenticeSurname,
                ApprenticeULN = apprenticeUln,
                FrameworkName = frameworkName,
                PathwayName = pathwayName,
                ApprenticeshipLevelName = apprenticeshipLevelName,
                QualificationsAndAwardingBodies = new List<QualificationAndAwardingBody>
                {
                    new QualificationAndAwardingBody { Name = "Q1", AwardingBody = "A1" }
                },
                ProviderName = "Provider",
                EmployerName = "Employer",
                ApprenticeStartdate = apprenticeStartdate,
                PrintRequestedAt = printRequestedAt,
                PrintRequestedBy = printRequestedBy,
                CertificationDate = certificationDate,
                CertificateReference = certificateReference
            };

            // Assert
            response.Id.Should().Be(id);
            response.FrameworkCertificateNumber.Should().Be(frameworkCertificateNumber);
            response.ApprenticeForename.Should().Be(apprenticeForename);
            response.ApprenticeSurname.Should().Be(apprenticeSurname);
            response.ApprenticeULN.Should().Be(apprenticeUln);
            response.FrameworkName.Should().Be(frameworkName);
            response.PathwayName.Should().Be(pathwayName);
            response.ApprenticeshipLevelName.Should().Be(apprenticeshipLevelName);
            response.QualificationsAndAwardingBodies.Should().NotBeNull();
            response.QualificationsAndAwardingBodies.Should().HaveCount(1);
            response.QualificationsAndAwardingBodies[0].Name.Should().Be("Q1");
            response.QualificationsAndAwardingBodies[0].AwardingBody.Should().Be("A1");
            response.ProviderName.Should().Be("Provider");
            response.EmployerName.Should().Be("Employer");
            response.ApprenticeStartdate.Should().Be(apprenticeStartdate);
            response.PrintRequestedAt.Should().Be(printRequestedAt);
            response.PrintRequestedBy.Should().Be(printRequestedBy);
            response.CertificationDate.Should().Be(certificationDate);
            response.CertificateReference.Should().Be(certificateReference);
        }
    }
}
