using System;

namespace SFA.DAS.ApprenticePortal.Models
{
    [Flags]
    public enum RolesAndResponsibilitiesConfirmations : byte
    {
        ApprenticeRolesAndResponsibilitiesConfirmed = 1,
        EmployerRolesAndResponsibilitiesConfirmed = 2,
        ProviderRolesAndResponsibilitiesConfirmed = 4
    }
}
