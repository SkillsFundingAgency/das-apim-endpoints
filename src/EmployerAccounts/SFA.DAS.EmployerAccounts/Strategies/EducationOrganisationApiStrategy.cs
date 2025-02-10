﻿using System.Threading.Tasks;
using SFA.DAS.EmployerAccounts.Application.Queries.GetLatestDetails;
using SFA.DAS.EmployerAccounts.Helpers;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EducationalOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EducationalOrganisation;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Strategies
{
    public class EducationOrganisationApiStrategy(
        IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration> eduOrgApi)
        : IOrganisationApiStrategy
    {
        public async Task<GetLatestDetailsResult> GetOrganisationDetails(string identifier, OrganisationType orgType)
        {
            var eduResponse = await eduOrgApi.GetWithResponseCode<GetLatestDetailsForEducationalOrgResponse>(new GetLatestDetailsForEducationalOrgRequest(identifier));
            OrganisationApiResponseHelper.CheckApiResponseStatus(eduResponse.StatusCode, orgType, identifier, eduResponse.ErrorContent);
            var eduOrgDetail = eduResponse.Body.EducationalOrganisation;

            return new GetLatestDetailsResult
            {
                OrganisationDetail = eduOrgDetail
            };
        }
    }
}
