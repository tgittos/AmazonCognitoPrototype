using System;
using System.Collections.Generic;
using System.Net.Http;
using AmazonCognitoSpike.Data;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Linq;

namespace AmazonCognitoSpike.Auth
{

    public class CognitoUserPoolResolver : IAudienceAuthorityResolver
    {
        private readonly DataContext db;

        public CognitoUserPoolResolver(DataContext _db)
        {
            db = _db;
        }

        public void Resolve(Guid organizationId, JwtBearerOptions options)
        {
            var org = db.Organizations.FirstOrDefault(o => o.OrganizationId == organizationId);
            ConfigureOnceResolved(org.CognitoAudience, org.CognitoAuthority, options);
        }

        private void ConfigureOnceResolved(string Audience, string Authority, JwtBearerOptions options)
        {
            if (!string.IsNullOrEmpty(Audience))
            {
                options.TokenValidationParameters.ValidAudience = Audience;
            }

            if (!string.IsNullOrEmpty(Authority))
            {
                options.MetadataAddress = Authority;
                if (!options.MetadataAddress.EndsWith("/", StringComparison.Ordinal))
                {
                    options.MetadataAddress += "/";
                }

                options.MetadataAddress += ".well-known/openid-configuration";

                if (options.RequireHttpsMetadata && !options.MetadataAddress.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("The MetadataAddress or Authority must use HTTPS unless disabled for development by setting RequireHttpsMetadata=false.");
                }

                var httpClient = new HttpClient(options.BackchannelHttpHandler ?? new HttpClientHandler());
                httpClient.Timeout = options.BackchannelTimeout;
                httpClient.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB

                options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(options.MetadataAddress, new OpenIdConnectConfigurationRetriever(),
                    new HttpDocumentRetriever(httpClient) { RequireHttps = options.RequireHttpsMetadata });
            }
        }
    }
}
