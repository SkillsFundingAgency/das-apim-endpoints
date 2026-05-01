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
using SFA.DAS.DigitalCertificates.Enums;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetCertificatesMatch
{
    public class GetCertificatesMatchQueryHandler : IRequestHandler<GetCertificatesMatchQuery, GetCertificatesMatchResult>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;
        private readonly Configuration.DigitalCertificatesConfiguration _configuration;

        public GetCertificatesMatchQueryHandler(
            IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient,
            IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient,
            Configuration.DigitalCertificatesConfiguration configuration)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
            _assessorsApiClient = assessorsApiClient;
            _configuration = configuration;
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
                .Where(m => IsCertificateType(m.CertificateType, CertificateType.Standard))
                .Select(m => m.Uln)
                .Union(excludedUlns)
                .Distinct()
                .ToList();

            var frameworkUlns = allMatches
                .Where(m => IsCertificateType(m.CertificateType, CertificateType.Framework))
                .Select(m => m.Uln)
                .Union(excludedUlns)
                .Distinct()
                .ToList();

            // Step 4: Retrieve masking data for standard and/or framework certificates
            var standardMasksList = new List<CertificateMaskResult>();
            var frameworkMasksList = new List<CertificateMaskResult>();

            // Retrieve masks in parallel where applicable
            var standardTask = standardUlns.Count > 0
                ? _assessorsApiClient.GetWithResponseCode<GetCertificateMasksResponse>(new GetStandardCertificateMasksRequest(standardUlns))
                : Task.FromResult<ApiResponse<GetCertificateMasksResponse>>(null);

            var frameworkTask = frameworkUlns.Count > 0
                ? _assessorsApiClient.GetWithResponseCode<GetCertificateMasksResponse>(new GetFrameworkCertificateMasksRequest(frameworkUlns))
                : Task.FromResult<ApiResponse<GetCertificateMasksResponse>>(null);

            await Task.WhenAll(standardTask, frameworkTask);

            var masksResponse = standardTask.Result;
            if (masksResponse != null && masksResponse.StatusCode != HttpStatusCode.NotFound)
            {
                masksResponse.EnsureSuccessStatusCode();
                if (masksResponse.Body?.Masks != null)
                {
                    standardMasksList.AddRange(masksResponse.Body.Masks.Select(m => (CertificateMaskResult)m));
                }
            }

            var frameworkMasksResponse = frameworkTask.Result;
            if (frameworkMasksResponse != null && frameworkMasksResponse.StatusCode != HttpStatusCode.NotFound)
            {
                frameworkMasksResponse.EnsureSuccessStatusCode();
                if (frameworkMasksResponse.Body?.Masks != null)
                {
                    frameworkMasksList.AddRange(frameworkMasksResponse.Body.Masks.Select(m => (CertificateMaskResult)m));
                }
            }

            var maxMasks = _configuration?.MaxMasks ?? 5;
            var standardCount = _configuration?.StandardMaskCount ?? 3;

            // Select masks using explicit selection logic: take up to `standardCount` from standard, remaining from framework, total `maxMasks`
            var selectedMasks = SelectMasks(standardMasksList, frameworkMasksList, maxMasks, standardCount);

            return new GetCertificatesMatchResult
            {
                Matches = allMatches,
                Masks = selectedMasks
            };
        }

        private static bool IsCertificateType(string value, CertificateType expected)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            return System.Enum.TryParse<CertificateType>(value, true, out var parsed) && parsed == expected;
        }

        private static List<CertificateMaskResult> SelectMasks(
            List<CertificateMaskResult> standardMasks,
            List<CertificateMaskResult> frameworkMasks,
            int maxMasks,
            int standardCount)
        {
            var result = new List<CertificateMaskResult>();

            // Take up to `standardCount` from standard first 
            var takeStandard = Math.Min(standardCount, maxMasks);
            result.AddRange(standardMasks.Take(takeStandard));

            // Fill remaining slots from framework
            if (result.Count < maxMasks)
                result.AddRange(frameworkMasks.Take(maxMasks - result.Count));

            // If still slots remaining, backfill from remaining standard
            if (result.Count < maxMasks)
                result.AddRange(standardMasks.Skip(takeStandard).Take(maxMasks - result.Count));

            return result.Take(maxMasks).ToList();
        }
    }
}
