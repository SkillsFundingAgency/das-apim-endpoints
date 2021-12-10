using System;

namespace SFA.DAS.ApprenticePortal.Models
{
    [Flags]
    public enum RolesAndResponsibilitiesConfirmations : byte
    {
        None = 0,
        ApprenticeRolesAndResponsibilitiesConfirmed = 1,
        EmployerRolesAndResponsibilitiesConfirmed = 2,
        ProviderRolesAndResponsibilitiesConfirmed = 4
    }
}
