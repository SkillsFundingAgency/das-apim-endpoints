using System;
using System.Collections.Generic;
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

        public OrganisationApiStrategyFactory(IReferenceDataApiClient<ReferenceDataApiConfiguration> refDataApi,
        IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration> eduOrgApi)
        {
            _strategyFactories = new Dictionary<OrganisationType, Func<IOrganisationApiStrategy>>
            {
                { OrganisationType.Company, () => new ReferenceDataApiStrategy(refDataApi) },
                { OrganisationType.Charity, () => new ReferenceDataApiStrategy(refDataApi) },
                { OrganisationType.PublicSector, () => new ReferenceDataApiStrategy(refDataApi) },
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
