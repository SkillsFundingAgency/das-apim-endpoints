using AutoFixture.Kernel;

namespace SFA.DAS.AODP.Shared.UnitTests.Helpers
{
    public class DateOnlySpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type)
            {
                if (type == typeof(DateOnly))
                    return DateOnly.FromDateTime(DateTime.UtcNow.Date);

                if (type == typeof(DateOnly?))
                    return (DateOnly?)DateOnly.FromDateTime(DateTime.UtcNow.Date);
            }

            return new NoSpecimen();
        }
    }
}
