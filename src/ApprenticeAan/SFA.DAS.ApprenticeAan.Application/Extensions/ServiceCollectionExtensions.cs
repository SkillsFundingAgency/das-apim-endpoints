using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.ApprenticeAan.Application.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationRegistrations(this IServiceCollection services) { services.AddMediatR(typeof(ServiceCollectionExtensions)); }
    }
}