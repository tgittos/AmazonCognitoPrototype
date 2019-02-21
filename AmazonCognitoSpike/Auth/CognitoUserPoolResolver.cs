using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace AmazonCognitoSpike.Auth
{

    public class CognitoUserPoolResolver : IAudienceAuthorityResolver
    {
        private class AudienceAuthorityPair
        {
            public string Audience { get; set; }
            public string Authority { get; set; }
        }

        public CognitoUserPoolResolver()
        {
        }

        public void Resolve(string organizationId, JwtBearerOptions options)
        {
            var orgMap = new Dictionary<string, AudienceAuthorityPair>
            {
                { "us-east-2_Lo9UKkZM2", new AudienceAuthorityPair { Audience = "13vons313o3s04lfv68jjc8lqe", Authority = "https://cognito-idp.us-east-2.amazonaws.com/us-east-2_Lo9UKkZM2" }},
                { "us-east-2_lqL388S1H", new AudienceAuthorityPair { Audience = "4jqk2qmccj2kh8mu99cqmbutu6", Authority = "https://cognito-idp.us-east-2.amazonaws.com/us-east-2_lqL388S1H" }}

            };

            var audienceAuthority = orgMap[organizationId];
            ConfigureOnceResolved(audienceAuthority.Audience, audienceAuthority.Authority, options);
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
