using System;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;

namespace SFA.DAS.DigitalCertificates.Extensions
{
    public static class ContractEnumExtensions
    {
        public static CertificateType ToCertificateType(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return CertificateType.Unknown;
            }

            return ParseEnum<CertificateType>(value);
        }

        public static ActionType ToActionType(this string value)
        {
            return ParseEnum<ActionType>(value);
        }

        private static TEnum ParseEnum<TEnum>(string value) where TEnum : struct, Enum
        {
            if (!Enum.TryParse<TEnum>(value, true, out var parsed) || !Enum.IsDefined(parsed))
            {
                throw new ArgumentException($"'{value}' is not a valid {typeof(TEnum).Name}", nameof(value));
            }

            return parsed;
        }
    }
}
