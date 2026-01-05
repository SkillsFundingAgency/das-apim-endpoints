using System.Diagnostics.CodeAnalysis;
using System.Net.Security;
using System.Security.Authentication;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace SFA.DAS.EarlyConnect.Api;

[ExcludeFromCodeCoverage]
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .ConfigureKestrel(serverOptions =>
                    {
                        // Disable server identification header
                        serverOptions.AddServerHeader = false;
                        
                        // Disable CBC mode ciphers
                        serverOptions.ConfigureHttpsDefaults(httpsOptions =>
                        {
                            httpsOptions.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
                            httpsOptions.OnAuthenticate = (context, sslOptions) =>
                            {
                                sslOptions.CipherSuitesPolicy = new CipherSuitesPolicy(
                                    new[] {
                                        // TLS 1.3 ciphers
                                        TlsCipherSuite.TLS_AES_128_GCM_SHA256,
                                        TlsCipherSuite.TLS_AES_256_GCM_SHA384,
                                        TlsCipherSuite.TLS_CHACHA20_POLY1305_SHA256,
                                        
                                        // TLS 1.2 non-CBC ciphers
                                        TlsCipherSuite.TLS_ECDHE_ECDSA_WITH_AES_128_GCM_SHA256,
                                        TlsCipherSuite.TLS_ECDHE_ECDSA_WITH_AES_256_GCM_SHA384,
                                        TlsCipherSuite.TLS_ECDHE_RSA_WITH_AES_128_GCM_SHA256,
                                        TlsCipherSuite.TLS_ECDHE_RSA_WITH_AES_256_GCM_SHA384
                                    }
                                );
                            };
                        });
                    })
                    .UseStartup<Startup>();
            });
}