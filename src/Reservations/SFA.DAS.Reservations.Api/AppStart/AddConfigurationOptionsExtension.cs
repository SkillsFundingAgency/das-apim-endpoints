﻿using Microsoft.Extensions.Configuration;﻿
﻿using Microsoft.Extensions.DependencyInjection;﻿
﻿using Microsoft.Extensions.Options;﻿
﻿using SFA.DAS.Api.Common.Configuration;﻿
﻿using SFA.DAS.SharedOuterApi.Configuration;﻿
﻿﻿
﻿namespace SFA.DAS.Reservations.Api.AppStart;﻿
﻿﻿
﻿public static class AddConfigurationOptionsExtension﻿
﻿{﻿
﻿    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)﻿
﻿    {﻿
﻿        services.AddOptions();﻿
﻿        ﻿
﻿        services.Configure<CommitmentsV2ApiConfiguration>(configuration.GetSection(nameof(CommitmentsV2ApiConfiguration)));﻿
﻿        services.AddSingleton(cfg => cfg.GetService<IOptions<CommitmentsV2ApiConfiguration>>().Value);﻿
﻿        services.Configure<CoursesApiConfiguration>(configuration.GetSection(nameof(CoursesApiConfiguration)));﻿
﻿        services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesApiConfiguration>>().Value);﻿
﻿        services.Configure<AccountsConfiguration>(configuration.GetSection("AccountsInnerApi"));﻿
﻿        services.AddSingleton(cfg => cfg.GetService<IOptions<AccountsConfiguration>>().Value);﻿
﻿        services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));﻿
﻿        services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);﻿
﻿        services.Configure<EmployerProfilesApiConfiguration>(configuration.GetSection(nameof(EmployerProfilesApiConfiguration)));﻿
﻿        services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerProfilesApiConfiguration>>().Value);﻿
﻿        services.Configure<RoatpConfiguration>(configuration.GetSection(nameof(RoatpConfiguration)));﻿
﻿        services.AddSingleton(cfg => cfg.GetService<IOptions<RoatpConfiguration>>().Value);﻿
﻿        services.Configure<RoatpV2ApiConfiguration>(configuration.GetSection("RoatpConfiguration"));﻿
﻿        services.AddSingleton(cfg => cfg.GetService<IOptions<RoatpV2ApiConfiguration>>().Value);﻿
﻿        services.Configure<ProviderRelationshipsApiConfiguration>(configuration.GetSection(nameof(ProviderRelationshipsApiConfiguration)));﻿
﻿        services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderRelationshipsApiConfiguration>>().Value);﻿
﻿        services.Configure<ReservationApiConfiguration>(configuration.GetSection(nameof(ReservationApiConfiguration)));﻿
﻿        services.AddSingleton(cfg => cfg.GetService<IOptions<ReservationApiConfiguration>>().Value);﻿
﻿            services.Configure<ProviderRelationshipsApiConfiguration>(﻿
﻿                configuration.GetSection(nameof(ProviderRelationshipsApiConfiguration)));﻿
﻿            services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderRelationshipsApiConfiguration>>().Value);﻿﻿
﻿    }﻿
﻿}