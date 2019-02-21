using System;
namespace AmazonCognitoSpike.Auth
{
    public interface IAudienceAuthorityResolver
    {
        void Resolve(string organizationId, JwtBearerOptions options);
    }
}
