using FluentValidation;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SFA.DAS.ApprenticeCommitments.Apis.ApprenticeAccountsApi
{
    public class UpdateApprenticeValidator :AbstractValidator<Apprentice>
    {
        private static readonly Regex ValidNameRegex = new Regex(@"^[a-zA-Z\s\-']+$", RegexOptions.Compiled, TimeSpan.FromMicroseconds(500));

        public bool IsValidPatchOperation(Operation<Apprentice> operation)
        {
            // Only validate operations that change or test a value
            if (operation.OperationType != OperationType.Replace &&
                operation.OperationType != OperationType.Add &&
                operation.OperationType != OperationType.Test)
            {
                return true; // Remove, Copy, Move don't carry a direct string value to validate
            }



            // Only validate string values
            if (!(operation.value is string stringValue))
                return true; // Non‑string values (numbers, booleans, objects) are not validated here



            // Determine which property is being targeted
            var path = operation.path?.TrimStart('/');
            if (string.IsNullOrEmpty(path))
                return true;



            // Apply validation only to known string properties that are vulnerable to XSS
            if (IsNameProperty(path))
            {
                return ValidNameRegex.IsMatch(stringValue);
            }



            // For other string properties (e.g., email, address) you may add additional validation rules.
            // For this fix we assume they are validated elsewhere or are less critical.
            return true;
        }

        private static bool IsNameProperty(string path)
        {
            // Support both direct and nested paths (e.g., "firstName", "address/line1" if needed)
            // For simplicity, we check the last segment after '/'
            var lastSegment = path.Contains('/') ? path[(path.LastIndexOf('/') + 1)..] : path;
            return lastSegment.Equals("firstName", StringComparison.OrdinalIgnoreCase) ||
                   lastSegment.Equals("lastName", StringComparison.OrdinalIgnoreCase) ||
                   lastSegment.Equals("fullName", StringComparison.OrdinalIgnoreCase);
        }
    }
}
