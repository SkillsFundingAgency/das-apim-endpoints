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

            if (IsAllowed(objectResult.Value))
            {
                return;
            }

            _logger.LogError(
                "Private beta ULN whitelist check failed for response type {ResponseType}.",
                objectResult.Value?.GetType().Name);

            context.Result = new StatusCodeResult(500);
        }

        private bool IsAllowed(object result)
        {
            return result switch
            {
                GetCertificatesResult certificatesResult =>
                    IsAllowed(certificatesResult),

                GetCertificatesMatchResult certificatesMatchResult =>
                    IsAllowed(certificatesMatchResult),

                _ => true
            };
        }

        private bool IsAllowed(GetCertificatesResult certificatesResult)
        {
            if (certificatesResult.Authorisation == null)
            {
                _logger.LogInformation(
                    "Private beta ULN whitelist check skipped. No authorised ULN was available, allowing user to attempt authorisation.");

                return true;
            }

            var uln = certificatesResult.Authorisation.Uln;

            if (_allowedUlns.Contains(uln))
            {
                return true;
            }

            _logger.LogError(
                "Private beta ULN whitelist check failed. Authorised ULN {Uln} is not allowed.",
                uln);

            return false;
        }

        private bool IsAllowed(GetCertificatesMatchResult certificatesMatchResult)
        {
            var matchedUlns = certificatesMatchResult.Matches
                .Select(match => match.Uln)
                .Distinct()
                .ToList();

            if (!matchedUlns.Any())
            {
                _logger.LogError(
                    "Private beta ULN whitelist check failed. No matching ULNs were available.");

                return false;
            }

            var allowedMatchedUlns = matchedUlns
                .Where(uln => _allowedUlns.Contains(uln))
                .ToList();

            if (allowedMatchedUlns.Any())
            {
                return true;
            }

            _logger.LogError(
                "Private beta ULN whitelist check failed. None of the matched ULNs were allowed. Matched ULNs: {MatchedUlns}",
                string.Join(",", matchedUlns));

            return false;
        }
    }
}