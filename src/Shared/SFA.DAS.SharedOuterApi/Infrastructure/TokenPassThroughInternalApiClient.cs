using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SFA.DAS.SharedOuterApi.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using SFA.DAS.SharedOuterApi.Exceptions;

namespace SFA.DAS.SharedOuterApi.Infrastructure
{
    public class TokenPassThroughInternalApiClient<T> : ApiClient<T>, ITokenPassThroughInternalApiClient<T> where T : ITokenPassThroughApiConfiguration
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<TokenPassThroughInternalApiClient<T>> _logger;
        private static int _expiryTimeSeconds = 30;
        private const string _serviceBearerTokenKey = "ServiceBearerToken";

        /// <summary>
        /// ApiClient used for requests to APIs that require tokens with account-level claims for Authorization.
        /// </summary>
        public TokenPassThroughInternalApiClient(
            IHttpClientFactory httpClientFactory,
            T apiConfiguration,
            IHttpContextAccessor httpContextAccessor,
            ILogger<TokenPassThroughInternalApiClient<T>> logger) : base(httpClientFactory, apiConfiguration)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        /// <summary>
        /// Applies a bearer token to the httpContext for the outgoing request
        /// </summary>
        public void GenerateServiceToken(string serviceAccount)
        {
            _httpContextAccessor.HttpContext.Items[_serviceBearerTokenKey] = GetBearerToken(serviceAccount);
        }

        /// <summary>
        /// Stores the bearer token from the incoming request of the current <see cref="HttpContext"/> to the Authorization header of the outgoing request
        /// </summary>
        protected override Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
        {
            if (_httpContextAccessor.HttpContext.Items?.ContainsKey(_serviceBearerTokenKey) == true)
            {
                var serviceBearerToken = _httpContextAccessor.HttpContext.Items[_serviceBearerTokenKey].ToString();
                httpRequestMessage.Headers.Add("Authorization", $"Bearer {serviceBearerToken}");
                _logger.LogInformation("Service Bearer token attached to request message.");
                return Task.CompletedTask;
            }

            var authHeader = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader))
            {
                httpRequestMessage.Headers.Add("Authorization", authHeader);
                _logger.LogInformation("X-Forwarded-Authorization header attached to request message.");
                return Task.CompletedTask;
            }

            authHeader = _httpContextAccessor.HttpContext.Request.Headers.Authorization.FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader))
            {
                httpRequestMessage.Headers.Add("Authorization", authHeader);
                _logger.LogInformation("Authorization header attached to request message.");
                return Task.CompletedTask;
            }

            _logger.LogWarning("No bearer Token Found in any headers or context, therefore no Authorization header was attached to request message.");               

            return Task.CompletedTask;
        }

        private string GetBearerToken(string serviceAccount)
        {
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                throw new AuthException("Cannot generate service token as the Authorization header is present");
            }

            var claims = new JwtSecurityTokenHandler().ReadJwtToken(authorizationHeader.Replace("Bearer ", string.Empty)).Claims.ToList();
            claims.Add(new Claim("serviceAccount", serviceAccount));

            ValidateSigningKey(Configuration.BearerTokenSigningKey);

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.BearerTokenSigningKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: creds,
                expires: DateTime.UtcNow.AddSeconds(_expiryTimeSeconds)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        private static void ValidateSigningKey(string? signingKey)
        {
            if (string.IsNullOrEmpty(signingKey))
            {
                throw new AuthException("BearerTokenSigningKey cannot be null or empty when using TokenPassThroughInternalApiClient to generate service tokens");
            }

            const int minimumKeySize = 128;
            if (signingKey.Length * 8 < minimumKeySize)
            {
                // This checks the key is at least 128 bits long, otherwise the token will fail to be generated
                throw new AuthException("Signing key must be at least 128bits in length");
            }
        }
    }
}