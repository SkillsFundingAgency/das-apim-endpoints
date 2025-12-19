using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Responses.Assessor
{
    public class WhenBuildingGetCertificateByIdResponse
    {
        [Test, AutoData]
        public void Then_Response_Can_Be_Constructed(
            Guid organisationId,
            string learnerFamilyName,
            string learnerGivenNames,
            long uln,
            string certificateType,
            string standardReference,
            string standardName,
            int standardLevel,
            DateTime? achievementDate,
            string overallGrade,
            string providerName,
            long providerUkPrn,
            string employerName,
            DateTime? learningStartDate,
            DateTime? printRequestedAt,
            string printRequestedBy)
        {
            var response = new GetCertificateByIdResponse
            {
                OrganisationId = organisationId,
                LearnerFamilyName = learnerFamilyName,
                LearnerGivenNames = learnerGivenNames,
                Uln = uln,
                Type = certificateType,
                StandardReference = standardReference,
                StandardName = standardName,
                StandardLevel = standardLevel,
                AchievementDate = achievementDate,
                OverallGrade = overallGrade,
                ProviderName = providerName,
                ProviderUkPrn = providerUkPrn,
                LearningStartDate = learningStartDate,
                PrintRequestedAt = printRequestedAt,
                PrintRequestedBy = printRequestedBy,
                CertificateData = new CertificateData
                {
                    EmployerName = employerName
                }
            };

            response.OrganisationId.Should().Be(organisationId);
            response.LearnerFamilyName.Should().Be(learnerFamilyName);
            response.LearnerGivenNames.Should().Be(learnerGivenNames);
            response.Uln.Should().Be(uln);
            response.Type.Should().Be(certificateType);
            response.StandardReference.Should().Be(standardReference);
            response.StandardName.Should().Be(standardName);
            response.StandardLevel.Should().Be(standardLevel);
            response.AchievementDate.Should().Be(achievementDate);
            response.OverallGrade.Should().Be(overallGrade);
            response.ProviderName.Should().Be(providerName);
            response.ProviderUkPrn.Should().Be(providerUkPrn);
            response.CertificateData.EmployerName.Should().Be(employerName);
            response.LearningStartDate.Should().Be(learningStartDate);
            response.PrintRequestedAt.Should().Be(printRequestedAt);
            response.PrintRequestedBy.Should().Be(printRequestedBy);
        }
    }
}
