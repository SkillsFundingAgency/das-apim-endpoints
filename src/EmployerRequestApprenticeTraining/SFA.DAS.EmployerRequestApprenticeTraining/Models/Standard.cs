using System.Collections.Generic;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Models
{
    public class Standard
    {
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public string StandardSector { get; set; }

        public static explicit operator Standard(InnerApi.Responses.StandardResponse source)
        {
            if (source == null) return null;

            return new Standard()
            {
                StandardReference = source.StandardReference,
                StandardTitle = source.StandardTitle,   
                StandardLevel = source.StandardLevel,
                StandardSector = source.StandardSector,
            };
        }
    }
}
