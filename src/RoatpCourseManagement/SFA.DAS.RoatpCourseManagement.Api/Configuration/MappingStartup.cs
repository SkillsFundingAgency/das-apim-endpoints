using AutoMapper;
using SFA.DAS.RoatpCourseManagement.Application.UkrlpData;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.RoatpCourseManagement.Api.Configuration
{
    [ExcludeFromCodeCoverage]
    public static class MappingStartup
    {
        public static void AddMappings()
        {
            Mapper.Reset();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<UkrlpVerificationDetailsProfile>();
                cfg.AddProfile<UkrlpContactPersonalDetailsProfile>();
                cfg.AddProfile<UkrlpContactAddressProfile>();
                cfg.AddProfile<UkrlpProviderAliasProfile>();
                cfg.AddProfile<UkrlpProviderContactProfile>();
                cfg.AddProfile<UkrlpProviderDetailsProfile>();
            });

            Mapper.AssertConfigurationIsValid();
        }
    }
}
