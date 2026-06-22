using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SFA.DAS.DigitalCertificates.Application.Queries.GetCertificates;
using SFA.DAS.DigitalCertificates.Application.Queries.GetCertificatesMatch;
using SFA.DAS.DigitalCertificates.Configuration;

namespace SFA.DAS.DigitalCertificates.Api.Attributes
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Method)]
    public class PrivateBetaUlnWhitelistAttribute : TypeFilterAttribute
    {
        public PrivateBetaUlnWhitelistAttribute() : base(typeof(PrivateBetaUlnWhitelistFilter))
        {
        }
    }

    public class PrivateBetaUlnWhitelistFilter : IActionFilter
    {
        private readonly PrivateBetaConfiguration _configuration;
        private readonly HashSet<long> _allowedUlns;
        private readonly ILogger<PrivateBetaUlnWhitelistFilter> _logger;

        public PrivateBetaUlnWhitelistFilter(
            DigitalCertificatesConfiguration configuration,
            ILogger<PrivateBetaUlnWhitelistFilter> logger)
        {
            _configuration = configuration.PrivateBetaConfiguration ?? new PrivateBetaConfiguration();
            _allowedUlns = (_configuration.AllowedUlns ?? []).ToHashSet();
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                return;
            }

            if (context.Result is not ObjectResult objectResult)
            {
                return;
            }

            if (!_configuration.WhitelistEnabled)
            {
                return;
            }

            var ulns = GetUlns(objectResult.Value).Distinct().ToList();

            if (!ulns.Any())
            {
                return;
            }

            var disallowedUlns = ulns
                .Where(uln => !_allowedUlns.Contains(uln))
                .ToList();

            if (!disallowedUlns.Any())
            {
                return;
            }

            _logger.LogError(
                "Private beta ULN whitelist check failed. Disallowed ULNs: {DisallowedUlns}",
                string.Join(",", disallowedUlns));

            context.Result = new StatusCodeResult(500);
        }

        private static IEnumerable<long> GetUlns(object result)
        {
            switch (result)
            {
                case GetCertificatesResult certificatesResult:
                    if (certificatesResult.Authorisation != null)
                    {
                        yield return certificatesResult.Authorisation.Uln;
                    }

                    break;

                case GetCertificatesMatchResult certificatesMatchResult:
                    foreach (var uln in certificatesMatchResult.Matches.Select(m => m.Uln))
                    {
                        yield return uln;
                    }

                    break;
            }
        }
    }
}
