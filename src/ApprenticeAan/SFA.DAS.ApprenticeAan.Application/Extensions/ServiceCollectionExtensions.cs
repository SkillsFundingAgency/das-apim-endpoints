﻿using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ApprenticeAan.Application.Regions.Queries.GetRegions;
using SFA.DAS.SharedOuterApi.Apprentice.GovUK.Auth.Application.Commands;

namespace SFA.DAS.ApprenticeAan.Application.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationRegistrations(this IServiceCollection services)
        {
            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(GetRegionsQuery).Assembly));
            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(UpsertApprenticeCommand).Assembly));
        }
    }
}