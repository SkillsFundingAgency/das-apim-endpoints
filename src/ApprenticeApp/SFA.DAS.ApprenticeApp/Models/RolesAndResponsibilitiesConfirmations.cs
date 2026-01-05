using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Models
{
    [Flags]
    public enum RolesAndResponsibilitiesConfirmations : byte
    {
        None = 0,
        ApprenticeRolesAndResponsibilitiesConfirmed = 1,
        EmployerRolesAndResponsibilitiesConfirmed = 2,
        ProviderRolesAndResponsibilitiesConfirmed = 4
    }

    public static class RolesAndResponsibilitiesConfirmationsExtensions
    {
        public static bool IsConfirmed(this RolesAndResponsibilitiesConfirmations? confirmationsValue)
        {
            if (confirmationsValue == null)
                return false;

            var confirmations = confirmationsValue.Value;
            if (confirmations.HasFlag(RolesAndResponsibilitiesConfirmations.ApprenticeRolesAndResponsibilitiesConfirmed) &&
                confirmations.HasFlag(RolesAndResponsibilitiesConfirmations.EmployerRolesAndResponsibilitiesConfirmed) &&
                confirmations.HasFlag(RolesAndResponsibilitiesConfirmations.ProviderRolesAndResponsibilitiesConfirmed))
            {
                return true;
            }

            return false;
        }
    }
}

