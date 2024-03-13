using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using RestEase;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;

namespace SFA.DAS.EmployerAccounts.Infrastructure
{
    public interface IReferenceDataApiClient
    {
        [Get("")]
        [AllowAnyStatusCode]
        Task<RestEase.Response<List<Organisation>>> SearchOrganisations(string searchTerm, int maximumResults, CancellationToken cancellationToken);
        
    }
}
