using FluentValidation;
using SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeAccounts;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace SFA.DAS.ApprenticeApp.Validators
{
    [ExcludeFromCodeCoverage]
    public class ApprenticePatchCommandValidator : AbstractValidator<ApprenticePatchCommand>
    {
        private static readonly HashSet<string> AllowedPaths = new(StringComparer.OrdinalIgnoreCase)
    {
        "/firstName",
        "/lastName",
        "/email",
        "/dateOfBirth",
        "/termsOfUseAccepted"
    };

        private static readonly Regex NameRegex = new(@"^[\p{L}\p{M} '\-]+$", RegexOptions.Compiled, TimeSpan.FromMicroseconds(500));

        public ApprenticePatchCommandValidator()
        {
            RuleFor(x => x.ApprenticeId)
                .NotEmpty();

            RuleFor(x => x.Patch)
                .NotNull()
                .WithMessage("Patch is required");

            RuleFor(x => x).Custom((command, context) =>
            {
                if (command.Patch is null)
                {
                    context.AddFailure("Patch is required");
                    return;
                }

                var json = command.Patch is JsonElement element
                    ? element.GetRawText()
                    : JsonSerializer.Serialize(command.Patch);

                using var doc = JsonDocument.Parse(json);

                if (doc.RootElement.ValueKind != JsonValueKind.Array)
                {
                    context.AddFailure("Patch must be an array of operations.");
                    return;
                }

                foreach (var op in doc.RootElement.EnumerateArray())
                {
                    if (op.ValueKind != JsonValueKind.Object)
                    {
                        context.AddFailure("Each patch operation must be an object.");
                        continue;
                    }

                    if (!op.TryGetProperty("op", out var opProp) || opProp.ValueKind != JsonValueKind.String)
                    {
                        context.AddFailure("Each patch operation must contain an 'op' value.");
                        continue;
                    }

                    var operation = opProp.GetString();
                    if (!string.Equals(operation, "replace", StringComparison.OrdinalIgnoreCase))
                    {
                        context.AddFailure($"Operation '{operation}' is not allowed.");
                        continue;
                    }

                    if (!op.TryGetProperty("path", out var pathProp) || pathProp.ValueKind != JsonValueKind.String)
                    {
                        context.AddFailure("Each patch operation must contain a 'path' value.");
                        continue;
                    }

                    var path = pathProp.GetString();
                    if (string.IsNullOrWhiteSpace(path) || !AllowedPaths.Contains(path))
                    {
                        context.AddFailure($"Patch path '{path}' is not allowed.");
                        continue;
                    }

                    if (!op.TryGetProperty("value", out var valueProp))
                    {
                        context.AddFailure($"Patch path '{path}' must include a value.");
                        continue;
                    }

                    ValidateValue(path, valueProp, context);
                }
            });
        }

        private static void ValidateValue(string path, JsonElement valueProp, ValidationContext<ApprenticePatchCommand> context)
        {
            switch (path.ToLowerInvariant())
            {
                case "/firstname":
                case "/lastname":
                    if (valueProp.ValueKind != JsonValueKind.String)
                    {
                        context.AddFailure($"{path} must be a string.");
                        return;
                    }

                    var name = valueProp.GetString() ?? string.Empty;
                    if (name.Length > 100)
                    {
                        context.AddFailure($"{path} must be 100 characters or fewer.");
                    }

                    if (!NameRegex.IsMatch(name))
                    {
                        context.AddFailure($"{path} contains invalid characters.");
                    }

                    break;

                case "/email":
                    if (valueProp.ValueKind != JsonValueKind.String)
                    {
                        context.AddFailure($"{path} must be a string.");
                        return;
                    }

                    var email = valueProp.GetString() ?? string.Empty;
                    if (email.Length > 254)
                    {
                        context.AddFailure($"{path} must be 254 characters or fewer.");
                    }

                    if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(email))
                    {
                        context.AddFailure($"{path} must be a valid email address.");
                    }

                    break;

                case "/dateofbirth":
                    if (valueProp.ValueKind != JsonValueKind.String)
                    {
                        context.AddFailure($"{path} must be a date string.");
                        return;
                    }

                    var dobText = valueProp.GetString();
                    if (!DateTime.TryParse(dobText, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var dob))
                    {
                        context.AddFailure($"{path} must be a valid date.");
                        return;
                    }

                    if (dob > DateTime.UtcNow.Date)
                    {
                        context.AddFailure($"{path} cannot be in the future.");
                    }

                    break;

                case "/termsofuseaccepted":
                    if (valueProp.ValueKind is not JsonValueKind.True and not JsonValueKind.False)
                    {
                        context.AddFailure($"{path} must be a boolean.");
                    }

                    break;
            }
        }
    }
}

