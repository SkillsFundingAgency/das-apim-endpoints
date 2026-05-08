namespace SFA.DAS.Aodp.Validation
{
    public static class ValidationPatterns
    {
        // Structural - must match this shape
        public static class Format
        {
            public const string QualificationNumber =
                @"^(?:\s*|\d{8}|\d{7}[A-Za-z]|\d{3}\/\d{4}\/(?:\d|[A-Za-z]))$";
        }

        // Text - allowed characters
        public static class Text
        {
            public const string Title =
                @"^[A-Za-z0-9 \-'\.&/(),:;]+$";

            public const string PersonName =
                @"^[A-Za-z \-']+$";

        }
    }
}
