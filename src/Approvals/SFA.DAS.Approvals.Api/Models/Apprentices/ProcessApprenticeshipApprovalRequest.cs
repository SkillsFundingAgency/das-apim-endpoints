using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Api.Models.Apprentices;

public class ProcessApprenticeshipApprovalRequest : IValidatableObject
{
    public bool ApplyChanges { get; set; }
    public UserInfo UserInfo { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (UserInfo == null)
        {
            yield break;
        }

        if (!IsValid(UserInfo.UserId, 100, IsValidUserId))
        {
            yield return new ValidationResult(
                "User ID must be 100 characters or fewer and contain only letters, numbers, hyphens, underscores, full stops or colons.",
                new[] { $"{nameof(UserInfo)}.{nameof(UserInfo.UserId)}" });
        }

        if (!IsValid(UserInfo.UserDisplayName, 255, IsValidDisplayName))
        {
            yield return new ValidationResult(
                "User display name must be 255 characters or fewer and contain only letters, numbers, spaces or name punctuation, without markup or control characters.",
                new[] { $"{nameof(UserInfo)}.{nameof(UserInfo.UserDisplayName)}" });
        }

        if (!IsValid(UserInfo.UserEmail, 255, IsValidEmail))
        {
            yield return new ValidationResult(
                "User email must be a valid email address of 255 characters or fewer, without markup or control characters.",
                new[] { $"{nameof(UserInfo)}.{nameof(UserInfo.UserEmail)}" });
        }
    }

    private static bool IsValid(string value, int maximumLength, Func<string, bool> validate)
    {
        // User details are optional on this endpoint; validate supplied content only.
        if (string.IsNullOrEmpty(value))
        {
            return true;
        }

        if (value.Length > maximumLength || value.EnumerateRunes().Any(rune =>
                Rune.GetUnicodeCategory(rune) is UnicodeCategory.Control or
                    UnicodeCategory.LineSeparator or UnicodeCategory.ParagraphSeparator))
        {
            return false;
        }

        try
        {
            // Validate canonical Unicode without changing the caller's identity data.
            return validate(value.Normalize(NormalizationForm.FormC));
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    private static bool IsValidUserId(string value) =>
        value.All(character => char.IsAsciiLetterOrDigit(character) || character is '-' or '_' or '.' or ':');

    private static bool IsValidDisplayName(string value) =>
        value.EnumerateRunes().All(rune =>
            Rune.IsLetterOrDigit(rune) ||
            Rune.GetUnicodeCategory(rune) is UnicodeCategory.NonSpacingMark or UnicodeCategory.SpacingCombiningMark or UnicodeCategory.SpaceSeparator ||
            rune.Value is '-' or '\u2010' or '\u2011' or '\'' or '\u2019' or '.' or ',' or '(' or ')' or '/' or '&' or '\u00b7' or '\u200c' or '\u200d');

    private static bool IsValidEmail(string value) =>
        !value.Contains('<') && !value.Contains('>') &&
        !value.EnumerateRunes().Any(rune => Rune.GetUnicodeCategory(rune) == UnicodeCategory.Format) &&
        MailAddress.TryCreate(value, out var address) &&
        string.IsNullOrEmpty(address.DisplayName) &&
        string.Equals(address.Address, value, StringComparison.Ordinal);
}
