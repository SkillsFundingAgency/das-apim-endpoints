using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Strategies;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Strategies
{
    public class OrganisationApiStrategyFactoryTests
    {

        [Test, MoqAutoData]
        public void CreateStrategy_ShouldReturnCompaniesHouseApiStrategy_ForCompany(
        OrganisationApiStrategyFactory factory)
        {
            var orgType = OrganisationType.Company;

            // Act
            var strategy = factory.CreateStrategy(orgType);

            // Assert
            strategy.Should().BeOfType<CompaniesHouseApiStrategy>();
        }


        [Test, MoqAutoData]
        public void CreateStrategy_ShouldReturnCharitiesApiStrategy_ForCharity(
        OrganisationApiStrategyFactory factory)
        {
            var orgType = OrganisationType.Charity;

            // Act
            var strategy = factory.CreateStrategy(orgType);

            // Assert
            strategy.Should().BeOfType<CharitiesApiStrategy>();
        }


        [Test, MoqAutoData]
        public void CreateStrategy_ShouldReturnEducationOrganisationApiStrategy_ForEducationOrganisation(
        OrganisationApiStrategyFactory factory)
        {
            var orgType = OrganisationType.EducationOrganisation;

            // Act
            var strategy = factory.CreateStrategy(orgType);

            // Assert
            strategy.Should().BeOfType<EducationOrganisationApiStrategy>();
        }

        [Test, MoqAutoData]
        public void CreateStrategy_ShouldReturnPublicSectorOrganisationApiStrategy_ForPublicSectorOrganisation(
            OrganisationApiStrategyFactory factory)
        {
            var orgType = OrganisationType.PublicSector;

            // Act
            var strategy = factory.CreateStrategy(orgType);

            // Assert
            strategy.Should().BeOfType<PublicSectorOrganisationApiStrategy>();
        }


        [Test, MoqAutoData]
        public void CreateStrategy_ShouldThrowException_ForUnknownOrganisationType(
            OrganisationApiStrategyFactory factory)
        {
            Action act = () => factory.CreateStrategy(0);

            act.Should().Throw<InvalidOperationException>().WithMessage("No strategy found for OrganisationType: 0");
        }
    }
}
