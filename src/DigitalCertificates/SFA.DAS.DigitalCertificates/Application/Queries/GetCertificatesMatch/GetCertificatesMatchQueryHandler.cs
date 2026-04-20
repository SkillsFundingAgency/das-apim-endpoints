using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetCertificatesMatch
{
    public class GetCertificatesMatchQueryHandler : IRequestHandler<GetCertificatesMatchQuery, GetCertificatesMatchResult>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;

        public GetCertificatesMatchQueryHandler(
            IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient,
            IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
            _assessorsApiClient = assessorsApiClient;
        }

        public async Task<GetCertificatesMatchResult> Handle(GetCertificatesMatchQuery request, CancellationToken cancellationToken)
        {
            // Step 1: Lookup user identity from Digi Certs Inner API
            var identityResponse = await _digitalCertificatesApiClient
                .GetWithResponseCode<GetUserIdentityResponse>(new GetUserIdentityRequest(request.UserId));

            if (identityResponse == null || identityResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return new GetCertificatesMatchResult();
            }

            identityResponse.EnsureSuccessStatusCode();
            var identity = identityResponse.Body;

            // If the user is already authorised return 204 (signal via null result)
            if (identity.Authorisation != null)
            {
                return null;
            }

            var excludedUlns = identity.Excluded ?? new List<long>();

            // Step 2: Search for certificate matches for each family name + date of birth
            var allMatches = new List<CertificateMatchResult>();

            if (identity.Identity != null && identity.DateOfBirth.HasValue)
            {
                var familyNames = identity.Identity
                    .Select(i => i.FamilyName)
                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    .Distinct()
                    .ToList();

                foreach (var familyName in familyNames)
                {
                    var searchResponse = await _assessorsApiClient
                        .GetWithResponseCode<GetCertificateSearchResponse>(
                            new GetCertificateSearchRequest(identity.DateOfBirth.Value, familyName, excludedUlns));

                    if (searchResponse == null || searchResponse.StatusCode == HttpStatusCode.NotFound)
                    {
                        continue;
                    }

                    searchResponse.EnsureSuccessStatusCode();

                    if (searchResponse.Body?.Matches != null)
                    {
                        allMatches.AddRange(searchResponse.Body.Matches.Select(m => (CertificateMatchResult)m));
                    }
                }
            }

            if (allMatches.Count == 0)
            {
                return new GetCertificatesMatchResult();
            }

            // Step 3: Collect all ULNs (new matches plus previously excluded) for mask retrieval

            var standardUlns = allMatches
                .Where(m => m.CertificateType == "Standard")
                .Select(m => m.Uln)
                .Union(excludedUlns)
                .Distinct()
                .ToList();

            var frameworkUlns = allMatches
                .Where(m => m.CertificateType == "Framework")
                .Select(m => m.Uln)
                .Union(excludedUlns)
                .Distinct()
                .ToList();

            // Step 4: Retrieve masking data for standard and/or framework certificates
            var allMasks = new List<CertificateMaskResult>();

            if (standardUlns.Count > 0)
            {
                var masksResponse = await _assessorsApiClient
                    .GetWithResponseCode<GetCertificateMasksResponse>(new GetStandardCertificateMasksRequest(standardUlns));

                if (masksResponse != null && masksResponse.StatusCode != HttpStatusCode.NotFound)
                {
                    masksResponse.EnsureSuccessStatusCode();
                    if (masksResponse.Body?.Masks != null)
                    {
                        allMasks.AddRange(masksResponse.Body.Masks.Select(m => (CertificateMaskResult)m));
                    }
                }
            }

            if (frameworkUlns.Count > 0)
            {
                var frameworkMasksResponse = await _assessorsApiClient
                    .GetWithResponseCode<GetCertificateMasksResponse>(new GetFrameworkCertificateMasksRequest(frameworkUlns));

                if (frameworkMasksResponse != null && frameworkMasksResponse.StatusCode != HttpStatusCode.NotFound)
                {
                    frameworkMasksResponse.EnsureSuccessStatusCode();
                    if (frameworkMasksResponse.Body?.Masks != null)
                    {
                        allMasks.AddRange(frameworkMasksResponse.Body.Masks.Select(m => (CertificateMaskResult)m));
                    }
                }
            }

            return new GetCertificatesMatchResult
            {
                Matches = allMatches,
                Masks = allMasks.Take(5).ToList()
            };
        }
    }
}
