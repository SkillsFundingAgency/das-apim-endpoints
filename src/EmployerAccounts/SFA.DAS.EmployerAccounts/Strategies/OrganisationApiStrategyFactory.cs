﻿using System;
using System.Collections.Generic;
using SFA.DAS.EmployerAccounts.Configuration;
using SFA.DAS.EmployerAccounts.ExternalApi;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Strategies
{
    public interface IOrganisationApiStrategyFactory
    {
        IOrganisationApiStrategy CreateStrategy(OrganisationType orgType);
    }

    public class OrganisationApiStrategyFactory : IOrganisationApiStrategyFactory
    {
        private readonly Dictionary<OrganisationType, Func<IOrganisationApiStrategy>> _strategyFactories;

        public OrganisationApiStrategyFactory(
            ICompaniesHouseApiClient<CompaniesHouseApiConfiguration> companiesHouseApi,
            IReferenceDataApiClient<ReferenceDataApiConfiguration> refDataApi,
            IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration> eduOrgApi,
            IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration> psOrgApi)
        {
            _strategyFactories = new Dictionary<OrganisationType, Func<IOrganisationApiStrategy>>
            {
                { OrganisationType.Company, () => new CompaniesHouseApiStrategy(companiesHouseApi) },
                { OrganisationType.Charity, () => new ReferenceDataApiStrategy(refDataApi) },
                { OrganisationType.PublicSector, () => new PublicSectorOrganisationApiStrategy(psOrgApi) },
                { OrganisationType.EducationOrganisation, () => new EducationOrganisationApiStrategy(eduOrgApi) }
            };
        }

        public IOrganisationApiStrategy CreateStrategy(OrganisationType orgType)
        {
            if (_strategyFactories.TryGetValue(orgType, out var strategyFactory))
            {
                return strategyFactory.Invoke();
            }

            throw new InvalidOperationException($"No strategy found for OrganisationType: {orgType}");
        }
    }
}
