using System;

namespace SFA.DAS.Approvals.Api.Models.DataLock
{
    [Flags]
    public enum DataLockErrorCode
    {
        /// <summary>
        /// No error
        /// </summary>
        None = 0,

        /// <summary>
        /// Error with UKPRN
        /// </summary>
        Dlock01 = 1,

        /// <summary>
        /// Error with ULN
        /// </summary>
        Dlock02 = 2,

        /// <summary>
        /// Error with Standard code
        /// </summary>
        Dlock03 = 4,

        /// <summary>
        /// Error with Framework code
        /// </summary>
        Dlock04 = 8,

        /// <summary>
        /// Error with Program type
        /// </summary>
        Dlock05 = 16,

        /// <summary>
        /// Error with Pathway code
        /// </summary>
        Dlock06 = 32,

        /// <summary>
        /// Error with Cost
        /// </summary>
        Dlock07 = 64,

        /// <summary>
        /// Error with Multiple Employers
        /// </summary>
        Dlock08 = 128,

        /// <summary>
        /// Error with Start Date too early
        /// </summary>
        Dlock09 = 256,

        /// <summary>
        /// Error with Employer stopped payments
        /// </summary>
        Dlock10 = 512
    }
}