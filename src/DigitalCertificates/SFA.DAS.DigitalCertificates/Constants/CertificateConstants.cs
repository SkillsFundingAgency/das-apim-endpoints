namespace SFA.DAS.DigitalCertificates.Constants
{
    public static class CertificateConstants
    {
        public const string PrintRequestedBy = "apprentice";

        public static readonly (string Action, string Status)[] DeliveryInformationStatuses =
        {
            ("Submit",  "Submitted"),
            ("Reprint", "Reprint"),
            ("Status",  "SentToPrinter"),
            ("Printed", "Printed"),
            ("Status",  "Delivered"),
        };
    }
}
