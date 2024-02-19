using System.Net;
using SFA.DAS.EmployerAccounts.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;

namespace SFA.DAS.EmployerAccounts.Helpers
{
    public static class OrganisationApiResponseHelper
    {
        public static void CheckApiResponseStatus(HttpStatusCode responseCode, OrganisationType organisationType, string identifier, string errorContent)
        {
            if (responseCode == HttpStatusCode.NotFound)
            {
                throw new OrganisationNotFoundException(organisationType, identifier);
            }
            else if (responseCode == HttpStatusCode.BadRequest)
            {
                throw new InvalidGetOrganisationException(errorContent);
            }
        }
    }
}
