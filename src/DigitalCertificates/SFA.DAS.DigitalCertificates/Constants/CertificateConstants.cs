namespace SFA.DAS.DigitalCertificates.Constants
{
    public static class CertificateConstants
    {
        // TODO: This constant should be removed; kept temporarily to avoid merge conflicts.
        public const string PrintRequestedBy = "apprentice";

        public static readonly (string Action, string Status)[] DeliveryInformationStatuses =
        {
            ("Submit",       "Submitted"),
            ("PrintRequest", "PrintRequested"),
            ("Reprint",      "Reprint"),
            ("Status",       "SentToPrinter"),
            ("Printed",      "Printed"),
            ("Status",       "Delivered"),
        };
    }
}
